using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ThemeHelper.Clothes.Models
{
    public partial class HomePageManufacturerModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }

        public string PictureUrl { get; set; }
    }
}