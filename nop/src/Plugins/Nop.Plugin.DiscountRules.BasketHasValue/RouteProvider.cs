using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.DiscountRules.BasketHasValue
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.DiscountRules.BasketHasValue.Configure",
                 "Plugins/DiscountRulesBasketHasValue/Configure",
                 new { controller = "DiscountRulesBasketHasValue", action = "Configure" },
                 new[] { "Nop.Plugin.DiscountRules.BasketHasValue.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
