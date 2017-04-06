using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects.Enums;
using Zit.Entity;

namespace Zit.BusinessObjects.BusinessModels
{
    [DataContract]
    public class InventoryTransferModel : ObservableObjectBase
    {
        private int _id;
        [DataMember]
	    public int Id
	    {
		    get { return _id;}
            set { SetProperty(ref _id, value); IsNew = _id == 0; }
	    }

        private int _storeId;
        [DataMember]
        public int StoreId
        {
            get { return _storeId; }
            set { SetProperty(ref _storeId, value);}
        }

        private int _toStoreId;
        [DataMember]
        public int ToStoreId
        {
            get { return _toStoreId; }
            set { SetProperty(ref _toStoreId, value); }
        }
	
        private string _transferNo;
        [DataMember]
        public string TransferNo
	    {
            get { return _transferNo; }
            set { SetProperty(ref _transferNo, value); }
	    }

        //Only For Report Binding
        [IgnoreDataMember]
        public byte[] TransferNoBarcode { get; set; }
	
        private string _desc;
        [DataMember]
		public string Desc
		{
			get { return _desc;}
			set { SetProperty(ref _desc, value);}
		}
	
        private DateTime _transferDate;
        [DataMember]
		public DateTime TransferDate
		{
            get { return _transferDate; }
            set { SetProperty(ref _transferDate, value); }
		}

        private bool _isNew;
        [IgnoreDataMember]
        public bool IsNew
        {
            get { return _isNew; }
            set { SetProperty(ref _isNew, value); }
        }
	
        private int _qty;
        [DataMember]
        public int Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private string _createdUser;
        [DataMember]
        public string CreatedUser
        {
            get { return _createdUser; }
            set { SetProperty(ref _createdUser, value); }
        }

        private ZitObservableCollection<InvenrotyTransferDetailModel> _details;
        [DataMember]
        public ZitObservableCollection<InvenrotyTransferDetailModel> Detail
        {
            get { return _details; }
            set {

                if (_details != null)
                {
                    _details.CollectionChanged -= Detail_CollectionChanged;
                }

                SetProperty(ref _details, value);

                if (_details != null)
                {
                    _details.CollectionChanged += Detail_CollectionChanged;
                    Qty = _details.Sum(m => m.Qty);
                }
            }
        }

        void Detail_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Qty = Detail.Sum(m => m.Qty);
        }
        
    }
}
