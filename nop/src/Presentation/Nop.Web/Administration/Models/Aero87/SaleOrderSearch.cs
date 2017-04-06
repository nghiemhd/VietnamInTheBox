using Nop.Core.Domain.Aero87;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Admin.Models.Aero87
{
    public class SaleOrderSearch : BaseNopEntityModel
    {
        [Display(Name="Từ Ngày")]
        public DateTime? FromDate { get; set; }
        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Tình trạng")]
        public PSStatus? PSStatus { get; set; }

        [Display(Name = "Diễn giải")]
        public string Desc { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }
        [Display(Name = "Mã SP")]
        public string ProductCode { get; set; }
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }
        [Display(Name = "Mã đơn hàng")]
        public string OrderNo { get; set; }
        [Display(Name = "Mã hóa đơn gốc")]
        public string RefNo { get; set; }

        [Display(Name = "Cửa hàng")]
        public int? StoreId { get; set; }

        [Display(Name = "Nhà vận chuyển")]
        public int? CarrierId { get; set; }

        [Display(Name = "Kênh")]
        public SaleChanel? SaleChanel { get; set; }

        [Display(Name = "Nguồn")]
        public int? SourceId { get; set; }

        [Display(Name = "Lý do đổi/trả")]
        public int? ReturnReasonId { get; set; }

        [Display(Name = "Mã vận đơn")]
        public string ShippingCode { get; set; }

        [Display(Name = "Loại thanh toán")]
        public PaymentType? PaymentType { get; set; }

        [Display(Name = "Thanh toán")]
        public PaymentStatus? PaymentStatus { get; set; }

        public IList<SelectListItem> Stores { get; set; }

        public IList<SelectListItem> Carriers { get; set; }

        public IList<SelectListItem> SalesSources { get; set; }

        public IList<SelectListItem> ReturnReasons { get; set; }
    }
}