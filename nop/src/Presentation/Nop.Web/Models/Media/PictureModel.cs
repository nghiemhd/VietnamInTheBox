using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Media
{
    public partial class PictureModel : BaseNopModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }

        public bool IsDiscount { get; set; }

        public string DiscountPercent { get; set; }

        public bool IsOutOfStock { get; set; }
    }
}