using System.Web.Mvc;
using Nop.Web.Framework.Security;
using DevTrends.MvcDonutCaching;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BaseNopController
    {

        [NopHttpsRequirement(SslRequirement.No)]
        [DonutOutputCache(CacheProfile = "Home")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
