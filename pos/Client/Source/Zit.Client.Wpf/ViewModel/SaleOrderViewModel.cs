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
using Sendo.Web.Mvc4.Infractstructure;

namespace Zit.Client.Wpf.ViewModel
{
    public class SaleOrderViewModel : FormViewModelBase
    {
        private Func<IZitServices> _service = () =>
        {
            return ServiceLocator.Current.GetInstance<IZitServices>();
        };

        private bool isFirstLoad = true;

        public SaleOrderViewModel(IZitServices service)
        {
            Title = "Đơn hàng bán lẻ";

            if (IsInDesignMode)
            {

            }
            else
            {
                Model = new POSSaleOrderModel();

                UpdateInfo = new RelayCommand(__updateInfo);
                Complete = new RelayCommand(__complete);
                RemoveDetail = new RelayCommand<int>(__removeDetail, (a) => { return Model.IsNew; });
                Print = new RelayCommand(__print);
                ReturnOrder = new RelayCommand(__returnOrder);
                NewOrder = new RelayCommand(__initNewOrder);
                FormLoad = new RelayCommand<object>(__formLoad);
                __regEvent();
            }
        }

        ~SaleOrderViewModel()
        {
            __unRegEvent();
        }

        List<CF_Obj> _objs;
        public List<CF_Obj> Objs
        {
            get { return _objs; }
            set { _objs = value; RaisePropertyChanged("Objs"); }
        }

        List<CF_SaleReturnReason> _saleReturnReasons;
        public List<CF_SaleReturnReason> SaleReturnReasons
        {
            get { return _saleReturnReasons; }
            set { _saleReturnReasons = value; RaisePropertyChanged("SaleReturnReasons"); }
        }

        List<CF_SaleSource> _saleSources;
        public List<CF_SaleSource> SaleSources
        {
            get { return _saleSources; }
            set { _saleSources = value; RaisePropertyChanged("SaleSources"); }
        }

        List<CF_Carrier> _carriers;
        public List<CF_Carrier> Carriers
        {
            get { return _carriers; }
            set { _carriers = value; RaisePropertyChanged("Carriers"); }
        }

        #region Privates

        private void __setNew()
        {
            Model.IsNew = true;
            Model.IsEdit = false;
            Model.IsReturn = false;
            ModeName = "Tạo mới";
        }

        private void __setEdit()
        {
            Model.IsNew = false;
            Model.IsEdit = true;
            Model.IsReturn = false;
            ModeName = "Chỉnh sửa/In";
        }

        private void __setReturn()
        {
            Model.IsNew = true;
            Model.IsEdit = false;
            Model.IsReturn = true;
            ModeName = "Trả hàng";
            Model.DiscountAmount = - Model.Discount;
            Model.ReceiveMoney = 0;
            Model.AeroShippingFee = 0;
            Model.ShippingFee = 0;
        }

        private void __formLoad(object obj)
        {
            if (isFirstLoad)
            {
                isFirstLoad = false;
                __initMasterData();
                __initNewOrder();
            }
        }

        private void __initMasterData()
        {
            Carriers = _service().GetAllCarrier().Data.ToList();
            Objs = _service().GetAllObj().Data.ToList();
            SaleSources = _service().GetAllSaleSource().Data.ToList();
            SaleReturnReasons = _service().GetAllSaleReturnReason().Data.ToList();
        }

        private void __initNewOrder()
        {
            var rp = _service().CreateNewPOSOrder();
            if (rp.HasError)
            {
                MessageBox.Show("Không thể khởi tạo đơn hàng mới");
            }
            else
            {
                __setNew();
                Model = Mapper.Map<POSSaleOrderModel>(rp.Data);
                var mainvm = ServiceLocator.Current.GetInstance<MainViewModel>();
                Model.Users = mainvm.Users.Aggregate((a, b) => { return a + "," + b; });
            }
        }

        private void __loadOrder(string orderNumber)
        {
            var rp = _service().GetPOSSaleOrderByOrderNo(orderNumber);
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
                    if (Model.IsReturn)
                    {
                        __complete();
                    }
                    else
                    {
                        if (Model.IsNew)
                        {
                            __complete();
                        }
                        else
                        {
                            __initNewOrder();
                        }
                    }
                    break;
                default:
                    if (data.Data.IndexOf("PS") == 0)
                    {
                        //Is Order Barcode
                        __loadOrder(data.Data);
                    }
                    else if (data.Data.IndexOf("TT") == 0)
                    {
                        if (data.Data.Length > 3)
                        {
                            decimal outPut = 0;
                            decimal.TryParse(data.Data.Substring(2,data.Data.Length -2),out outPut);
                            Model.ReceiveMoney = outPut;
                        }
                    }
                    else if (data.Data.IndexOf("PVC") == 0)
                    {
                        if (data.Data.Length > 4)
                        {
                            decimal outPut = 0;
                            decimal.TryParse(data.Data.Substring(3, data.Data.Length - 3), out outPut);
                            Model.ShippingFee = outPut;
                        }
                    }
                    else
                    {
                        if (Model.IsEdit)
                        {
                            __initNewOrder();
                        }
                        __processBarcodeProduct(data);
                    }
                    break;
            }
        }

        private void __processBarcodeProduct(BarcodeDataMsg data)
        {
            if (Model.IsReturn)
            { 
                //Check Barcode
                var barcode = data.Data;
                var item = Model.Detail.Where(m => m.Barcode == barcode).FirstOrDefault();
                if (item == null)
                {
                    MessageBox.Show("Sản phẩm không hợp lệ để trả hàng, Mã không đúng");
                }
                else
                {
                    var returnItem = new POSSaleOrderDetailModel();
                    Mapper.Map(item, returnItem);
                    returnItem.IsReturn = true;
                    returnItem.Qty = -1;
                    Model.Detail.Add(returnItem);
                }
            }
            else
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

        private POSSaleOrderDetailModel __getDetailByBarcode(string barcode)
        {
            var rp = _service().CreatePOSOrderDetailByBarcode(barcode);
            if(rp.HasError)
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

        private POSSaleOrderModel _model;
        public POSSaleOrderModel Model
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

        #region Command

        public ICommand FormLoad { get; private set; }
        public ICommand RemoveDetail { get; private set; }
        public ICommand Complete { get; private set; }
        public ICommand Print { get; private set; }
        public ICommand ReturnOrder { get; private set; }
        public ICommand NewOrder { get; private set; }
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
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                MessageBox.Show("Không tìm thấy máy in","Thông báo",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }

            Model.PrintDesc = string.Format("Lưu ý : Quý khách nên giữ lại hóa đơn để khi thắc mắc, khiếu nại, đổi hàng chúng tôi có căn cứ xử lý. Hạn chót đổi hàng: {0} Đổi hàng trong vòng 7 ngày. Hàng đổi phải còn nguyên nhãn mác và hóa đơn mua hàng. Mắt kính, đồng hồ, nước hoa và nữ trang không đổi"
                ,Model.OrderDate.AddDays(6).ToString("dd/MM/yyyy"));

            Model.StoreName = WorkContext.UserContext.StoreName;
            Model.StoreAddress = WorkContext.UserContext.StoreAdress;

            printDoc.PrintPage += (object sender, PrintPageEventArgs e) => 
            {
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = @"Reports\SPOrder.rdlc";
                rpt.EnableExternalImages = true;
                rpt.DataSources.Add(new ReportDataSource("ModelData", new List<POSSaleOrderModel>() {Model}));
                rpt.DataSources.Add(new ReportDataSource("ModelDataDetail", new List<POSSaleOrderDetailModel>(Model.Detail)));

                IBarcodeWriter barWriter = new BarcodeWriter()
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new ZXing.Common.EncodingOptions { 
                        PureBarcode = true,
                        Height = 100,
                        Width = 500
                    }
                };
                var bmp = barWriter.Write(Model.OrderNo);
                var converter = new ImageConverter();
                Model.OrderNoBarcode = (byte[])converter.ConvertTo(bmp,typeof(byte[]));

                string deviceInfo =
                                  string.Format(@"<DeviceInfo>
                                        <OutputFormat>EMF</OutputFormat>
                                        <PageWidth>{0}in</PageWidth>
                                        <PageHeight>130in</PageHeight>
                                        <MarginTop>0in</MarginTop>
                                        <MarginLeft>0in</MarginLeft>
                                        <MarginRight>0in</MarginRight>
                                        <MarginBottom>0in</MarginBottom>
                                    </DeviceInfo>", (((float)e.PageBounds.Width) / 100).ToString(new CultureInfo("en")));
                Warning[] warnings;

                MemoryStream stream = new MemoryStream();
                
                rpt.Render("Image", deviceInfo, (a,b,c,d,f) => {
                    return stream;
                }, out warnings);

                stream.Position = 0;

                Metafile pageImage = new Metafile(stream);

                Rectangle adjustedRect = new Rectangle(
                    e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
                    e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
                    e.PageBounds.Width,
                    e.PageBounds.Height);

                e.Graphics.FillRectangle(Brushes.White, adjustedRect);

                e.Graphics.DrawImage(pageImage, adjustedRect);

                e.HasMorePages = false;

                stream.Dispose();
            };

            printDoc.Print();
            _service().UpdatePrintCount(Model.Id);
        }

        private void __returnOrder()
        {
            __setReturn();
        }


        private void __updateInfo()
        {
            var rp = _service().SavePOSInfo(Model);
            if (rp.HasError)
            {
                MessageBox.Show(rp.ToErrorMsg());
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin thành công");
            }
            __loadOrder(Model.OrderNo);
        }

        private void __complete()
        {
            var rp = _service().SavePOSOrder(Model);
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
                    __print();
                    
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
