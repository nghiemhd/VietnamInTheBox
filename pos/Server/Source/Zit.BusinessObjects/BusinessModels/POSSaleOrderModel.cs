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
    public class POSSaleOrderModel : ObservableObjectBase
    {
        private int _id;
        [DataMember]
	    public int Id
	    {
		    get { return _id;}
            set { SetProperty(ref _id, value); IsNew = _id == 0; }
	    }

        private int _status;
        [DataMember]
        public int Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); IsAllowEditAeroFee = (Status <= 1); }
        }

        private int _storeId;
        [DataMember]
        public int StoreId
        {
            get { return _storeId; }
            set { SetProperty(ref _storeId, value);}
        }

        private int? _returnReasonId;
        [DataMember]
        public int? ReturnReasonId
        {
            get { return _returnReasonId; }
            set { SetProperty(ref _returnReasonId, value); }
        }

        private int _sourceId;
        [DataMember]
        public int SourceId
        {
            get { return _sourceId; }
            set { SetProperty(ref _sourceId, value); }
        }
	
        private string _orderNo;
        [DataMember]
	    public string OrderNo
	    {
		    get { return _orderNo;}
		    set { SetProperty(ref _orderNo, value);}
	    }

        //Only For Report Binding
        [IgnoreDataMember]
        public byte[] OrderNoBarcode { get; set; }
        [IgnoreDataMember]
        public string PrintDesc { get; set; }
        [IgnoreDataMember]
        public string StoreName { get; set; }
        [IgnoreDataMember]
        public string StoreAddress { get; set; }
	
        private string _desc;
        [DataMember]
		public string Desc
		{
			get { return _desc;}
			set { SetProperty(ref _desc, value);}
		}

        private string _users;
        [DataMember]
        public string Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private bool _isMasterCard;
        [DataMember]
        public bool IsMasterCard
        {
            get { return _isMasterCard; }
            set { SetProperty(ref _isMasterCard, value); }
        }

        private string _mobile;
        [DataMember]
        public string Mobile
        {
            get { return _mobile; }
            set { SetProperty(ref _mobile, value); }
        }

        private string _customerName;
        [DataMember]
        public string CustomerName
        {
            get { return _customerName; }
            set { SetProperty(ref _customerName, value); }
        }
	
        private DateTime _orderDate;
        [DataMember]
		public DateTime OrderDate
		{
			get { return _orderDate;}
			set { SetProperty(ref _orderDate, value);}
		}

        private decimal _discountPercent;
        [DataMember]
		public decimal DiscountPercent
		{
            get { return _discountPercent; }
			set {
                SetProperty(ref _discountPercent, value);

                if (value != 0 || (DiscountAmount == 0))
                {
                    SetProperty(ref _discountAmount, 0, "DiscountAmount");
                    Discount = SubTotal * value / 100;
                }
            }
		}

        private decimal _discountAmount;
        [DataMember]
        public decimal DiscountAmount
        {
            get { return _discountAmount; }
            set
            {
                SetProperty(ref _discountAmount, value);
                if (value != 0 || (DiscountPercent == 0))
                {
                    SetProperty(ref _discountPercent, 0, "DiscountPercent");
                    Discount = DiscountAmount;
                }
            }
        }

        private decimal _discount;
        [DataMember]
        public decimal Discount
        {
            get { return _discount; }
            set
            {
                SetProperty(ref _discount, value);
                Amount = SubTotal - Discount + ShippingFee;
            }
        }
	
        private string _refNo;
        [DataMember]
		public string RefNo
		{
			get { return _refNo;}
			set { SetProperty(ref _refNo, value);}
		}

        private SaleChanel _chanelId;
        [DataMember]
		public SaleChanel ChanelId
		{
			get { return _chanelId;}
			set { 
                SetProperty(ref _chanelId, value);
                //IsEnableRef = (value == SaleChanel.Aero87 || value == SaleChanel.Sendo);
                IsEnableRef = true;
                IsObjEnable = (value == SaleChanel.NoiBo) && IsNew;
                IsCarrierEnable = (value == SaleChanel.Aero87 || value == SaleChanel.Phone) && IsNew;
            }
		}

        private bool _isObjEnable;
        [IgnoreDataMember]
        public bool IsObjEnable
        {
            get { return _isObjEnable; }
            set { SetProperty(ref _isObjEnable, value); if (!value) ObjId = null; }
        }

        private int? _objId;
        [DataMember]
        public int? ObjId
        {
            get { return _objId; }
            set
            {
                SetProperty(ref _objId, value);
            }
        }

        private bool _isCarrierEnable;
        [IgnoreDataMember]
        public bool IsCarrierEnable
        {
            get { return _isCarrierEnable; }
            set { SetProperty(ref _isCarrierEnable, value); if ((!value)  && IsNew) CarrierId = null; }
        }

        private int? _carrierId;
        [DataMember]
        public int? CarrierId
        {
            get { return _carrierId; }
            set
            {
                SetProperty(ref _carrierId, value);
                if (_carrierId != null)
                    IsEnableShippingCode = true;
            }
        }

        private bool _isEnableShippingCode;

        [IgnoreDataMember]
        public bool IsEnableShippingCode
        {
            get { return _isEnableShippingCode; }
            set { SetProperty(ref _isEnableShippingCode, value); }
        }

        private string _shippingCode;
        [DataMember]
        public string ShippingCode
        {
            get { return _shippingCode; }
            set { SetProperty(ref _shippingCode, value); }
        }

        [DataMember]
        public bool IsPaid { get; set; }
        [DataMember]
        public Nullable<DateTime> PaymentDate { get; set; }

        private bool _isNew;
        [IgnoreDataMember]
        public bool IsNew
        {
            get { return _isNew; }
            set { SetProperty(ref _isNew, value); }
        }

        private bool _isAllowEditAeroFee;
        [IgnoreDataMember]
        public bool IsAllowEditAeroFee
        {
            get { return (Status <= 1); }
            set { SetProperty(ref _isAllowEditAeroFee, value); }
        }

        private bool _isEnableRef;
        [IgnoreDataMember]
        public bool IsEnableRef
        {
            get { return _isEnableRef; }
            set { SetProperty(ref _isEnableRef, value); }
        }

        private bool _isReturn;
        [IgnoreDataMember]
        public bool IsReturn
        {
            get { return _isReturn; }
            set { SetProperty(ref _isReturn, value); if (value) Detail_CollectionChanged(null, null); }
        }

        private bool _isEdit;
        [IgnoreDataMember]
        public bool IsEdit
        {
            get { return _isEdit; }
            set { SetProperty(ref _isEdit, value); }
        }
	
        private decimal _amount;
        [DataMember]
		public decimal Amount
		{
			get { return _amount;}
			set { 
                SetProperty(ref _amount, value);
                ReturnMoney = ReceiveMoney - Amount;
            }
		}

        private decimal _subTotal;
        [DataMember]
        public decimal SubTotal
        {
            get { return _subTotal; }
            set { 
                SetProperty(ref _subTotal, value);

                if (DiscountPercent != 0)
                {
                    Discount = _subTotal * DiscountPercent / 100;
                }

                Amount = SubTotal - Discount + ShippingFee;
            }
        }

        private decimal _shippingFee;
        [DataMember]
        public decimal ShippingFee
        {
            get { return _shippingFee; }
            set
            {
                SetProperty(ref _shippingFee, value);
                Amount = SubTotal - Discount + ShippingFee;
            }
        }

        private decimal _aeroshippingFee;
        [DataMember]
        public decimal AeroShippingFee
        {
            get { return _aeroshippingFee; }
            set
            {
                SetProperty(ref _aeroshippingFee, value);
            }
        }

        private int _qty;
        [DataMember]
        public int Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal _receiveMoney;
        [DataMember]
        public decimal ReceiveMoney
        {
            get { return _receiveMoney; }
            set { 
                SetProperty(ref _receiveMoney, value);
                ReturnMoney = ReceiveMoney - Amount;
            }
        }

        private decimal _returnMoney;
        [DataMember]
        public decimal ReturnMoney
        {
            get { return _returnMoney < 0 ? 0 : _returnMoney; }
            set { SetProperty(ref _returnMoney, value); }
        }

        private string _createdUser;
        [DataMember]
        public string CreatedUser
        {
            get { return _createdUser; }
            set { SetProperty(ref _createdUser, value); }
        }

        [DataMember]
        public DateTime RequestDataDate { get; set; }

        private ZitObservableCollection<POSSaleOrderDetailModel> _details;
        [DataMember]
        public ZitObservableCollection<POSSaleOrderDetailModel> Detail
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
                    SubTotal = _details.Where(m => (IsReturn && m.IsReturn) || (!IsReturn)).Sum(m => m.LineAmount);
                    Qty = _details.Where(m => (IsReturn && m.IsReturn) || (!IsReturn)).Sum(m => m.Qty);
                }
            }
        }

        void Detail_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SubTotal = Detail.Where(m => (IsReturn && m.IsReturn) || (!IsReturn)).Sum(m => m.LineAmount);
            Qty = Detail.Where(m => (IsReturn && m.IsReturn) || (!IsReturn)).Sum(m => m.Qty);
        }
        
    }
}
