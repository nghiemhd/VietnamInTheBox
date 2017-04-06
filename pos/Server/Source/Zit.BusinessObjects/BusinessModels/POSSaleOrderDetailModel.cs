using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Zit.Entity;

namespace Zit.BusinessObjects.BusinessModels
{
    [DataContract]
    public class POSSaleOrderDetailModel : ObservableObjectBase
    {
        private int _id;
        [DataMember]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _barcodeId;
        [DataMember]
        public int BarcodeId
        {
            get { return _barcodeId; }
            set { SetProperty(ref _barcodeId, value); }
        }

        private int _productId;
        [DataMember]
        public int ProductId
        {
            get { return _productId; }
            set { SetProperty(ref _productId, value); }
        }

        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _attributeDesc;
        [DataMember]
        public string AttributeDesc
        {
            get { return _attributeDesc; }
            set { SetProperty(ref _attributeDesc, value); }
        }

        private string _barcode;
        [DataMember]
        public string Barcode
        {
            get { return _barcode; }
            set { SetProperty(ref _barcode, value); }
        }

        private decimal _lineAmount;
        [DataMember]
        public decimal LineAmount
        {
            get { return _lineAmount; }
            set { SetProperty(ref _lineAmount, value); }
        }

        private int _lineDiscount;
        [DataMember]
        public int LineDiscount
        {
            get { return _lineDiscount; }
            set { 
                SetProperty(ref _lineDiscount, value);
                LineAmount = Qty * SellUnitPrice * (100 - LineDiscount) / 100;
            }
        }

        private int _qty;
        [DataMember]
        public int Qty
        {
            get { return _qty; }
            set { 
                SetProperty(ref _qty, value);
                LineAmount = Qty * SellUnitPrice * (100 - LineDiscount) / 100;
            }
        }

        private int _saleOrderId;
        [DataMember]
        public int SaleOrderId
        {
            get { return _saleOrderId; }
            set { SetProperty(ref _saleOrderId, value); }
        }

        private decimal _sellUnitPrice;
        [DataMember]
        public decimal SellUnitPrice
        {
            get { return _sellUnitPrice; }
            set { 
                SetProperty(ref _sellUnitPrice, value);
                LineAmount = Qty * SellUnitPrice * (100 - LineDiscount) / 100;
            }
        }

        private int _seq;
        [DataMember]
        public int Seq
        {
            get { return _seq; }
            set { SetProperty(ref _seq, value); }
        }

        private bool _isReturn;
        [DataMember]
        public bool IsReturn
        {
            get { return _isReturn; }
            set { SetProperty(ref _isReturn, value); }
        }
    }
}
