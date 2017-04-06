using Nop.Web.Framework;
using System.ComponentModel;

namespace Nop.Plugin.DiscountRules.BasketHasValue.Models
{
    public class RequirementModel
    {
        [NopResourceDisplayName("Plugins.DiscountRules.BasketHasValue.Fields.Amount")]
        public decimal BasketAmount { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }

        [DisplayName("Use Amount")]
        public bool UseBasketAmount { get; set; }

        [DisplayName("Use Product Count")]
        public bool UseProductCount { get; set; }

        [DisplayName("Product Count")]
        public int ProductCount { get; set; }
    }
}