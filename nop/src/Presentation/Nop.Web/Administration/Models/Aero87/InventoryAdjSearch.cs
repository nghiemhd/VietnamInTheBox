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
    public class InventoryAdjSearch : BaseNopEntityModel
    {
        [Display(Name = "Từ Ngày")]
        public DateTime? FromDate { get; set; }
        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }
        [Display(Name = "Tình trạng")]
        public IAStatus? IAStatus { get; set; }
        [Display(Name = "Mã CT")]
        public string AdjNo { get; set; }
        [Display(Name = "Mã SP")]
        public string ProductCode { get; set; }
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Kho")]
        public int? StoreId { get; set; }

        public IList<SelectListItem> Stores { get; set; }
    }
}