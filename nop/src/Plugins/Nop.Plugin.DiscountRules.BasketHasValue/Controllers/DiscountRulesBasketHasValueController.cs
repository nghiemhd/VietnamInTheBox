using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.DiscountRules.BasketHasValue.Models;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.DiscountRules.BasketHasValue.Controllers
{
    [AdminAuthorize]
    public class DiscountRulesBasketHasValueController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ISettingService _settingService;

        public DiscountRulesBasketHasValueController(IDiscountService discountService, ISettingService settingService)
        {
            this._discountService = discountService;
            this._settingService = settingService;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            base.Initialize(requestContext);
        }

        public ActionResult Configure(int discountId, int? discountRequirementId)
        {
            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            DiscountRequirement discountRequirement = null;
            if (discountRequirementId.HasValue)
            {
                discountRequirement = discount.DiscountRequirements.Where(dr => dr.Id == discountRequirementId.Value).FirstOrDefault();
                if (discountRequirement == null)
                    return Content("Failed to load requirement.");
            }

            var basketAmountRequirement = _settingService.GetSettingByKey<decimal>(string.Format("DiscountRequirement.BasketHasValue_BasketAmount-{0}", discountRequirementId.HasValue ? discountRequirementId.Value : 0));
            var useBasketAmountRequirement = _settingService.GetSettingByKey<bool>(string.Format("DiscountRequirement.BasketHasValue_UseBasketAmount-{0}", discountRequirementId.HasValue ? discountRequirementId.Value : 0));

            var productCountRequirement = _settingService.GetSettingByKey<int>(string.Format("DiscountRequirement.BasketHasValue_ProductCount-{0}", discountRequirementId.HasValue ? discountRequirementId.Value : 0));
            var useProductCountRequirement = _settingService.GetSettingByKey<bool>(string.Format("DiscountRequirement.BasketHasValue_UseProductCount-{0}", discountRequirementId.HasValue ? discountRequirementId.Value : 0));

            var model = new RequirementModel();
            model.RequirementId = discountRequirementId.HasValue ? discountRequirementId.Value : 0;
            model.DiscountId = discountId;
            model.BasketAmount = basketAmountRequirement;
            model.UseBasketAmount = useBasketAmountRequirement;
            model.UseProductCount = useProductCountRequirement;
            model.ProductCount = productCountRequirement;

            //add a prefix
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("DiscountRulesBasketHasValue{0}", discountRequirementId.HasValue ? discountRequirementId.Value.ToString() : "0");

            return View("Nop.Plugin.DiscountRules.BasketHasValue.Views.DiscountRulesBasketHasValue.Configure", model);
        }

        [HttpPost]
        public ActionResult Configure(int discountId, int? discountRequirementId, decimal basketAmount, bool useBasketAmount, int productCount, bool useProductCount)
        {
            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            DiscountRequirement discountRequirement = null;
            if (discountRequirementId.HasValue)
                discountRequirement = discount.DiscountRequirements.Where(dr => dr.Id == discountRequirementId.Value).FirstOrDefault();

            if (discountRequirement != null)
            {
                //update existing rule
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_BasketAmount-{0}", discountRequirement.Id), basketAmount);
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_UseBasketAmount-{0}", discountRequirement.Id), useBasketAmount);
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_ProductCount-{0}", discountRequirement.Id), productCount);
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_UseProductCount-{0}", discountRequirement.Id), useProductCount);
            }
            else
            {
                //save new rule
                discountRequirement = new DiscountRequirement()
                {
                    DiscountRequirementRuleSystemName = "DiscountRequirement.BasketHasValue"
                };
                discount.DiscountRequirements.Add(discountRequirement);
                _discountService.UpdateDiscount(discount);

                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_BasketAmount-{0}", discountRequirement.Id), basketAmount);
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_UseBasketAmount-{0}", discountRequirement.Id), useBasketAmount);
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_ProductCount-{0}", discountRequirement.Id), productCount);
                _settingService.SetSetting(string.Format("DiscountRequirement.BasketHasValue_UseProductCount-{0}", discountRequirement.Id), useProductCount);
            }
            return Json(new { Result = true, NewRequirementId = discountRequirement.Id }, JsonRequestBehavior.AllowGet);
        }
        
    }
}