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
    public class InventoryTransactionSearch : BaseNopEntityModel
    {
        [Display(Name = "Từ ngày")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Mã SP")]
        public string ProductCode { get; set; }

        [Display(Name = "Số CT")]
        public string TranNo { get; set; }

        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Cửa hàng")]
        public int? StoreId { get; set; }

        [Display(Name = "Loại CT")]
        public DocType? TypeId { get; set; }

        public IList<SelectListItem> Stores { get; set; }
    }
}