using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Admin.Models.Aero87
{
    public class WarehouseCountSearch : BaseNopEntityModel
    {
        [Display(Name = "Đến hết ngày")]
        public DateTime ToEndDate { get; set; }

        [Display(Name = "Mã SP")]
        public string ProductCode { get; set; }

        [Display(Name = "Theo lượng")]
        public QtyFilter? QtyFilter { get; set; }

        [Display(Name = "Sort Theo tuổi")]
        public bool OrderByAge { get; set; }

        [Display(Name = "Theo thương hiệu")]
        public int SearchManufacturerId { get; set; }

        [Display(Name = "Theo ngành hàng")]
        public int SearchCategoryId { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

        [Display(Name = "Cửa hàng")]
        public int? StoreId { get; set; }

        public IList<SelectListItem> Stores { get; set; }
    }

    public enum QtyFilter : int
    {
        [Display(Name = "Hết hàng")]
        OutStock = 0,
        [Display(Name="Tồn dương")]
        Stock = 1,
        [Display(Name = "Tồn âm")]
        Negative = -1
    }
}