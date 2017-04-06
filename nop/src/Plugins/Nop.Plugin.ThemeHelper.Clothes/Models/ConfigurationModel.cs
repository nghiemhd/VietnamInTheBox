using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ThemeHelper.Clothes.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        public bool DataAlreadyInstalled { get; set; }

        [NopResourceDisplayName("Clothes.SubCategoryLevels")]
        public int SubCategoryLevels { get; set; }
        public bool SubCategoryLevels_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.RootCategoryNumber")]
        public int RootCategoryNumber { get; set; }
        public bool RootCategoryNumber_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.PhoneNumber")]
        public string PhoneNumber { get; set; }
        public bool PhoneNumber_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.TwitterLink")]
        public string TwitterLink { get; set; }
        public bool TwitterLink_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.FacebookLink")]
        public string FacebookLink { get; set; }
        public bool FacebookLink_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.YouTubeLink")]
        public string YouTubeLink { get; set; }
        public bool YouTubeLink_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.SendoLink")]
        public string SendoLink { get; set; }
        public bool SendoLink_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.Slide1Html")]
        [AllowHtml]
        public string Slide1Html { get; set; }
        public bool Slide1Html_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.Slide2Html")]
        [AllowHtml]
        public string Slide2Html { get; set; }
        public bool Slide2Html_OverrideForStore { get; set; }

        [NopResourceDisplayName("Clothes.Slide3Html")]
        [AllowHtml]
        public string Slide3Html { get; set; }
        public bool Slide3Html_OverrideForStore { get; set; }
    }
}