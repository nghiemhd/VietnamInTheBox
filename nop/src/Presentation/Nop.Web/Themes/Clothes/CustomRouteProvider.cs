using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework.Localization;
using System.Web.Mvc;

namespace Nop.Web.Themes.Clothes
{
    public partial class RouteProvider : IRouteProvider
    {

        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.MapLocalizedRoute("lien_he",
                            "lien_he",
                            new { controller = "Common", action = "ContactUs" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("so_do_website",
                            "so_do_website",
                            new { controller = "Common", action = "Sitemap" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("tim_kiem_san_pham",
                            "tim_kiem_san_pham/",
                            new { controller = "Catalog", action = "Search" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("san_pham_vua_xem",
                            "san_pham_vua_xem/",
                            new { controller = "Catalog", action = "RecentlyViewedProducts" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("danh_sach_so_sanh",
                            "danh_sach_so_sanh/",
                            new { controller = "Catalog", action = "CompareProducts" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("san_pham_moi",
                            "san_pham_moi/",
                            new { controller = "Catalog", action = "RecentlyAddedProducts" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("danh_sach_don_hang",
                            "danh_sach_don_hang",
                            new { controller = "Customer", action = "Orders" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("danh_sach_dia_chi",
                            "danh_sach_dia_chi",
                            new { controller = "Customer", action = "Addresses" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("thong_tin_tai_khoan",
                            "thong_tin_tai_khoan",
                            new { controller = "Customer", action = "Info" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("gio_hang",
                            "gio_hang/",
                            new { controller = "ShoppingCart", action = "Cart" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("danh_sach_mong_muon",
                            "danh_sach_mong_muon/{customerGuid}",
                            new { controller = "ShoppingCart", action = "Wishlist", customerGuid = UrlParameter.Optional },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("thoat",
                            "thoat/",
                            new { controller = "Customer", action = "Logout" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("dang_ky",
                            "dang_ky/",
                            new { controller = "Customer", action = "Register" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("dang_nhap",
                            "dang_nhap/",
                            new { controller = "Customer", action = "Login" },
                            new[] { "Nop.Web.Controllers" });
            routes.MapLocalizedRoute("thanh_toan",
                            "thanh_toan/",
                            new { controller = "Checkout", action = "Index" },
                            new[] { "Nop.Web.Controllers" });

            routes.MapLocalizedRoute("cam_nang_mua_sam",
                            "cam_nang_mua_sam/",
                            new { controller = "Blog", action = "List" },
                            new[] { "Nop.Web.Controllers" });
        }

        public int Priority
        {
            get { return int.MaxValue; }
        }
    }
}