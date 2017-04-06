using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;

namespace Nop.Plugin.DiscountRules.BasketHasValue
{
    public partial class BasketHasValueDiscountRequirementRule : BasePlugin, IDiscountRequirementRule
    {
        private readonly ISettingService _settingService;

        public BasketHasValueDiscountRequirementRule(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        /// <summary>
        /// Check discount requirement
        /// </summary>
        /// <param name="request">Object that contains all information required to check the requirement (Current customer, discount, etc)</param>
        /// <returns>true - requirement is met; otherwise, false</returns>
        public bool CheckRequirement(CheckDiscountRequirementRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.DiscountRequirement == null)
                throw new NopException("Discount requirement is not set");

            var basketAmountRequirement = _settingService.GetSettingByKey<decimal>(string.Format("DiscountRequirement.BasketHasValue-{0}", request.DiscountRequirement.Id));

            var useBasketAmountRequirement = _settingService.GetSettingByKey<bool>(string.Format("DiscountRequirement.BasketHasValue_UseBasketAmount-{0}", request.DiscountRequirement.Id));

            var productCountRequirement = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.BasketHasValue_ProductCount-{0}", request.DiscountRequirement.Id));
            var useProductCountRequirement = _settingService.GetSettingByKey<bool>(string.Format("DiscountRequirement.BasketHasValue_UseProductCount-{0}", request.DiscountRequirement.Id));

            if (useBasketAmountRequirement && basketAmountRequirement == decimal.Zero)
                return true;

            if (useProductCountRequirement && productCountRequirement == decimal.Zero)
                return true;

            if (useBasketAmountRequirement)
            {
                var cartItems = request.Customer.ShoppingCartItems;
                decimal basketAmount = cartItems.Sum(i => (i.Quantity * i.Product.Price));
                return basketAmount >= basketAmountRequirement;
            }
            else if(useProductCountRequirement)
            {
                var cartItems = request.Customer.ShoppingCartItems;
                int basketQty = cartItems.Sum(i => (i.Quantity));
                return basketQty >= productCountRequirement;
            }
            //if (request.Customer == null || request.Customer.IsGuest())
                //return false;
            return false;
        }

        /// <summary>
        /// Get URL for rule configuration
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="discountRequirementId">Discount requirement identifier (if editing)</param>
        /// <returns>URL</returns>
        public string GetConfigurationUrl(int discountId, int? discountRequirementId)
        {
            //configured in RouteProvider.cs
            string result = "Plugins/DiscountRulesBasketHasValue/Configure/?discountId=" + discountId;
            if (discountRequirementId.HasValue)
                result += string.Format("&discountRequirementId={0}", discountRequirementId.Value);
            return result;
        }

        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.BasketHasValue.Fields.Amount", "Required basket value");
            this.AddOrUpdatePluginLocaleResource("Plugins.DiscountRules.BasketHasValue.Fields.Amount.Hint", "Discount will be applied if customer has x.xx value in their basket.");
            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.DiscountRules.BasketHasValue.Fields.Amount");
            this.DeletePluginLocaleResource("Plugins.DiscountRules.BasketHasValue.Fields.Amount.Hint");
            base.Uninstall();
        }
    }
}