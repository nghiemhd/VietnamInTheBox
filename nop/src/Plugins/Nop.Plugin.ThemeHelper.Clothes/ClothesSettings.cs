using Nop.Core.Configuration;

namespace Nop.Plugin.ThemeHelper.Clothes
{
    public class ClothesSettings : ISettings
    {
        public bool DataAlreadyInstalled { get; set; }
        public int SubCategoryLevels { get; set; }
        public int RootCategoryNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string TwitterLink { get; set; }
        public string FacebookLink { get; set; }
        public string YouTubeLink { get; set; }
        public string SendoLink { get; set; }

        public string Slide1Html { get; set; }
        public string Slide2Html { get; set; }
        public string Slide3Html { get; set; }
    }
}