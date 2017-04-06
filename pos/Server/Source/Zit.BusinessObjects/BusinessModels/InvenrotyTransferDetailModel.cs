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
    public class InvenrotyTransferDetailModel : ObservableObjectBase
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

        private int _qty;
        [DataMember]
        public int Qty
        {
            get { return _qty; }
            set { 
                SetProperty(ref _qty, value);
            }
        }

        private int _transferId;
        [DataMember]
        public int TransferId
        {
            get { return _transferId; }
            set { SetProperty(ref _transferId, value); }
        }

        private int _seq;
        [DataMember]
        public int Seq
        {
            get { return _seq; }
            set { SetProperty(ref _seq, value); }
        }
    }
}
