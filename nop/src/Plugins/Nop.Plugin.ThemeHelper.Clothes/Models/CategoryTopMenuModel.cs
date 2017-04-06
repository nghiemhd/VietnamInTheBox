using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ThemeHelper.Clothes.Models
{
    public partial class CategoryTopMenuModel : BaseNopModel
    {
        public CategoryTopMenuModel()
        {
            Categories = new List<CategoryModel>();
            Manufacturer = new ManufacturerModel();
        }

        public List<CategoryModel> Categories { get; set; }
        public ManufacturerModel Manufacturer { get; set; }

        public class CategoryModel : BaseNopEntityModel
        {
            public CategoryModel()
            {
                SubCategories = new List<CategoryModel>();
            }

            public string Name { get; set; }

            public string SeName { get; set; }

            public bool IsHotCat { get; set; }

            public bool IsHighlighted { get; set; }

            public bool IncludeInTopMenu { get; set; }

            public List<CategoryModel> SubCategories { get; set; }
        }

        public class ManufacturerModel : BaseNopEntityModel
        {
            public ManufacturerModel()
            {
                Manufacturers = new List<ManufacturerModel>();
            }

            public string Name { get; set; }

            public string SeName { get; set; }

            public List<ManufacturerModel> Manufacturers { get; set; }
        }
    }
}