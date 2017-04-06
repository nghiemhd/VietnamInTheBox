using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Plugin.ThemeHelper.Clothes.Infrastructure.Cache;
using Nop.Plugin.ThemeHelper.Clothes.Models;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Forums;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.UI;

namespace Nop.Plugin.ThemeHelper.Clothes.Controllers
{
    public class ThemeHelperClothesController : Controller
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICheckoutAttributeService _checkoutAttributeService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IForumService _forumService;
        private readonly IStoreService _storeService;

        private readonly MediaSettings _mediaSettings;
        private readonly ForumSettings _forumSettings;
        private readonly OrderSettings _orderSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly ClothesSettings _clothesSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly CommonSettings _commonSettings;
        private readonly BlogSettings _blogSettings;
        private readonly NewsSettings _newsSettings;

        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public ThemeHelperClothesController(IWorkContext workContext, 
            ISettingService settingService,
            IStoreService storeService, 
            ClothesSettings clothesSettings, 
            ForumSettings forumSettings, 
            OrderSettings orderSettings, 
            ICheckoutAttributeService checkoutAttributeService, 
            IGenericAttributeService genericAttributeService, 
            IOrderProcessingService orderProcessingService, 
            ILocalizationService localizationService, 
            IPermissionService permissionService, 
            IForumService forumService, 
            IStoreContext storeContext, 
            CustomerSettings customerSettings, 
            ICacheManager cacheManager, 
            ICategoryService categoryService, 
            IManufacturerService manufacturerService, 
            IPictureService pictureService, 
            MediaSettings mediaSettings, 
            CatalogSettings catalogSettings, 
            CommonSettings commonSettings, 
            BlogSettings blogSettings, 
            NewsSettings newsSettings, 
            IPluginFinder pluginFinder, 
            ILogger logger)
        {
            this._workContext = workContext;
            this._settingService = settingService;
            this._storeService = storeService;
            this._clothesSettings = clothesSettings;
            this._forumSettings = forumSettings;
            this._orderSettings = orderSettings;
            this._checkoutAttributeService = checkoutAttributeService;
            this._genericAttributeService = genericAttributeService;
            this._orderProcessingService = orderProcessingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._forumService = forumService;
            this._storeContext = storeContext;
            this._customerSettings = customerSettings;
            this._cacheManager = cacheManager;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._pictureService = pictureService;
            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._commonSettings = commonSettings;
            this._blogSettings = blogSettings;
            this._newsSettings = newsSettings;
            this._pluginFinder = pluginFinder;
            this._logger = logger;
        }

        #endregion

        #region Utilities

        #region Notification code copied from Nop.Admin.Controllers.BaseNopController

        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("nop.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        #endregion

        [NonAction]
        protected int GetUnreadPrivateMessages()
        {
            var result = 0;
            var customer = _workContext.CurrentCustomer;
            if (_forumSettings.AllowPrivateMessages && !customer.IsGuest())
            {
                var privateMessages = _forumService.GetAllPrivateMessages(_storeContext.CurrentStore.Id,
                    0, customer.Id, false, null, false, string.Empty, 0, 1);

                if (privateMessages.TotalCount > 0)
                {
                    result = privateMessages.TotalCount;
                }
            }

            return result;
        }

        [NonAction]
        protected IList<CategoryTopMenuModel.CategoryModel> PrepareCategoryNavigationModel(int rootCategoryNumber, int subCategoryLevels, int rootCategoryId, int level)
        {
            var result = new List<CategoryTopMenuModel.CategoryModel>();
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(rootCategoryId);
            if (rootCategoryId == 0 && _clothesSettings.RootCategoryNumber > 0)
            {
                //limit root categories
                categories = categories.Take(_clothesSettings.RootCategoryNumber).ToList();
            }

            foreach (var category in categories)
            {
                var categoryModel = new CategoryTopMenuModel.CategoryModel()
                {
                    Id = category.Id,
                    Name = category.GetLocalized(x => x.Name),
                    SeName = category.GetSeName(),
                    IsHotCat = category.IsHotCat,
                    IsHighlighted = category.IsHighlighted,
                    IncludeInTopMenu = category.IncludeInTopMenu
                };

                //subcategories
                if (level < _clothesSettings.SubCategoryLevels)
                {
                    categoryModel.SubCategories.AddRange(PrepareCategoryNavigationModel(rootCategoryNumber, subCategoryLevels, category.Id, level + 1));
                }

                result.Add(categoryModel);
            }

            return result;
        }

        [NonAction]
        protected CategoryTopMenuModel.ManufacturerModel PrepareManufacturerNavigationModel()
        {
            var result = new CategoryTopMenuModel.ManufacturerModel();
            result.Name = _localizationService.GetResource("manufacturers");

            var manufacturers = _manufacturerService.GetAllManufacturers();

            foreach (var manufacturer in manufacturers)
            {
                var manufacturerModel = new CategoryTopMenuModel.ManufacturerModel()
                {
                    Id = manufacturer.Id,
                    Name = manufacturer.Name,
                    SeName = manufacturer.GetSeName()
                };

                result.Manufacturers.Add(manufacturerModel);
            }

            return result;
        }

        #endregion

        #region Methods

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var clothesSettings = _settingService.LoadSetting<ClothesSettings>(storeScope);

            var model = new ConfigurationModel()
                            {
                                ActiveStoreScopeConfiguration = storeScope,
                                DataAlreadyInstalled = clothesSettings.DataAlreadyInstalled,

                                RootCategoryNumber = clothesSettings.RootCategoryNumber,
                                SubCategoryLevels = clothesSettings.SubCategoryLevels,

                                PhoneNumber = clothesSettings.PhoneNumber,
                                FacebookLink = clothesSettings.FacebookLink,
                                TwitterLink = clothesSettings.TwitterLink,
                                YouTubeLink = clothesSettings.YouTubeLink,
                                SendoLink = clothesSettings.SendoLink,

                                Slide1Html = clothesSettings.Slide1Html,
                                Slide2Html = clothesSettings.Slide2Html,
                                Slide3Html = clothesSettings.Slide3Html,
                            };

            if (storeScope > 0)
            {
                model.RootCategoryNumber_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.RootCategoryNumber, storeScope);
                model.SubCategoryLevels_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.SubCategoryLevels, storeScope);
                
                model.PhoneNumber_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.PhoneNumber, storeScope);
                model.FacebookLink_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.FacebookLink, storeScope);
                model.TwitterLink_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.TwitterLink, storeScope);
                model.YouTubeLink_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.YouTubeLink, storeScope);
                model.SendoLink_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.YouTubeLink, storeScope);

                model.Slide1Html_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.Slide1Html, storeScope);
                model.Slide2Html_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.Slide2Html, storeScope);
                model.Slide3Html_OverrideForStore = _settingService.SettingExists(clothesSettings, x => x.Slide3Html, storeScope);
            }

            return View("Nop.Plugin.ThemeHelper.Clothes.Views.ThemeHelperClothes.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        [FormValueRequired("saveconfigure")]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var clothesSettings = _settingService.LoadSetting<ClothesSettings>(storeScope);

            clothesSettings.RootCategoryNumber = model.RootCategoryNumber;
            clothesSettings.SubCategoryLevels = model.SubCategoryLevels;

            clothesSettings.PhoneNumber = model.PhoneNumber;
            clothesSettings.FacebookLink = model.FacebookLink;
            clothesSettings.TwitterLink = model.TwitterLink;
            clothesSettings.YouTubeLink = model.YouTubeLink;
            clothesSettings.SendoLink = model.SendoLink;

            clothesSettings.Slide1Html = model.Slide1Html;
            clothesSettings.Slide2Html = model.Slide2Html;
            clothesSettings.Slide3Html = model.Slide3Html;
            
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            if (model.RootCategoryNumber_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.RootCategoryNumber, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.RootCategoryNumber, storeScope);

            if (model.SubCategoryLevels_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.SubCategoryLevels, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.SubCategoryLevels, storeScope);


            if (model.PhoneNumber_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.PhoneNumber, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.PhoneNumber, storeScope);

            if (model.TwitterLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.TwitterLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.TwitterLink, storeScope);

            if (model.FacebookLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.FacebookLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.FacebookLink, storeScope);

            if (model.YouTubeLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.YouTubeLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.YouTubeLink, storeScope);

            if (model.SendoLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.SendoLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.SendoLink, storeScope);


            if (model.Slide1Html_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.Slide1Html, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.Slide1Html, storeScope);

            if (model.Slide2Html_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.Slide2Html, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.Slide2Html, storeScope);

            if (model.Slide3Html_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(clothesSettings, x => x.Slide3Html, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(clothesSettings, x => x.Slide3Html, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        //preconfigure
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("preconfigure")]
        public ActionResult Preconfigure()
        {
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("ThemeHelper.Clothes");
            if (pluginDescriptor == null)
                throw new Exception("Cannot load the plugin");

            //plugin
            var plugin = pluginDescriptor.Instance() as ClothesPlugin;
            if (plugin == null)
                throw new Exception("Cannot load the plugin");

            //preconfigure
            try
            {
                plugin.Preconfigure();

                var message = _localizationService.GetResource("Clothes.PreconfigureCompleted");
                AddNotification(NotifyType.Success, message, true);
                _logger.Information(message);
            }
            catch (Exception exception)
            {
                var message = _localizationService.GetResource("Clothes.PreconfigureError") + exception;
                AddNotification(NotifyType.Error, message, true);
                _logger.Error(message);
            }

            return Configure();
        }

        //install sample data
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("installsampledata")]
        public ActionResult InstallSampleData()
        {
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("ThemeHelper.Clothes");
            if (pluginDescriptor == null)
                throw new Exception("Cannot load the plugin");

            //plugin
            var plugin = pluginDescriptor.Instance() as ClothesPlugin;
            if (plugin == null)
                throw new Exception("Cannot load the plugin");

            //install data
            try
            {
                plugin.InstallSampleData();

                var clothesSettings = _settingService.LoadSetting<ClothesSettings>();
                clothesSettings.DataAlreadyInstalled = true;
                _settingService.SaveSetting(clothesSettings, x => x.DataAlreadyInstalled);

                var message = _localizationService.GetResource("Clothes.InstallationCompleted");
                AddNotification(NotifyType.Success, message, true);
                _logger.Information(message);
            }
            catch (Exception exception)
            {
                var message = _localizationService.GetResource("Clothes.InstallationError") + exception;
                AddNotification(NotifyType.Error, message, true);
                _logger.Error(message);
            }

            return Configure();
        }

        //call block
        [ChildActionOnly]
        public ActionResult Call()
        {
            var model = new CallModel()
            {
                PhoneNumber = _clothesSettings.PhoneNumber
            };
            return PartialView("../../Themes/Clothes/Views/Common/Call", model);
        }

        //header links
        //[ChildActionOnly]
        public ActionResult HeaderLinks()
        {
            var customer = _workContext.CurrentCustomer;

            var unreadMessageCount = GetUnreadPrivateMessages();
            var unreadMessage = string.Empty;
            var alertMessage = string.Empty;
            if (unreadMessageCount > 0)
            {
                unreadMessage = string.Format(_localizationService.GetResource("PrivateMessages.TotalUnread"), unreadMessageCount);

                //notifications here
                if (_forumSettings.ShowAlertForPM &&
                    !customer.GetAttribute<bool>(SystemCustomerAttributeNames.NotifiedAboutNewPrivateMessages, _storeContext.CurrentStore.Id))
                {
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.NotifiedAboutNewPrivateMessages, true, _storeContext.CurrentStore.Id);
                    alertMessage = string.Format(_localizationService.GetResource("PrivateMessages.YouHaveUnreadPM"), unreadMessageCount);
                }
            }

            var cart = _workContext.CurrentCustomer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .Where(sci => sci.StoreId == _storeContext.CurrentStore.Id)
                .ToList();
            //a customer should visit the shopping cart page before going to checkout if:
            //1. "terms of services" are enabled
            //2. we have at least one checkout attribute
            //3. min order sub-total is OK
            var checkoutAttributes = _checkoutAttributeService.GetAllCheckoutAttributes();
            if (!cart.RequiresShipping())
            {
                //remove attributes which require shippable products
                checkoutAttributes = checkoutAttributes.RemoveShippableAttributes();
            }
            bool minOrderSubtotalAmountOk = _orderProcessingService.ValidateMinOrderSubtotalAmount(cart);

            var model = new HeaderLinksModel()
            {
                IsAuthenticated = customer.IsRegistered(),
                CustomerEmailUsername = customer.IsRegistered() ? (_customerSettings.UsernamesEnabled ? customer.Username : customer.Email) : "",
                ShoppingCartEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
                ShoppingCartItems = customer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .Where(sci => sci.StoreId == _storeContext.CurrentStore.Id)
                    .ToList()
                    .GetTotalProducts(),
                WishlistEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableWishlist),
                WishlistItems = customer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.Wishlist)
                    .Where(sci => sci.StoreId == _storeContext.CurrentStore.Id)
                    .ToList()
                    .GetTotalProducts(),
                AllowPrivateMessages = customer.IsRegistered() && _forumSettings.AllowPrivateMessages,
                UnreadPrivateMessages = unreadMessage,
                DisplayCheckoutButton = 
                    checkoutAttributes.Count == 0 &&
                    minOrderSubtotalAmountOk,
                AlertMessage = alertMessage,
            };

            return PartialView("../../Themes/Clothes/Views/Common/HeaderLinks", model);
        }

        //shopping cart box
        [ChildActionOnly]
        public ActionResult ShoppingCartBox()
        {
            var customer = _workContext.CurrentCustomer;
            var model = new ShoppingCartBoxModel()
            {
                ItemsCount = customer.ShoppingCartItems.Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                                                       .Where(sci => sci.StoreId == _storeContext.CurrentStore.Id)
                                                       .ToList().GetTotalProducts(),
                ShoppingCartEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
            };
            return View("../../Themes/Clothes/Views/Common/ShoppingCartBox", model);
        }

        // category top menu
        public ActionResult CategoryTopMenu()
        {
            var rootCategoryNumber = _clothesSettings.RootCategoryNumber;
            var subCategoryLevels = _clothesSettings.SubCategoryLevels;

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ClothesThemeModelCacheEventConsumer.CATEGORY_HOMEPAGE_NAVIGATION_MODEL_KEY, _workContext.WorkingLanguage.Id,
                string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                return new CategoryTopMenuModel()
                {
                    Categories = PrepareCategoryNavigationModel(rootCategoryNumber, subCategoryLevels, 0, 0).ToList(),
                    Manufacturer = PrepareManufacturerNavigationModel()
                };
            }
            );

            return PartialView("../../Themes/Clothes/Views/Common/CategoryTopMenu", cachedModel);
        }

        //slider
        [ChildActionOnly]
        public ActionResult Slider()
        {
            var model = new SliderModel()
            {
                Slide1Html = _clothesSettings.Slide1Html,
                Slide2Html = _clothesSettings.Slide2Html,
                Slide3Html = _clothesSettings.Slide3Html,
            };
            return View("../../Themes/Clothes/Views/Home/Slider", model);
        }

        //manufacturers
        [ChildActionOnly]
        public ActionResult HomePageManufacturers()
        {
            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ClothesThemeModelCacheEventConsumer.MANUFACTURER_HOMEPAGE_NAVIGATION_MODEL_KEY, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var manufacturers = _manufacturerService.GetAllManufacturers();
                var model = new List<HomePageManufacturerModel>();
                foreach (var manufacturer in manufacturers.Take(_catalogSettings.ManufacturersBlockItemsToDisplay))
                {
                    var modelMan = new HomePageManufacturerModel()
                    {
                        Id = manufacturer.Id,
                        Name = manufacturer.GetLocalized(x => x.Name),
                        SeName = manufacturer.GetSeName(),
                    };

                    if (manufacturer.PictureId != 0)
                    {
                        modelMan.PictureUrl = _pictureService.GetPictureUrl(manufacturer.PictureId,
                                                                            _mediaSettings.ManufacturerThumbPictureSize);
                        if(!string.IsNullOrEmpty(modelMan.PictureUrl))
                        {
                            model.Add(modelMan);
                        }
                    }
                }
                return model;
            });

            return PartialView("../../Themes/Clothes/Views/Catalog/HomePageManufacturers", cacheModel);
        }

        //footer
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterModel()
            {
                StoreName = _storeContext.CurrentStore.Name,
                WishlistEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableWishlist),
                ShoppingCartEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
                SitemapEnabled = _commonSettings.SitemapEnabled,
                WorkingLanguageId = _workContext.WorkingLanguage.Id,
                FacebookLink = _clothesSettings.FacebookLink,
                TwitterLink = _clothesSettings.TwitterLink,
                YouTubeLink = _clothesSettings.YouTubeLink,
                SendoLink = _clothesSettings.SendoLink,
                BlogEnabled = _blogSettings.Enabled,
                CompareProductsEnabled = _catalogSettings.CompareProductsEnabled,
                ForumEnabled = _forumSettings.ForumsEnabled,
                NewsEnabled = _newsSettings.Enabled,
                RecentlyViewedProductsEnabled = _catalogSettings.RecentlyViewedProductsEnabled,
                RecentlyAddedProductsEnabled = _catalogSettings.RecentlyAddedProductsEnabled
            };
            return PartialView("../../Themes/Clothes/Views/Common/Footer", model);
        }

        #endregion
    }
}