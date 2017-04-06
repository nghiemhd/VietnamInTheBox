using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zit.Client.Wpf.Infractstructure;
using Zit.Client.Wpf.Messages;
using Zit.Client.Proxy.ZitServices;
using AutoMapper;
using Zit.BusinessObjects.Enums;
using Zit.DataTransferObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.Entity;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Reporting.WinForms;
using System.Drawing;
using ZXing;
using System.Threading;
using System.Globalization;
using Zit.BusinessObjects;

namespace Zit.Client.Wpf.ViewModel
{
    public class InventoryTransferViewModel : FormViewModelBase
    {
        private Func<IZitServices> _service = () =>
        {
            return ServiceLocator.Current.GetInstance<IZitServices>();
        };

        private bool isFirstLoad = true;

        public InventoryTransferViewModel(IZitServices service)
        {
            Title = "Vận đơn";

            if (IsInDesignMode)
            {

            }
            else
            {
                Model = new InventoryTransferModel();

                Complete = new RelayCommand(__complete);
                RemoveDetail = new RelayCommand<int>(__removeDetail, (a) => { return Model.IsNew; });
                Print = new RelayCommand(__print);

                NewTransfer = new RelayCommand(__initNewTransfer);
                FormLoad = new RelayCommand<object>(__formLoad);
                __regEvent();

                //Init Store
            }
        }

        ~InventoryTransferViewModel()
        {
            __unRegEvent();
        }

        #region Privates

        private void __setNew()
        {
            Model.IsNew = true;
            ModeName = "Tạo mới";
        }

        private void __setEdit()
        {
            Model.IsNew = false;
            ModeName = "Xem thông tin";
        }

        private void __formLoad(object obj)
        {
            if (isFirstLoad)
            {
                isFirstLoad = false;
                __initStore();
                __initNewTransfer();
            }
        }

        private void __initStore()
        {
            var rp = _service().GetAllStore();
            Stores = rp.Data.ToList().Where(m => m.Id != AppConfig.StoreId).ToList();
        }

        private void __initNewTransfer()
        {
            var rp = _service().CreateNewTransfer();
            if (rp.HasError)
            {
                MessageBox.Show("Không thể khởi tạo vận đơn mới");
            }
            else
            {
                __setNew();
                Model = Mapper.Map<InventoryTransferModel>(rp.Data);
            }
        }

        private void __loadOrder(string transferNumber)
        {
            var rp = _service().GetInvTransferByTransferNo(transferNumber);
            if (rp.HasError)
            {
                MessageBox.Show("Không thể lấy thông tin đơn hàng");
            }
            else
            {
                Model = rp.Data;
                __setEdit();
            }
        }

        private void __regEvent()
        {
            MessengerInstance.Register<BarcodeDataMsg>(this, this.ViewId, (data) => 
                { 
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        __barcode(data); 
                    });
                });
            MessengerInstance.Register<CommandMsg>(this, this.ViewId, __command);
        }

        private void __unRegEvent()
        {
            MessengerInstance.Unregister(this);
        }

        private void __command(CommandMsg msg)
        {
            BarcodeDataMsg barMsg = new BarcodeDataMsg();
            switch (msg.Data)
            {
                case null:
                    barMsg.Data = "ENTER";
                    break;
                default:
                    barMsg.Data = msg.Data;
                    break;
            }

            if (barMsg.Data != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    __barcode(barMsg);
                });   
            }
        }

        private void __barcode(BarcodeDataMsg data)
        {
            //Process Command
            switch (data.Data)
            {
                case "ENTER":
                    if (Model.IsNew)
                    {
                        __complete();
                    }
                    else
                    {
                        __initNewTransfer();
                    }
                    break;
                default:
                    if (data.Data.IndexOf("IT") == 0)
                    {
                        //Is Order Barcode
                        __loadOrder(data.Data);
                    }
                    else
                    {
                        if (!Model.IsNew)
                        {
                            __initNewTransfer();
                        }
                        __processBarcodeProduct(data);
                    }
                    break;
            }
        }

        private void __processBarcodeProduct(BarcodeDataMsg data)
        {
            if (Model.IsNew)
            {
                if (!string.IsNullOrWhiteSpace(data.Data))
                {
                    int qty = 1;
                    if (!string.IsNullOrWhiteSpace(data.CmdData) && int.TryParse(data.CmdData, out qty))
                    {
                        if (qty < 0)
                        {
                            //Remove
                            var forRemove = Model.Detail.Where(m => m.Barcode == data.Data).ToList();
                            foreach (var rev in forRemove)
                            {
                                if (rev.Qty > Math.Abs(qty))
                                {
                                    rev.Qty = rev.Qty + qty;

                                    break;
                                }
                                else
                                {
                                    qty = qty + rev.Qty;
                                    //Remove
                                    Model.Detail.Remove(rev);

                                    if (qty == 0) break;
                                }
                            }
                            __reSeq();
                            return;
                        }
                    }
                    //For Add
                    __addDetailByBarcode(data.Data, qty);
                }
            }
            else
            {
                MessageBox.Show("Không cho phép thay đổi đơn hàng, mã vạch quét không hợp lệ");
            }
        }

        private void __addDetailByBarcode(string barcode,int qty)
        {
            var detail = __getDetailByBarcode(barcode);
            if (detail == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm có barcode này");
            }
            else
            {
                var curr = Model.Detail.Where(m => m.Barcode == detail.Barcode).FirstOrDefault();
                if (curr != null)
                {
                    curr.Qty = curr.Qty + qty;
                }
                else
                {
                    detail.Qty = qty;
                    detail.RefObj = this;
                    Model.Detail.Add(detail);
                }
                __reSeq();
            }
        }

        private InvenrotyTransferDetailModel __getDetailByBarcode(string barcode)
        {
            var rp = _service().CreateITDetailByBarcode(barcode);
            if (rp.HasError)
            {
                MessageBox.Show("Đã có lỗi khi thực hiện, xin hãy thử lại");
                return null;
            }
            else
                return rp.Data;
        }

        private void __reSeq()
        {
            int n = 0;
            Model.Detail.ToList().ForEach(m => {
                n++;
                m.Seq = n;
            });
        }

        #endregion

        private InventoryTransferModel _model;
        public InventoryTransferModel Model
        {
            get { return _model; }
            set { _model = value; RaisePropertyChanged("Model"); }
        }

        private string _modeName;
        public string ModeName
        {
            get { return _modeName; }
            set { _modeName = value; RaisePropertyChanged("ModeName"); }
        }

        List<CF_Store> _stores;
        public List<CF_Store> Stores
        {
            get { return _stores; }
            set { _stores = value; RaisePropertyChanged("Stores"); }
        }

        #region Command

        public ICommand FormLoad { get; private set; }
        public ICommand RemoveDetail { get; private set; }
        public ICommand Complete { get; private set; }
        public ICommand Print { get; private set; }
        public ICommand ReturnOrder { get; private set; }
        public ICommand NewTransfer { get; private set; }
        public ICommand UpdateInfo { get; private set; }

        #endregion


        #region Privates

        private void __removeDetail(int seq)
        {
            var dt = Model.Detail.First(m => m.Seq == seq);
            Model.Detail.Remove(dt);
        }

        private void __print()
        {
            MessageBox.Show("Chưa hỗ trợ chức năng này");

//            PrintDocument printDoc = new PrintDocument();
//            if (!printDoc.PrinterSettings.IsValid)
//            {
//                MessageBox.Show("Không tìm thấy máy in","Thông báo",MessageBoxButton.OK,MessageBoxImage.Warning);
//                return;
//            }

//            printDoc.PrintPage += (object sender, PrintPageEventArgs e) => 
//            {
//                LocalReport rpt = new LocalReport();
//                rpt.ReportPath = @"Reports\SPOrder.rdlc";
//                rpt.EnableExternalImages = true;
//                rpt.DataSources.Add(new ReportDataSource("ModelData", new List<POSSaleOrderModel>() {Model}));
//                rpt.DataSources.Add(new ReportDataSource("ModelDataDetail", new List<POSSaleOrderDetailModel>(Model.Detail)));

//                IBarcodeWriter barWriter = new BarcodeWriter()
//                {
//                    Format = BarcodeFormat.CODE_128,
//                    Options = new ZXing.Common.EncodingOptions { 
//                        PureBarcode = true,
//                        Height = 100,
//                        Width = 500
//                    }
//                };
//                var bmp = barWriter.Write(Model.OrderNo);
//                var converter = new ImageConverter();
//                Model.OrderNoBarcode = (byte[])converter.ConvertTo(bmp,typeof(byte[]));

//                string deviceInfo =
//                                  string.Format(@"<DeviceInfo>
//                                        <OutputFormat>EMF</OutputFormat>
//                                        <PageWidth>{0}in</PageWidth>
//                                        <PageHeight>130in</PageHeight>
//                                        <MarginTop>0in</MarginTop>
//                                        <MarginLeft>0in</MarginLeft>
//                                        <MarginRight>0in</MarginRight>
//                                        <MarginBottom>0in</MarginBottom>
//                                    </DeviceInfo>", (((float)e.PageBounds.Width) / 100).ToString(new CultureInfo("en")));
//                Warning[] warnings;

//                MemoryStream stream = new MemoryStream();
                
//                rpt.Render("Image", deviceInfo, (a,b,c,d,f) => {
//                    return stream;
//                }, out warnings);

//                stream.Position = 0;

//                Metafile pageImage = new Metafile(stream);

//                Rectangle adjustedRect = new Rectangle(
//                    e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
//                    e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
//                    e.PageBounds.Width,
//                    e.PageBounds.Height);

//                e.Graphics.FillRectangle(Brushes.White, adjustedRect);

//                e.Graphics.DrawImage(pageImage, adjustedRect);

//                e.HasMorePages = false;

//                stream.Dispose();
//            };

//            printDoc.Print();
        }

        private void __complete()
        {
            var rp = _service().SaveInvTransfer(Model);
            if (rp.HasError)
            {
                MessageBox.Show(rp.ToErrorMsg());
            }
            else
            {
                Model = rp.Data;
                __setEdit();
                try
                {
                    //__print();
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
