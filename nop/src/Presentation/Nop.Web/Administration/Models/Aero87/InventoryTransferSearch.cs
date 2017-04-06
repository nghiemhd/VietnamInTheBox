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
    public class InventoryTransferSearch : BaseNopEntityModel
    {
        [Display(Name = "Từ ngày")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Tình trạng")]
        public ITStatus? Status { get; set; }

        [Display(Name = "Mã SP")]
        public string ProductCode { get; set; }

        [Display(Name = "Số CT")]
        public string TranNo { get; set; }

        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Từ kho")]
        public int? FromStoreId { get; set; }

        [Display(Name = "Đến kho")]
        public int? ToStoreId { get; set; }

        public IList<SelectListItem> Stores { get; set; }
    }
}