using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Seo;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;

namespace Nop.Plugin.ThemeHelper.Clothes
{
    public class ClothesPlugin : BasePlugin, IMiscPlugin
    {
        #region Fields

        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;
        private readonly IRepository<ManufacturerTemplate> _manufacturerTemplateRepository;
        private readonly IRepository<CategoryTemplate> _categoryTemplateRepository;
        private readonly IRepository<ProductAttribute> _productAttributeRepository;
        private readonly IRepository<ProductTemplate> _productTemplateRepository;
        private readonly IRepository<RelatedProduct> _relatedProductRepository;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<ProductTag> _productTagRepository;
        private readonly IRepository<UrlRecord> _urlRecordRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<NewsItem> _newsItemRepository;
        private readonly IRepository<Product> _productRepository;

        private readonly LocalizationSettings _localizationSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly WidgetSettings _widgetSettings;
        private readonly MediaSettings _mediaSettings;

        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWidgetService _widgetService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public ClothesPlugin(
            IRepository<SpecificationAttribute> specificationAttributeRepository,
            IRepository<ManufacturerTemplate> manufacturerTemplateRepository,
            IRepository<CategoryTemplate> categoryTemplateRepository,
            IRepository<ProductAttribute> productAttributeRepository,
            IRepository<ProductTemplate> productTemplateRepository,
            IRepository<RelatedProduct> relatedProductRepository,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<ProductTag> productTagRepository,
            IRepository<UrlRecord> urlRecordRepository,
            IRepository<Category> categoryRepository,
            IRepository<BlogPost> blogPostRepository,
            IRepository<Language> languageRepository,
            IRepository<NewsItem> newsItemRepository,
            IRepository<Product> productRepository,

            ISettingService settingService, 
            IWebHelper webHelper, 
            IPictureService pictureService, 
            
            LocalizationSettings localizationSettings, 
            CatalogSettings catalogSettings, 
            WidgetSettings widgetSettings, 
            MediaSettings mediaSettings, 
            IWidgetService widgetService)
        {
            this._specificationAttributeRepository = specificationAttributeRepository;
            this._manufacturerTemplateRepository = manufacturerTemplateRepository;
            this._categoryTemplateRepository = categoryTemplateRepository;
            this._productAttributeRepository = productAttributeRepository;
            this._productTemplateRepository = productTemplateRepository;
            this._relatedProductRepository = relatedProductRepository;
            this._manufacturerRepository = manufacturerRepository;
            this._productTagRepository = productTagRepository;
            this._urlRecordRepository = urlRecordRepository;
            this._categoryRepository = categoryRepository;
            this._blogPostRepository = blogPostRepository;
            this._languageRepository = languageRepository;
            this._newsItemRepository = newsItemRepository;
            this._productRepository = productRepository;

            this._settingService = settingService;
            this._webHelper = webHelper;
            this._pictureService = pictureService;

            this._localizationSettings = localizationSettings;
            this._catalogSettings = catalogSettings;
            this._widgetSettings = widgetSettings;
            this._mediaSettings = mediaSettings;
            this._widgetService = widgetService;
        }

        #endregion

        #region Methods

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "ThemeHelperClothes";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.ThemeHelper.Clothes.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new ClothesSettings()
                               {
                                   RootCategoryNumber = 5,
                                   SubCategoryLevels = 3,

                                   PhoneNumber = "(123) 456-78901",
                                   FacebookLink = "http://www.facebook.com/nopCommerce",
                                   TwitterLink = "https://twitter.com/nopCommerce",
                                   YouTubeLink = "http://www.youtube.com/user/nopCommerce",

                                   Slide1Html = "<div><img src='/Themes/Clothes/Content/images/slide1.jpg' width='939' height='411' /><a class='slider-button' href='/plenty-by-tracy-reese-sights-of-the-shoreline-white-dress' style=\"bottom: 58px;left: 220px;\">SHOP NOW</a></div>",
                                   Slide2Html = "<div><img src='/Themes/Clothes/Content/images/slide2.jpg' width='939' height='411' /><a class='slider-button' href='/shoes' style=\"bottom: 130px;left: 60px;\">SHOP NOW</a></div>",
                                   Slide3Html = "<div><img src='/Themes/Clothes/Content/images/slide3.jpg' width='939' height='411' /><a class='slider-button' href='/dresses' style=\"bottom: 105px;left: 46px;\">SHOP NOW</a></div>",
                               };
            _settingService.SaveSetting(settings);

            //update locales 
            this.AddOrUpdatePluginLocaleResource("Shoppingcart.Headerquantity", "{0}");
            this.AddOrUpdatePluginLocaleResource("Search.SearchBox.Tooltip", "Search entire store here...");

            //add locales
            this.AddOrUpdatePluginLocaleResource("Clothes.Configuration", "Configuration");
            this.AddOrUpdatePluginLocaleResource("Clothes.PhoneNumber", "Phone");
            this.AddOrUpdatePluginLocaleResource("Clothes.PhoneNumber.Hint", "Specify your phone number. It'll be displayed at the header of the site");
            this.AddOrUpdatePluginLocaleResource("Clothes.Call.Message", "Call");
            this.AddOrUpdatePluginLocaleResource("Clothes.ShoppingCart.Items", "item(s)");
            this.AddOrUpdatePluginLocaleResource("Clothes.SubCategoryLevels", "Number of subcategory levels");
            this.AddOrUpdatePluginLocaleResource("Clothes.SubCategoryLevels.Hint", "Enter the number of subcategory levels to be displayed in the header menu");
            this.AddOrUpdatePluginLocaleResource("Clothes.RootCategoryNumber", "Number of root categories");
            this.AddOrUpdatePluginLocaleResource("Clothes.RootCategoryNumber.Hint", "Enter the number of top level categories to be displayed in the header menu");
            this.AddOrUpdatePluginLocaleResource("Clothes.Slide1Html", "Slide1 content");
            this.AddOrUpdatePluginLocaleResource("Clothes.Slide2Html", "Slide2 content");
            this.AddOrUpdatePluginLocaleResource("Clothes.Slide3Html", "Slide3 content");
            this.AddOrUpdatePluginLocaleResource("Clothes.Slide1Html.Hint", "Enter the content of the first slide. It'll be displayed on the home page.");
            this.AddOrUpdatePluginLocaleResource("Clothes.Slide2Html.Hint", "Enter the content of the second slide. It'll be displayed on the home page.");
            this.AddOrUpdatePluginLocaleResource("Clothes.Slide3Html.Hint", "Enter the content of the third slide. It'll be displayed on the home page.");
            this.AddOrUpdatePluginLocaleResource("Clothes.Sale", "Sale");
            this.AddOrUpdatePluginLocaleResource("Clothes.FacebookLink", "Facebook URL");
            this.AddOrUpdatePluginLocaleResource("Clothes.FacebookLink.Hint", "Specify your Facebook page URL");
            this.AddOrUpdatePluginLocaleResource("Clothes.TwitterLink", "Twitter URL");
            this.AddOrUpdatePluginLocaleResource("Clothes.TwitterLink.Hint", "Specify your twitter page URL");
            this.AddOrUpdatePluginLocaleResource("Clothes.YouTubeLink", "YouTube URL");
            this.AddOrUpdatePluginLocaleResource("Clothes.YouTubeLink.Hint", "Specify your YouTube channel URL");
            this.AddOrUpdatePluginLocaleResource("Clothes.Information", "Information");
            this.AddOrUpdatePluginLocaleResource("Clothes.CustomerService", "Customer service");
            this.AddOrUpdatePluginLocaleResource("Clothes.FollowUs", "Follow us");
            this.AddOrUpdatePluginLocaleResource("Clothes.SelectManufacturer", "Browse by manufacturer");
            this.AddOrUpdatePluginLocaleResource("Clothes.GridView", "Grid view");
            this.AddOrUpdatePluginLocaleResource("Clothes.ListView", "List view");
            this.AddOrUpdatePluginLocaleResource("Clothes.Top", "Top");
            this.AddOrUpdatePluginLocaleResource("Clothes.InstallationCompleted", "Installation completed!");
            this.AddOrUpdatePluginLocaleResource("Clothes.InstallationError", "Installation error: ");
            this.AddOrUpdatePluginLocaleResource("Clothes.SampleData", "Sample data");
            this.AddOrUpdatePluginLocaleResource("Clothes.DataAlreadyInstalled", "The data is already installed!");
            this.AddOrUpdatePluginLocaleResource("Clothes.InstallConfirmation", "Are you sure?");
            this.AddOrUpdatePluginLocaleResource("Clothes.InstallButton", "Install");
            this.AddOrUpdatePluginLocaleResource("Clothes.SampleData.Note", "Install sample data only on the clean installation! Click only once!");
            this.AddOrUpdatePluginLocaleResource("Clothes.Installing", "Wait...");
            this.AddOrUpdatePluginLocaleResource("Clothes.InstallSampleData", "Install sample data");
            this.AddOrUpdatePluginLocaleResource("Clothes.PreconfigureSystem", "Preconfigure system");
            this.AddOrUpdatePluginLocaleResource("Clothes.Preconfigure", "Preconfigure");
            this.AddOrUpdatePluginLocaleResource("Clothes.PreconfigureCompleted", "Preconfigure completed!");
            this.AddOrUpdatePluginLocaleResource("Clothes.PreconfigureError", "Preconfigure error: ");
            this.AddOrUpdatePluginLocaleResource("Clothes.ChangesSaved", "Changes have been saved!");
            this.AddOrUpdatePluginLocaleResource("Clothes.PriceRangeFilter.Under", "Under");
            this.AddOrUpdatePluginLocaleResource("Clothes.PriceRangeFilter.Over", "Over");
            this.AddOrUpdatePluginLocaleResource("Clothes.NoFilter", "No filter");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<ClothesSettings>();

            //update locales 
            this.AddOrUpdatePluginLocaleResource("Shoppingcart.Headerquantity", "({0})");
            this.AddOrUpdatePluginLocaleResource("Search.SearchBox.Tooltip", "Search store");

            //delete locales
            this.DeletePluginLocaleResource("Clothes.Configuration");
            this.DeletePluginLocaleResource("Clothes.PhoneNumber");
            this.DeletePluginLocaleResource("Clothes.PhoneNumber.Hint");
            this.DeletePluginLocaleResource("Clothes.Call.Message");
            this.DeletePluginLocaleResource("Clothes.ShoppingCart.Items");
            this.DeletePluginLocaleResource("Clothes.SubCategoryLevels");
            this.DeletePluginLocaleResource("Clothes.SubCategoryLevels.Hint");
            this.DeletePluginLocaleResource("Clothes.RootCategoryNumber");
            this.DeletePluginLocaleResource("Clothes.RootCategoryNumber.Hint");
            this.DeletePluginLocaleResource("Clothes.Slide1Html");
            this.DeletePluginLocaleResource("Clothes.Slide2Html");
            this.DeletePluginLocaleResource("Clothes.Slide3Html");
            this.DeletePluginLocaleResource("Clothes.Slide1Html.Hint");
            this.DeletePluginLocaleResource("Clothes.Slide2Html.Hint");
            this.DeletePluginLocaleResource("Clothes.Slide3Html.Hint");
            this.DeletePluginLocaleResource("Clothes.Sale");
            this.DeletePluginLocaleResource("Clothes.FacebookLink");
            this.DeletePluginLocaleResource("Clothes.FacebookLink.Hint");
            this.DeletePluginLocaleResource("Clothes.TwitterLink");
            this.DeletePluginLocaleResource("Clothes.TwitterLink.Hint");
            this.DeletePluginLocaleResource("Clothes.YouTubeLink");
            this.DeletePluginLocaleResource("Clothes.YouTubeLink.Hint");
            this.DeletePluginLocaleResource("Clothes.Information");
            this.DeletePluginLocaleResource("Clothes.CustomerService");
            this.DeletePluginLocaleResource("Clothes.FollowUs");
            this.DeletePluginLocaleResource("Clothes.SelectManufacturer");
            this.DeletePluginLocaleResource("Clothes.GridView");
            this.DeletePluginLocaleResource("Clothes.ListView");
            this.DeletePluginLocaleResource("Clothes.Top");
            this.DeletePluginLocaleResource("Clothes.InstallationCompleted");
            this.DeletePluginLocaleResource("Clothes.InstallationError");
            this.DeletePluginLocaleResource("Clothes.SampleData");
            this.DeletePluginLocaleResource("Clothes.DataAlreadyInstalled");
            this.DeletePluginLocaleResource("Clothes.InstallConfirmation");
            this.DeletePluginLocaleResource("Clothes.InstallButton");
            this.DeletePluginLocaleResource("Clothes.SampleData.Note");
            this.DeletePluginLocaleResource("Clothes.Installing");
            this.DeletePluginLocaleResource("Clothes.InstallSampleData");
            this.DeletePluginLocaleResource("Clothes.PreconfigureSystem");
            this.DeletePluginLocaleResource("Clothes.PreConfigure");
            this.DeletePluginLocaleResource("Clothes.PreconfigureCompleted");
            this.DeletePluginLocaleResource("Clothes.PreconfigureError");
            this.DeletePluginLocaleResource("Clothes.ChangesSaved");
            this.DeletePluginLocaleResource("Clothes.PriceRangeFilter.Over");
            this.DeletePluginLocaleResource("Clothes.PriceRangeFilter.Under");
            this.DeletePluginLocaleResource("Clothes.NoFilter");

            base.Uninstall();
        }

        #endregion

        #region Utilities

        private void InstallBlogPosts()
        {
            var defaultLanguage = _languageRepository.Table.FirstOrDefault();
            var blogPosts = new List<BlogPost>
                                {
                                    new BlogPost
                                        {
                                             AllowComments = true,
                                             Language = defaultLanguage,
                                             Title = "Online Discount Coupons",
                                             Body = "<p>Online discount coupons enable access to great offers from some of the world&rsquo;s best sites for Internet shopping. The online coupons are designed to allow compulsive online shoppers to access massive discounts on a variety of products. The regular shopper accesses the coupons in bulk and avails of great festive offers and freebies thrown in from time to time.  The coupon code option is most commonly used when using a shopping cart. The coupon code is entered on the order page just before checking out. Every online shopping resource has a discount coupon submission option to confirm the coupon code. The dedicated web sites allow the shopper to check whether or not a discount is still applicable. If it is, the sites also enable the shopper to calculate the total cost after deducting the coupon amount like in the case of grocery coupons.  Online discount coupons are very convenient to use. They offer great deals and professionally negotiated rates if bought from special online coupon outlets. With a little research and at times, insider knowledge the online discount coupons are a real steal. They are designed to promote products by offering &lsquo;real value for money&rsquo; packages. The coupons are legitimate and help with budgeting, in the case of a compulsive shopper. They are available for special trade show promotions, nightlife, sporting events and dinner shows and just about anything that could be associated with the promotion of a product. The coupons enable the online shopper to optimize net access more effectively. Getting a &lsquo;big deal&rsquo; is not more utopian amidst rising prices. The online coupons offer internet access to the best and cheapest products displayed online. Big discounts are only a code away! By Gaynor Borade (buzzle.com)</p>",
                                             Tags = "e-commerce, money",
                                             CreatedOnUtc = DateTime.UtcNow,
                                        },
                                    new BlogPost
                                        {
                                             AllowComments = true,
                                             Language = defaultLanguage,
                                             Title = "Customer Service - Client Service",
                                             Body = "<p>Managing online business requires different skills and abilities than managing a business in the &lsquo;real world.&rsquo; Customers can easily detect the size and determine the prestige of a business when they have the ability to walk in and take a look around. Not only do &lsquo;real-world&rsquo; furnishings and location tell the customer what level of professionalism to expect, but &quot;real world&quot; personal encounters allow first impressions to be determined by how the business approaches its customer service. When a customer walks into a retail business just about anywhere in the world, that customer expects prompt and personal service, especially with regards to questions that they may have about products they wish to purchase.<br /><br />Customer service or the client service is the service provided to the customer for his satisfaction during and after the purchase. It is necessary to every business organization to understand the customer needs for value added service. So customer data collection is essential. For this, a good customer service is important. The easiest way to lose a client is because of the poor customer service. The importance of customer service changes by product, industry and customer. Client service is an important part of every business organization. Each organization is different in its attitude towards customer service. Customer service requires a superior quality service through a careful design and execution of a series of activities which include people, technology and processes. Good customer service starts with the design and communication between the company and the staff.<br /><br />In some ways, the lack of a physical business location allows the online business some leeway that their &lsquo;real world&rsquo; counterparts do not enjoy. Location is not important, furnishings are not an issue, and most of the visual first impression is made through the professional design of the business website.<br /><br />However, one thing still remains true. Customers will make their first impressions on the customer service they encounter. Unfortunately, in online business there is no opportunity for front- line staff to make a good impression. Every interaction the customer has with the website will be their primary means of making their first impression towards the business and its client service. Good customer service in any online business is a direct result of good website design and planning.</p><p>By Jayashree Pakhare (buzzle.com)</p>",
                                             Tags = "e-commerce, nopCommerce, asp.net, sample tag, money",
                                             CreatedOnUtc = DateTime.UtcNow.AddSeconds(1),
                                        },
                                };
            blogPosts.ForEach(bp => _blogPostRepository.Insert(bp));

            //search engine names
            foreach (var blogPost in blogPosts)
            {
                _urlRecordRepository.Insert(new UrlRecord()
                {
                    EntityId = blogPost.Id,
                    EntityName = "BlogPost",
                    LanguageId = blogPost.LanguageId,
                    IsActive = true,
                    Slug = blogPost.ValidateSeName("", blogPost.Title, true)
                });
            }
        }

        private void InstallNews()
        {
            var defaultLanguage = _languageRepository.Table.FirstOrDefault();
            var news = new List<NewsItem>
                                {
                                    new NewsItem
                                        {
                                             AllowComments = true,
                                             Language = defaultLanguage,
                                             Title = "nopCommerce new release!",
                                             Short = "nopCommerce includes everything you need to begin your e-commerce online store. We have thought of everything and it's all included!<br /><br />nopCommerce is a fully customizable shopping cart. It's stable and highly usable. From downloads to documentation, www.nopCommerce.com offers a comprehensive base of information, resources, and support to the nopCommerce community.",
                                             Full = "<p>nopCommerce includes everything you need to begin your e-commerce online store. We have thought of everything and it's all included!</p><p>For full feature list go to <a href=\"http://www.nopCommerce.com\">nopCommerce.com</a></p><p>Providing outstanding custom search engine optimization, web development services and e-commerce development solutions to our clients at a fair price in a professional manner.</p>",
                                             Published  = true,
                                             CreatedOnUtc = DateTime.UtcNow,
                                        },
                                    new NewsItem
                                        {
                                             AllowComments = true,
                                             Language = defaultLanguage,
                                             Title = "New online store is open!",
                                             Short = "The new nopCommerce store is open now! We are very excited to offer our new range of products. We will be constantly adding to our range so please register on our site, this will enable you to keep up to date with any new products.",
                                             Full = "<p>Our online store is officially up and running. Stock up for the holiday season! We have a great selection of items. We will be constantly adding to our range so please register on our site, this will enable you to keep up to date with any new products.</p><p>All shipping is worldwide and will leave the same day an order is placed! Happy Shopping and spread the word!!</p>",
                                             Published  = true,
                                             CreatedOnUtc = DateTime.UtcNow.AddSeconds(1),
                                        },
                                };
            news.ForEach(n => _newsItemRepository.Insert(n));

            //search engine names
            foreach (var newsItem in news)
            {
                _urlRecordRepository.Insert(new UrlRecord()
                {
                    EntityId = newsItem.Id,
                    EntityName = "NewsItem",
                    LanguageId = newsItem.LanguageId,
                    IsActive = true,
                    Slug = newsItem.ValidateSeName("", newsItem.Title, true)
                });
            }
        }

        private void InstallSpecificationAttributes()
        {
            var sa1 = new SpecificationAttribute
            {
                Name = "Color",
                DisplayOrder = 0,
            };
            sa1.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Pink",
                DisplayOrder = 1,
            });
            sa1.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Brown",
                DisplayOrder = 2,
            });
            sa1.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Multi",
                DisplayOrder = 3,
            });
            sa1.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Red",
                DisplayOrder = 4,
            });


            var sa2 = new SpecificationAttribute
            {
                Name = "Material",
                DisplayOrder = 1,
            };
            sa2.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Nubuk",
                DisplayOrder = 1,
            });
            sa2.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Leather",
                DisplayOrder = 2,
            });
            sa2.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Wool",
                DisplayOrder = 3,
            });
            sa2.SpecificationAttributeOptions.Add(new SpecificationAttributeOption()
            {
                Name = "Suede",
                DisplayOrder = 4,
            });

            var specificationAttributes = new List<SpecificationAttribute>
                                {
                                    sa1,
                                    sa2
                                };
            specificationAttributes.ForEach(sa => _specificationAttributeRepository.Insert(sa));

        }

        private void InstallProductAttributes()
        {
            var productAttributes = new List<ProductAttribute>
            {
                new ProductAttribute
                {
                    Name = "Color",
                },
                new ProductAttribute
                {
                    Name = "Size",
                },
            };
            productAttributes.ForEach(pa => _productAttributeRepository.Insert(pa));
        }

        private void InstallManufacturers()
        {
            var sampleImagesPath = _webHelper.MapPath("~/Themes/Clothes/Content/images/SampleData/");
            var manufacturerTemplateInGridAndLines =
                _manufacturerTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Products in Grid or Lines") ??
                _manufacturerTemplateRepository.Table.FirstOrDefault();

            var allManufacturers = new List<Manufacturer>();

            var manufacturerMadder = new Manufacturer
            {
                Name = "Madder Clothes",
                ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                Published = true,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PriceRanges = "-150;150-200;200-",
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer-Madder.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Madder clothes"), true).Id,
            };
            _manufacturerRepository.Insert(manufacturerMadder);
            allManufacturers.Add(manufacturerMadder);

            var manufacturerDadies = new Manufacturer
            {
                Name = "Ladies Only Sports",
                ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                Published = true,
                DisplayOrder = 1,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PriceRanges = "-150;150-200;200-",
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer-Dadies.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Ladies Only Sports"), true).Id,
            };
            _manufacturerRepository.Insert(manufacturerDadies);
            allManufacturers.Add(manufacturerDadies);

            var manufacturerMusket = new Manufacturer
            {
                Name = "Musket Wear",
                ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                Published = true,
                DisplayOrder = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PriceRanges = "-150;150-200;200-",
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer-Musket.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Musket Wear"), true).Id,
            };
            _manufacturerRepository.Insert(manufacturerMusket);
            allManufacturers.Add(manufacturerMusket);

            var manufacturerKiddy = new Manufacturer
            {
                Name = "Kiddy Wear",
                ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                Published = true,
                DisplayOrder = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PriceRanges = "-150;150-200;200-",
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer-Kiddy.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Kiddy Wear"), true).Id,
            };
            _manufacturerRepository.Insert(manufacturerKiddy);
            allManufacturers.Add(manufacturerKiddy);

            var manufacturerGore = new Manufacturer
            {
                Name = "Gore Bike Wear",
                ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                Published = true,
                DisplayOrder = 4,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PriceRanges = "-150;150-200;200-",
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer-Gore.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Gore Bike Wear"), true).Id,
            };
            _manufacturerRepository.Insert(manufacturerGore);
            allManufacturers.Add(manufacturerGore);

            //search engine names
            foreach (var manufacturer in allManufacturers)
            {
                _urlRecordRepository.Insert(new UrlRecord()
                {
                    EntityId = manufacturer.Id,
                    EntityName = "Manufacturer",
                    LanguageId = 0,
                    IsActive = true,
                    Slug = manufacturer.ValidateSeName("", manufacturer.Name, true)
                });
            }
        }

        private void InstallCategories()
        {
            var sampleImagesPath = _webHelper.MapPath("~/Themes/Clothes/Content/images/SampleData/");
            var categoryTemplateInGridAndLines =
                _categoryTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Products in Grid or Lines") ??
                _categoryTemplateRepository.Table.FirstOrDefault();

            var allCategories = new List<Category>();

            var categoryApparel = new Category
            {
                Name = "Apparel",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Apparel",
                MetaDescription = "Apparel",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-100;100-200;200-;",
                Published = true,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-apparel.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Apparel"), true).Id,
            };
            allCategories.Add(categoryApparel);
            _categoryRepository.Insert(categoryApparel);

            var categoryDresses = new Category
            {
                Name = "Dresses",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Dresses",
                MetaDescription = "Dresses",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-100;100-200;200-;",
                Published = true,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                ParentCategoryId = categoryApparel.Id,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-dresses.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Dresses"), true).Id,
            };
            allCategories.Add(categoryDresses);
            _categoryRepository.Insert(categoryDresses);

            var categoryTops = new Category
            {
                Name = "Tops",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Tops",
                MetaDescription = "Tops",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-100;1000-200;200-;",
                Published = true,
                DisplayOrder = 1,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                ParentCategoryId = categoryApparel.Id,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-tops.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Tops"), true).Id,
            };
            allCategories.Add(categoryTops);
            _categoryRepository.Insert(categoryTops);

            var categoryBottoms = new Category
            {
                Name = "Bottoms",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Bottoms",
                MetaDescription = "Bottoms",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-100;100-200;200-;",
                Published = true,
                DisplayOrder = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                ParentCategoryId = categoryApparel.Id,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-bottoms.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Bottoms"), true).Id,
            };
            allCategories.Add(categoryBottoms);
            _categoryRepository.Insert(categoryBottoms);

            var categoryLingerie = new Category
            {
                Name = "Lingerie",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Lingerie",
                MetaDescription = "Lingerie",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-70;70-120;120-;",
                Published = true,
                DisplayOrder = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                ParentCategoryId = categoryApparel.Id,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-lingerie.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Lingerie"), true).Id,
            };
            _categoryRepository.Insert(categoryLingerie);
            allCategories.Add(categoryLingerie);

            var categoryShoes = new Category
            {
                Name = "Shoes",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Shoes",
                MetaDescription = "Shoes",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-10;10-50;50-;",
                Published = true,
                DisplayOrder = 1,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-shoes.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Shoes"), true).Id,
            };
            _categoryRepository.Insert(categoryShoes);
            allCategories.Add(categoryShoes);

            var categoryBeauty = new Category
            {
                Name = "Beauty",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Bridal jewelry",
                MetaDescription = "Bridal jewelry",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-10;10-50;50-;",
                Published = true,
                DisplayOrder = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-beauty.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Beauty"), true).Id,
            };
            _categoryRepository.Insert(categoryBeauty);
            allCategories.Add(categoryBeauty);

            var categoryAccessories = new Category
            {
                Name = "Accessories",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Accessories",
                MetaDescription = "Accessories",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 12, 24",
                PriceRanges = "-50;50-100;100-;",
                Published = true,
                DisplayOrder = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-accessories.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Accessories"), true).Id,
            };
            _categoryRepository.Insert(categoryAccessories);
            allCategories.Add(categoryAccessories);

            var categorySale = new Category
            {
                Name = "Sale",
                CategoryTemplateId = categoryTemplateInGridAndLines.Id,
                MetaKeywords = "Sale",
                MetaDescription = "Sale",
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "9, 6, 18, 36",
                PriceRanges = "-100;100-500;500-;",
                Published = true,
                DisplayOrder = 4,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category-sale.jpg"), "image/jpeg", _pictureService.GetPictureSeName("Sale"), true).Id,
            };
            _categoryRepository.Insert(categorySale);
            allCategories.Add(categorySale);

            //search engine names
            foreach (var category in allCategories)
            {
                _urlRecordRepository.Insert(new UrlRecord()
                {
                    EntityId = category.Id,
                    EntityName = "Category",
                    LanguageId = 0,
                    IsActive = true,
                    Slug = category.ValidateSeName("", category.Name, true)
                });
            }
        }

        private void AddProductTag(Product product, string tag)
        {
            var productTag = _productTagRepository.Table.FirstOrDefault(pt => pt.Name == tag);
            if (productTag == null)
            {
                productTag = new ProductTag()
                {
                    Name = tag,
                };
            }
            product.ProductTags.Add(productTag);
            _productRepository.Update(product);
        }

        // private void InstallProducts()
        //{
        //    //pictures
        //    var sampleImagesPath = _webHelper.MapPath("~/Themes/Clothes/Content/images/SampleData/");

        //    var productTemplateInGrid =
        //        _productTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Variants in Grid") ??
        //        _productTemplateRepository.Table.FirstOrDefault();
        //    var productTemplateSingleVariant =
        //        _productTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Single Product Variant") ??
        //        _productTemplateRepository.Table.FirstOrDefault();

        //    var allProducts = new List<Product>();


        //    var productCassiopeiaEmbroideredDress = new Product()
        //    {
        //        Name = "PATTERN OFF-THE-SHOULDER MINI DRESS",
        //        ShortDescription = "<p>Chiffon hi-low sleeveless dress with embroidered detailing all over. Square neckline in the front. Layer of ruffled fabric around the front. Lined.</p><p>The delicately romantic and texture heavy New Romantics collection was inspired by the dream-worthy beach views and magically hidden coves of Sardinia, Italy.</p><p>Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p>",
        //        FullDescription = "<p>Sheer double layered chiffon mini dress with gorgeous embroidered detailing around the neckline in the front and back. Zips at upper back with cutout detailing at the middle back. Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p> <p><ul><li>100% Polyester</li><li>Machine Wash Cold</li><li>Import</li></ul></p>",
        //        ProductTemplateId = productTemplateInGrid.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productCassiopeiaEmbroideredDress);
        //    productCassiopeiaEmbroideredDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 230M,
        //        Price = 199M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productCassiopeiaEmbroideredDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productCassiopeiaEmbroideredDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productCassiopeiaEmbroideredDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Madder clothes")
        //    });

        //    //pictures
        //    productCassiopeiaEmbroideredDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-CassiopeiaEmbroideredDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productCassiopeiaEmbroideredDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productCassiopeiaEmbroideredDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-CassiopeiaEmbroideredDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productCassiopeiaEmbroideredDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productCassiopeiaEmbroideredDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-CassiopeiaEmbroideredDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productCassiopeiaEmbroideredDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    var pvaCassiopeiaEmbroideredDress = new ProductVariantAttribute()
        //    {
        //        ProductAttribute = _productAttributeRepository.Table.Where(x => x.Name == "Size").FirstOrDefault(),
        //        AttributeControlType = AttributeControlType.DropdownList,
        //        IsRequired = true,
        //    };
        //    pvaCassiopeiaEmbroideredDress.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "XS",
        //        DisplayOrder = 1,
        //    });
        //    pvaCassiopeiaEmbroideredDress.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "S",
        //        DisplayOrder = 2,
        //    });
        //    pvaCassiopeiaEmbroideredDress.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "M",
        //        DisplayOrder = 3,
        //    });
        //    pvaCassiopeiaEmbroideredDress.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "L",
        //        DisplayOrder = 4,
        //    });

        //    productCassiopeiaEmbroideredDress.ProductVariants.FirstOrDefault().ProductVariantAttributes.Add(pvaCassiopeiaEmbroideredDress);


        //    var pvaCassiopeiaEmbroideredDress1 = new ProductVariantAttribute()
        //    {
        //        ProductAttribute = _productAttributeRepository.Table.Where(x => x.Name == "Color").FirstOrDefault(),
        //        AttributeControlType = AttributeControlType.ColorSquares,
        //        IsRequired = false,
        //    };
        //    pvaCassiopeiaEmbroideredDress1.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Pink",
        //        ColorSquaresRgb = "#d1bec0",
        //        IsPreSelected = true,
        //        DisplayOrder = 1,
        //    });
        //    pvaCassiopeiaEmbroideredDress1.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Brown",
        //        ColorSquaresRgb = "#4e302e",
        //        IsPreSelected = true,
        //        DisplayOrder = 2,
        //    });
        //    pvaCassiopeiaEmbroideredDress1.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Yellow",
        //        ColorSquaresRgb = "#e5c36a",
        //        IsPreSelected = true,
        //        DisplayOrder = 3,
        //    });

        //    productCassiopeiaEmbroideredDress.ProductVariants.FirstOrDefault().ProductVariantAttributes.Add(pvaCassiopeiaEmbroideredDress1);
             
        //     _productRepository.Insert(productCassiopeiaEmbroideredDress);


        //    var productHarvestEmbroideryDress = new Product()
        //    {
        //        Name = "DressBerry Women Off-White & Black Print Top",
        //        ShortDescription = "<p>Floral printed mini sleeveless dress with embroidered trimming around the neckline, upper back and around the mesh cutout at the front of the waist. Adjustable shoulder straps and crisscross straps in the back. Zips up the back. Stretchy smocking at each side of the back. Lined. </p><p>Sheer lace slip dress with high neckline at front and low 'V'-back. Skirt portion is double layered with ruffled trimming. Hangs longer on the sides than the middle. Texturized FP New Romantics Cassiopeia Embroidered Dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p>",
        //        FullDescription = "<p>Pieced together geometric slip dress with lace trimming. Racer back style. Stretchy smocked detailing on front and back. Delicate and pretty. Perfect for layering, or wearing on its own with a seamless romper underneath!</p><p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p> <p><ul><li>100% Rayon</li><li>Machine Wash Cold</li><li>Import </li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productHarvestEmbroideryDress);
        //    productHarvestEmbroideryDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 500M,
        //        Price = 399M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productHarvestEmbroideryDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productHarvestEmbroideryDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Tops"),
        //        DisplayOrder = 1,
        //    });
        //    productHarvestEmbroideryDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productHarvestEmbroideryDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productHarvestEmbroideryDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Madder clothes")
        //    });

        //    //pictures
        //    productHarvestEmbroideryDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-HarvestEmbroideryDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productHarvestEmbroideryDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productHarvestEmbroideryDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-HarvestEmbroideryDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productHarvestEmbroideryDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productHarvestEmbroideryDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-HarvestEmbroideryDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productHarvestEmbroideryDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productHarvestEmbroideryDress);


        //    var productBandoDress = new Product()
        //    {
        //        Name = "Printed chiffon mini dress",
        //        ShortDescription = "<p>Printed chiffon cape-like mini dress with ruffled trimming. Keyhole opening at front of chest with ties. Oversized, butterfly-style sleeves.</p><p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed. </p> <p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p>",
        //        FullDescription = "<p>Cotton hi-low dress with beautiful embroidered detailing at front of chest and upper back. Comes with slip to wear underneath for extra coverage. Slip has adjustable shoulder straps.</p><p>Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p> <p><ul><li>100% Polyester</li><li>Machine Wash Cold</li><li>Made in the USA</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productBandoDress);
        //    productBandoDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productBandoDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productBandoDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productBandoDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Madder clothes")
        //    });

        //    //pictures
        //    productBandoDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-bandoDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBandoDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productBandoDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-bandoDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBandoDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productBandoDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-bandoDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBandoDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productBandoDress);


        //    var productFittedWithDaisiesDress = new Product()
        //    {
        //        Name = "Fitted With Daisies and wonderful Floral print Dress",
        //        ShortDescription = "<p>Fitted With Daisies Dress with daisy chain cutout detailing around waist. Zips up the back. <p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p></p><p>Chic mini tank dress with contrast lace trimming near the front neckline and around the entire bottom hem. Zips up left side of waist. Low scoop back. Fitted at the waist and gently flares out below. </p>",
        //        FullDescription = "<p>Lightly crinkled slip dress with embroidered detailing. Bottom hem is raw edge and lightly frayed. Braided straps. Texturized FP New Romantics Cassiopeia Embroidered Dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p> <p><ul><li>100% Rayon</li><li>Machine Wash Cold</li><li>Import </li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productFittedWithDaisiesDress);
        //    productFittedWithDaisiesDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 299M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productFittedWithDaisiesDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productFittedWithDaisiesDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Tops"),
        //        DisplayOrder = 1,
        //    });
        //    productFittedWithDaisiesDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productFittedWithDaisiesDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Madder clothes")
        //    });

        //    //pictures
        //    productFittedWithDaisiesDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-FittedWithDaisiesDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productFittedWithDaisiesDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productFittedWithDaisiesDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-FittedWithDaisiesDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productFittedWithDaisiesDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productFittedWithDaisiesDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-FittedWithDaisiesDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productFittedWithDaisiesDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productFittedWithDaisiesDress);


        //    var productBlueLineDress = new Product()
        //    {
        //        Name = "FP New Romantics Cassiopeia Embroidered Dress",
        //        ShortDescription = "<p>Texturized FP New Romantics Cassiopeia Embroidered Dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume. Eyelet detailed mini sundress with floral trimming at the front of chest and around the bottom. Upper back is smocked and stretchy. Zips up left side of waist. Extreme hi-low hem. Bottom hem is has eyelet ruffle trimming all around. Light ruching of fabric below waistline. Lined. </p><p>Sheer lace slip dress with high neckline at front and low 'V'-back. Skirt portion is double layered with ruffled trimming. Hangs longer on the sides than the middle.</p>",
        //        FullDescription = "<p>Floral printed silk maxi dress with fitted waist and long ruffled bottom hem. Zips up the back. Luxurious, feminine and delicately dramatic. </p><p>Chic mini tank dress with contrast lace trimming near the front neckline and around the entire bottom hem. Zips up left side of waist. Low scoop back. Fitted at the waist and gently flares out below.</p> <p><ul><li>100% Rayon</li><li>Machine Wash Cold</li><li>Import </li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productBlueLineDress);
        //    productBlueLineDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 300M,
        //        Price = 199M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productBlueLineDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productBlueLineDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productBlueLineDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productBlueLineDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Madder clothes")
        //    });

        //    //pictures
        //    productBlueLineDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-blueLineDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlueLineDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productBlueLineDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-blueLineDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlueLineDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productBlueLineDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-blueLineDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlueLineDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productBlueLineDress);


        //    var productOrnamentDress = new Product()
        //    {
        //        Name = "Colorfully graphic printed kaftan dress",
        //        ShortDescription = "<p>Colorfully graphic printed kaftan dress with tie detailing at front of neckline. Two hip pockets. Slit at bottom of front and on both bottom sides. Oversized fit. Makes a beautiful beach cover up!</p><p>Floral printed crinkle chiffon maxi dress with 'V'-neckline and adjustable shoulder straps. Button detailing at front of chest. Upper back is stretchy. Lined.</p> <p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p>",
        //        FullDescription = "<p>Eyelet detailed mini sundress with floral trimming at the front of chest and around the bottom. Upper back is smocked and stretchy. Zips up left side of waist. Extreme hi-low hem. Bottom hem is has eyelet ruffle trimming all around. Light ruching of fabric below waistline. Lined. </p> <p><ul><li>100% Tencel</li><li>Machine Wash Cold</li><li>Import </li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productOrnamentDress);
        //    productOrnamentDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 199M,
        //        Price = 89M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productOrnamentDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productOrnamentDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productOrnamentDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productOrnamentDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Madder clothes")
        //    });

        //    //pictures
        //    productOrnamentDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-OrnamentDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productOrnamentDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productOrnamentDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-OrnamentDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productOrnamentDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productOrnamentDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-OrnamentDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productOrnamentDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productOrnamentDress);


        //     var productTriangleDress = new Product()
        //    {
        //        Name = "TEXTURIZED FIT-N-FLARE TANK MINI DRESS",
        //        ShortDescription = "<p>Eyelet detailed mini sundress with floral trimming at the front of chest and around the bottom. Upper back is smocked and stretchy. Zips up left side of waist. Extreme hi-low hem. Bottom hem is has eyelet ruffle trimming all around. Light ruching of fabric below waistline. Lined. </p><p>Polka dotted chiffon maxi dress with sheer flutter sleeves. Skirt portion of dress has ruffled detailing all around. Fitted at the waist. Square drop back with ties at the back of the neck.</p>",
        //        FullDescription = "<p>Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume. </p><p>Texturized FP New Romantics Cassiopeia Embroidered Dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p> <p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p><p><ul><li>100% Tencel</li><li>Machine Wash Cold</li><li>Made in the USA</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productTriangleDress);
        //    productTriangleDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 150M,
        //        Price = 130M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productTriangleDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productTriangleDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productTriangleDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Gore Bike Wear")
        //    });

        //    //pictures
        //    productTriangleDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-TriangleDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productTriangleDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productTriangleDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-TriangleDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productTriangleDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productTriangleDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-TriangleDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productTriangleDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productTriangleDress);


        //    var productBlackPinkDress = new Product()
        //    {
        //        Name = "Harvest Embroidery Dress",
        //        ShortDescription = "<p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p><p>Polka dotted chiffon maxi dress with sheer flutter sleeves. Skirt portion of dress has ruffled detailing all around. Fitted at the waist. Square drop back with ties at the back of the neck. Polka dotted chiffon maxi dress with sheer flutter sleeves. Skirt portion of dress has ruffled detailing all around. Fitted at the waist. Square drop back with ties at the back of the neck.</p>",
        //        FullDescription = "<p>Eyelet detailed mini sundress with floral trimming at the front of chest and around the bottom. Upper back is smocked and stretchy. Zips up left side of waist. Extreme hi-low hem. Bottom hem is has eyelet ruffle trimming all around. Light ruching of fabric below waistline. Lined. </p><p>Sheer lace slip dress with high neckline at front and low 'V'-back. Skirt portion is double layered with ruffled trimming. Hangs longer on the sides than the middle.</p> <p><ul><li>100% Tencel</li><li>Machine Wash Cold</li><li>Import </li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productBlackPinkDress);
        //    productBlackPinkDress.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productBlackPinkDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productBlackPinkDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Tops"),
        //        DisplayOrder = 1,
        //    });
        //    productBlackPinkDress.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productBlackPinkDress.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Gore Bike Wear")
        //    });

        //    //pictures
        //    productBlackPinkDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BlackPinkDress-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlackPinkDress.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productBlackPinkDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BlackPinkDress-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlackPinkDress.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productBlackPinkDress.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BlackPinkDress-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlackPinkDress.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productBlackPinkDress);


        //    var productWhiteTop = new Product()
        //    {
        //        Name = "Plenty by Tracy Reese Sights of the Shoreline White Dress",
        //        ShortDescription = "<p>Printed chiffon cape-like mini dress with ruffled trimming. Keyhole opening at front of chest with ties. Oversized, butterfly-style sleeves.</p><p>Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume. </p>",
        //        FullDescription = "<p>Floral printed crinkle chiffon maxi dress with 'V'-neckline and adjustable shoulder straps. Button detailing at front of chest. Upper back is stretchy. Lined. Eyelet detailed mini sundress with floral trimming at the front of chest and around the bottom. Upper back is smocked and stretchy. Zips up left side of waist. Extreme hi-low hem. Bottom hem is has eyelet ruffle trimming all around. Light ruching of fabric below waistline</p> <p><ul><li>95% Cotton, 5% Polyurethane</li><li>Machine Wash Cold</li><li>Import </li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productWhiteTop);
        //    productWhiteTop.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productWhiteTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Dresses"),
        //        DisplayOrder = 1,
        //    });
        //    productWhiteTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productWhiteTop.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Gore Bike Wear")
        //    });

        //    //pictures
        //    productWhiteTop.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-WhiteTop-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productWhiteTop.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productWhiteTop.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-WhiteTop-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productWhiteTop.Name), true),
        //        DisplayOrder = 1,
        //    });

        //    _productRepository.Insert(productWhiteTop);


        //    var productGreenTop = new Product()
        //    {
        //        Name = "Staple The Label Neoprene Mesh Combo Dress",
        //        ShortDescription = "<p>Super soft striped tie dye burnout tee with asymmetrical hem at the front. Bottom left side has slit. </p><p>We the Free brings us back to our down-to-earth, All-American roots, made mostly with casual cottons that have a lightly distressed and perfectly worn in feel. </p><p>Gorgeous cross-design tie dye tank with asymmetrical hem. Supersoft, lightweight fabric. Oversized armholes. Chic mini tank dress with contrast lace trimming near the front neckline and around the entire bottom hem. Zips up left side of waist. Low scoop back. Fitted at the waist and gently flares out below.</p>",
        //        FullDescription = "<p>Hawaiian floral printed off-the-shoulder crop top with elastic hems. We love this styled with a straw fedora and high-waisted denim cutoffs for a beach-ready look! </p><p>Graphic printed open shoulder kaftan top. Oversized, relaxed fit. Has ties that go through the front chest of the top and can tie around the waist for a more fitted look.</p> <p>Polka dotted chiffon maxi dress with sheer flutter sleeves. Skirt portion of dress has ruffled detailing all around. Fitted at the waist. Square drop back with ties at the back of the neck. Polka dotted chiffon maxi dress with sheer flutter sleeves. Skirt portion of dress has ruffled detailing all around. Fitted at the waist. Square drop back with ties at the back of the neck.</p> <p><ul><li>95% Cotton, 5% Polyurethane</li><li>Machine Wash Cold</li><li>By One Teaspoon</li><li>Import</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productGreenTop);
        //    productGreenTop.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 30M,
        //        Price = 20M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productGreenTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Tops"),
        //        DisplayOrder = 1,
        //    });
        //    productGreenTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productGreenTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productGreenTop.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Gore Bike Wear")
        //    });

        //    //pictures
        //    productGreenTop.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-GreenTop-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productGreenTop.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productGreenTop.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-GreenTop-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productGreenTop.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productGreenTop.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-GreenTop-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productGreenTop.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productGreenTop);


        //    var productPajamas = new Product()
        //    {
        //        Name = "Seamless Romper",
        //        ShortDescription = "<p>Distressed wash oversized tunic with numbers graphic on front and back. </p><p>Each piece is special and unique so level of distressing may vary slightly between each item.</p><p>Graphic printed open shoulder kaftan top. Oversized, relaxed fit. Has ties that go through the front chest of the top and can tie around the waist for a more fitted look. Floral printed mini sleeveless dress with embroidered trimming around the neckline, upper back and around the mesh cutout at the front of the waist. Adjustable shoulder straps and crisscross straps in the back. Zips up the back. Stretchy smocking at each side of the back. Lined.</p>",
        //        FullDescription = "<p>Oversized heathered tee with 'V'-neckline in front. Cuffed sleeves. Laidback, boyfriend fit. Because of the unique wash process of this tee, the saturation of color of each tee will vary slightly. </p><p>We the Free brings us back to our down-to-earth, All-American roots, made mostly with casual cottons that have a lightly distressed and perfectly worn in feel.</p> <p><ul><li>89% Nylon, 11% Spandex</li><li>Machine Wash Cold</li><li>By One Teaspoon</li><li>Import</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productPajamas);
        //    productPajamas.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 115M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productPajamas.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Tops"),
        //        DisplayOrder = 1,
        //    });
        //    productPajamas.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Lingerie"),
        //        DisplayOrder = 1,
        //    });
        //    productPajamas.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productPajamas.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Gore Bike Wear")
        //    });

        //    //pictures
        //    productPajamas.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Pajamas-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPajamas.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productPajamas.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Pajamas-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPajamas.Name), true),
        //        DisplayOrder = 1,
        //    });

        //    _productRepository.Insert(productPajamas);


        //    var productBra = new Product()
        //    {
        //        Name = "Solid Balconette Bra",
        //        ShortDescription = "<p>Solid-colored demi balconette bra with underwire. Side boning for extra support. Has a smooth cup lining for a seamless shape, perfect for wearing under t-shirts. </p><p>Adjustable shoulder straps and clasp closure in the back. Shoulder straps are removable and convertible, so that the bra can be worn in 5 different ways: classic, strapless, one-shoulder, crossback and halter. </p><ul><li>State: New York</li> <li>Color options: Distressed </li><li>Fit: Boyfriend</li> <li>Five-pocket styling</li>  <li>Cotton/spandex fabric blend</li><li>Zipper fly</li></ul>",
        //        FullDescription = "<p>Oversized heathered tee with 'V'-neckline in front. Cuffed sleeves. Laidback, boyfriend fit. Because of the unique wash process of this tee, the saturation of color of each tee will vary slightly. </p> <p><ul><li>95% Cotton, 5% Polyurethane</li><li>Machine Wash Cold</li><li>By One Teaspoon</li><li>Import</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productBra);
        //    productBra.ProductVariants.Add(new ProductVariant()
        //    {
        //        Name = "White",
        //        OldPrice = 199M,
        //        Price = 119M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });
        //    productBra.ProductVariants.Add(new ProductVariant()
        //    {
        //        Name = "White",
        //        OldPrice = 199M,
        //        Price = 119M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        PictureId = _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Bra-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBra.Name), true).Id
        //    });

        //    //categories
        //    productBra.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Lingerie"),
        //        DisplayOrder = 1,
        //    });
        //    productBra.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productBra.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productBra.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Gore Bike Wear")
        //    });

        //    //pictures
        //    productBra.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Bra-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBra.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productBra.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Bra-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBra.Name), true),
        //        DisplayOrder = 1,
        //    });

        //    _productRepository.Insert(productBra);


        //    var productRedJeans = new Product()
        //    {
        //        Name = "Washed Denim Overall",
        //        ShortDescription = "<p>Distressed wash skinny denim overalls. 5-pocket style. Adjustable crisscross straps. Pocket detailing on front bib. </p><p>Printed ankle length skinny jeans with zipper and button fly closure. Subtle stretch for that perfect and most comfortable fit. </p> <p>Contrast stitching and embroidered back pockets up the style quotient on these weathered skinny jeans from MDZ. These 'Stella' jeans feature a light blue wash and a touch of comfortable stretch.</p>",
        //        FullDescription = "<p>Distressed Levi's boyfriend jeans. Medium blue wash. 5-pocket style with zipper and button fly closure. Cool slouchy fit. </p><p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed.</p> <p><ul><li>95% Cotton, 5% Polyurethane</li><li>Machine Wash Cold</li><li>By One Teaspoon</li><li>Import</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productRedJeans);
        //    productRedJeans.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 319M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productRedJeans.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Bottoms"),
        //        DisplayOrder = 1,
        //    });
        //    productRedJeans.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productRedJeans.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productRedJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedJeans-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedJeans.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productRedJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedJeans-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedJeans.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productRedJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedJeans-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedJeans.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productRedJeans);


        //    var productBlueJeans = new Product()
        //    {
        //        Name = "Blue Women's Distressed Boyfriend Jeans",
        //        ShortDescription = "<p>Look your best wearing these attractive dark wash skinny jeans. These jeans have embroidered back pockets, contrast stitching, and more that make them look trendy. The skinny style is ideal for showing off your legs while staying comfortable</p><p>Perfect for a casual day out or in, this pair of distressed five-pocket boyfriend jeans features a comfortable blend of cotton with a touch of spandex. This trendy style of blue jeans flatters a range of body types, so you can feel confident</p><p>Bleach printed distressed denim cutoff shorts with frayed bottom hems and exposed pockets at the bottom. 5-pocket style. Zipper and button fly closure. </p>",
        //        FullDescription = "<p>Contrast stitching and embroidered back pockets up the style quotient on these weathered skinny jeans from MDZ. These 'Stella' jeans are finished with a stylish dark wash and a touch of comfortable stretch. </p> <p><ul> <li>Color options: Dark blue </li><li>Fit: Bootcut</li> <li>Traditional 5-pocket design with back pocket embroidery</li>  <li>Belt loops at waistband </li><li>Zipper fly with button closure</li> <li>Contrast stitching</li><li>Distressed front details </li></ul></p>",
        //        ProductTemplateId = productTemplateInGrid.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productBlueJeans);
        //    productBlueJeans.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 300M,
        //        Price = 219M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productBlueJeans.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Bottoms"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productBlueJeans.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productBlueJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BlueJeans-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlueJeans.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productBlueJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BlueJeans-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlueJeans.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productBlueJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BlueJeans-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBlueJeans.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    var pvaBlueJeans = new ProductVariantAttribute()
        //    {
        //        ProductAttribute = _productAttributeRepository.Table.Where(x => x.Name == "Size").FirstOrDefault(),
        //        AttributeControlType = AttributeControlType.RadioList,
        //        IsRequired = true,
        //    };
        //    pvaBlueJeans.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "XS",
        //        DisplayOrder = 1,
        //    });
        //    pvaBlueJeans.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "S",
        //        DisplayOrder = 2,
        //    });
        //    pvaBlueJeans.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "M",
        //        DisplayOrder = 3,
        //    });
        //    pvaBlueJeans.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "L",
        //        DisplayOrder = 4,
        //    });

        //    productBlueJeans.ProductVariants.FirstOrDefault().ProductVariantAttributes.Add(pvaBlueJeans);
            
        //     _productRepository.Insert(productBlueJeans);


        //    var productPinkJeans = new Product()
        //    {
        //        Name = "MDZ Women's Bootcut Jeans",
        //        ShortDescription = "<p>Perfect for a casual day out or in, this pair of distressed five-pocket boyfriend jeans features a comfortable blend of cotton with a touch of spandex. This trendy style of blue jeans flatters a range of body types, so you can feel confident</p> <p>Distressed details lend a trendy finish to these contemporary jeans from Rue Blue Jeans. The pants are fashioned with a flattering boyfriend fit.</p> <p>Distressed wash skinny denim overalls. 5-pocket style. Adjustable crisscross straps. Pocket detailing on front bib. Printed ankle length skinny jeans with zipper and button fly closure. Subtle stretch for that perfect and most comfortable fit.</p>",
        //        FullDescription = "<p>Cotton hi-low dress with beautiful embroidered detailing at front of chest and upper back. Comes with slip to wear underneath for extra coverage. Slip has adjustable shoulder straps. Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume.</p><p><ul><li>State: New York</li> <li>Color options: Distressed </li><li>Fit: Boyfriend</li> <li>Five-pocket styling</li>  <li>Cotton/spandex fabric blend</li><li>Zipper fly</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productPinkJeans);
        //    productPinkJeans.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 390M,
        //        Price = 209M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productPinkJeans.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Bottoms"),
        //        DisplayOrder = 1,
        //    });
        //    productPinkJeans.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productPinkJeans.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productPinkJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-PinkJeans-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPinkJeans.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productPinkJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-PinkJeans-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPinkJeans.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productPinkJeans.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-PinkJeans-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPinkJeans.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productPinkJeans);


        //    var productShorts = new Product()
        //    {
        //        Name = "Printed Cut Off",
        //        ShortDescription = "<p>Bleach printed distressed denim cutoff shorts with frayed bottom hems and exposed pockets at the bottom. 5-pocket style. Zipper and button fly closure. </p><p>Distressed 5-pocket denim shorts with scalloped crochet trimming around bottom hems. Zipper and button fly. Some destroyed texture in places for a cool, worn-in feel and look. Bottom hems above the trimming are frayed.</p><p>Oversized heathered tee with 'V'-neckline in front. Cuffed sleeves. Laidback, boyfriend fit. Because of the unique wash process of this tee, the saturation of color of each tee will vary slightly. We the Free brings us back to our down-to-earth, All-American roots, made mostly with casual cottons that have a lightly distressed and perfectly worn in feel.</p>",
        //        FullDescription = "<p>Distressed denim cutoff shorts with sequin, bead and faux pearl embellishment all over the front. Bottom hems are frayed. Destroyed holes on </p><p><ul><li>State: New York</li> <li>Color options: Distressed </li><li>Fit: Boyfriend</li> <li>Five-pocket styling</li>  <li>Cotton/spandex fabric blend</li><li>Zipper fly</li></ul></p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productShorts);
        //    productShorts.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 199M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productShorts.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Bottoms"),
        //        DisplayOrder = 1,
        //    });
        //    productShorts.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Lingerie"),
        //        DisplayOrder = 1,
        //    });
        //    productShorts.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productShorts.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productShorts.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Shorts-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productShorts.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productShorts.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Shorts-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productShorts.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productShorts.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Shorts-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productShorts.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productShorts);


        //    var productSocks = new Product()
        //    {
        //        Name = "Rainbow Anklet 6 pack",
        //        ShortDescription = "<p>Pack of 6 pairs of rainbow colored ankle socks. Stretchy fit. </p> <p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed. </p><p>Russian valenki is a precious gift for yourself, your children and family. Valenki are traditional Russian felt boots invented a long time ago as the best solution for cold Russian winters.</p>",
        //        FullDescription = "<p>Cotton hi-low dress with beautiful embroidered detailing at front of chest and upper back. Comes with slip to wear underneath for extra coverage. Slip has adjustable shoulder straps. Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume. </p><p>Contrast stitching and embroidered back pockets up the style quotient on these weathered skinny jeans from MDZ. These 'Stella' jeans feature a light blue wash and a touch of comfortable stretch.</p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productSocks);
        //    productSocks.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 25M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productSocks.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Bottoms"),
        //        DisplayOrder = 1,
        //    });
        //    productSocks.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Accessories"),
        //        DisplayOrder = 1,
        //    });
        //    productSocks.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productSocks.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productSocks.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Socks-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSocks.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productSocks.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Socks-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSocks.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productSocks.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Socks-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSocks.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productSocks);


        //    var productPanties = new Product()
        //    {
        //        Name = "Intimately Printed Lace Bloomer",
        //        ShortDescription = "<p>Floral printed lace bloomer shorts with ruffle trimming. Elastic around waist and each leg opening.</p><p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed. </p><p>Contrast stitching and embroidered back pockets up the style quotient on these weathered skinny jeans from MDZ. These 'Stella' jeans feature a light blue wash and a touch of comfortable stretch.</p>",
        //        FullDescription = "<p>Cotton hi-low dress with beautiful embroidered detailing at front of chest and upper back. Comes with slip to wear underneath for extra coverage. Slip has adjustable shoulder straps. Texturized pattern off-the-shoulder mini dress with cool cutout detailing at the back. metallic thread pattern on upper bodice. Zips up the lower back. Shoulder straps are adjustable. Fitted at the waist. Skirt portion is lined with tiered tulle for volume. </p><p>Perfect for a casual day out or in, this pair of distressed five-pocket boyfriend jeans features a comfortable blend of cotton with a touch of spandex. This trendy style of blue jeans flatters a range of body types, so you can feel confident.</p>",
        //        ProductTemplateId = productTemplateInGrid.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productPanties);
        //    productPanties.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 100M,
        //        Price = 50M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productPanties.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Bottoms"),
        //        DisplayOrder = 1,
        //    });
        //     productPanties.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Beauty"),
        //        DisplayOrder = 1,
        //    });
        //    productPanties.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Lingerie"),
        //        DisplayOrder = 1,
        //    });
        //    productPanties.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productPanties.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productPanties.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productPanties.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Panties-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPanties.Name), true),
        //        DisplayOrder = 0,
        //    });

        //    var pvaPanties = new ProductVariantAttribute()
        //    {
        //        ProductAttribute = _productAttributeRepository.Table.Where(x => x.Name == "Color").FirstOrDefault(),
        //        AttributeControlType = AttributeControlType.ColorSquares,
        //        IsRequired = true,
        //    };
        //    pvaPanties.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Pink",
        //        IsPreSelected = true,
        //        ColorSquaresRgb = "#e6c9cb",
        //        DisplayOrder = 1,
        //    });
        //    pvaPanties.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Ivory",
        //        ColorSquaresRgb = "#eeeee0",
        //        DisplayOrder = 2,
        //    });
        //    pvaPanties.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Brown",
        //        ColorSquaresRgb = "#6e5046",
        //        DisplayOrder = 3,
        //    });

        //    productPanties.ProductVariants.FirstOrDefault().ProductVariantAttributes.Add(pvaPanties);

        //    _productRepository.Insert(productPanties);


        //    var productMulticolorTop = new Product()
        //    {
        //        Name = "Fine Jersey Stripe Racerback Tank Top",
        //        ShortDescription = "<p>Paisley printed tank top. A timeless summer shape updated in the season's favorite pattern! </p> <p> Eyelet detailed mini sundress with floral trimming at the front of chest and around the bottom. Upper back is smocked and stretchy. Zips up left side of waist. Extreme hi-low hem. Bottom hem is has eyelet ruffle trimming all around. Light ruching of fabric below waistline. Lined. </p> <p>Polka dotted chiffon maxi dress with sheer flutter sleeves. Skirt portion of dress has ruffled detailing all around. Fitted at the waist. Square drop back with ties at the back of the neck.</p>",
        //        FullDescription = "<p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed. Printed chiffon cape-like mini dress with ruffled trimming. Keyhole opening at front of chest with ties. Oversized, butterfly-style sleeves.</p> <p>Distressed wash buttonfront apron-style dress. Two front hip pockets and one front chest pocket. Cutout detailing at upper back. Shoulder straps each have a button that attaches to the front of the dress. Zips up back of waist. Bottom hems are frayed. Chic mini tank dress with contrast lace trimming near the front neckline and around the entire bottom hem. Zips up left side of waist. Low scoop back. Fitted at the waist and gently flares out below.</p>",
        //        ProductTemplateId = productTemplateInGrid.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productMulticolorTop);
        //    productMulticolorTop.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 35M,
        //        Price = 29M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productMulticolorTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Lingerie"),
        //        DisplayOrder = 1,
        //    });
        //    productMulticolorTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Tops"),
        //        DisplayOrder = 1,
        //    });
        //    productMulticolorTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });
        //    productMulticolorTop.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productMulticolorTop.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Musket Wear")
        //    });

        //    //pictures
        //    productMulticolorTop.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-MulticolorTop-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMulticolorTop.Name), true),
        //        DisplayOrder = 0,
        //    });

        //    var pvaMulticolorTop = new ProductVariantAttribute()
        //    {
        //        ProductAttribute = _productAttributeRepository.Table.Where(x => x.Name == "Color").FirstOrDefault(),
        //        AttributeControlType = AttributeControlType.ColorSquares,
        //        IsRequired = true,
        //    };
        //    pvaMulticolorTop.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Pink",
        //        IsPreSelected = true,
        //        ColorSquaresRgb = "#cd9cb9",
        //        DisplayOrder = 1,
        //    });
        //    pvaMulticolorTop.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Gold",
        //        ColorSquaresRgb = "#95776f",
        //        DisplayOrder = 2,
        //    });
        //    pvaMulticolorTop.ProductVariantAttributeValues.Add(new ProductVariantAttributeValue()
        //    {
        //        Name = "Brown",
        //        ColorSquaresRgb = "#755958",
        //        DisplayOrder = 3,
        //    });

        //    productMulticolorTop.ProductVariants.FirstOrDefault().ProductVariantAttributes.Add(pvaMulticolorTop);

        //    _productRepository.Insert(productMulticolorTop);


        //    var productRedSleepwear = new Product()
        //    {
        //        Name = "Printed Pajama Shirt",
        //        ShortDescription = "<p>Printed button front pajama shirt with collar. Comfy enough for sleep and pretty enough to dress up and wear out.</p><p>Pieced together printed top with floral crochet inserts. Long bubble sleeves. 'V'-neckline in front. Sleeve hems are stretchy. Hi-low hem.</p><ul><li>89% Nylon, 11% Spandex</li><li>Machine Wash Cold</li><li>By One Teaspoon</li><li>Import</li></ul>",
        //        FullDescription = "<p>Embroidered flouncy-shaped tunic with stretchy smocking around the neckline. Fabric is lightly crinkled all over. Sleeve hems are stretchy. Ties at front of neckline. *Special Note: This top does not have tassels at the end of the ties as shown in the catalog shot. The top is exactly as pictured in the studio shots.</p><ul><li>95% Cotton, 5% Polyurethane</li><li>Machine Wash Cold</li><li>By One Teaspoon</li><li>Import</li></ul>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productRedSleepwear);
        //    productRedSleepwear.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productRedSleepwear.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Lingerie"),
        //        DisplayOrder = 1,
        //    });
        //    productRedSleepwear.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Apparel"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productRedSleepwear.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Ladies Only Sports")
        //    });

        //    //pictures
        //    productRedSleepwear.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedSleepwear-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedSleepwear.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productRedSleepwear.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedSleepwear-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedSleepwear.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productRedSleepwear.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedSleepwear-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedSleepwear.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productRedSleepwear);


        //    var productMenBrownShoes = new Product()
        //    {
        //        Name = "Men's Back-To-Berkeley Canvas Boots",
        //        ShortDescription = "<p>Retro, activity-inspired styling fuses with thoroughly contemporary construction in The North Face® Men’s Back-to-Berkeley Boot Canvas. The mid-cut casual boot incorporates cotton canvas uppers with leather mudguards and sturdy lace eyelets, plus grippy rubber outsoles for traction over slick terrain.</p><p> Inside, Trek-Dry® linings help to wick away moisture to keep feet dry and comfortable, and Ortholite® footbeds contribute ample support. Great for casual wear through spring and summer and available in a range of striking colours, The North Face® Men’s Back-to-Berkeley Boot Canvas is the casual boot choice for those inspired by the outdoor life.</p>",
        //        FullDescription = "<ul><li>A lighter-weight, spring</li><li>1% cotton canvas upper</li><li>Protective, abrasion-resistant, PU-coated leather mudguard</li><li>Durable, metal lace hardware</li><li>Tonal, secondary lace option</li><li>Comfortable, moisture-wicking Trek-Dry® lining</li><li>OrthoLite® footbed BOTTOM</li><li>Die-cut EVA midsole</li><li>Durable, 4% recycled rubber outsole</li></ul>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productMenBrownShoes);
        //    productMenBrownShoes.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 199M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productMenBrownShoes.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Shoes"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productMenBrownShoes.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Ladies Only Sports")
        //    });

        //    //pictures
        //    productMenBrownShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-MenBrownShoes-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMenBrownShoes.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productMenBrownShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-MenBrownShoes-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMenBrownShoes.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productMenBrownShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-MenBrownShoes-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMenBrownShoes.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    productMenBrownShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Color").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Brown").FirstOrDefault()
        //    });
        //    productMenBrownShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Nubuk").FirstOrDefault()
        //    });

        //    _productRepository.Insert(productMenBrownShoes);


        //    var productRedShoes = new Product()
        //    {
        //        Name = "Henry Ferrera Women's Spike Embellished Pumps",
        //        ShortDescription = "<p>Steal the spotlight in these bold eye-catching pumps from Henry Ferrera. These pumps feature a vicious panel of spikes and studs across the back that will make any outfit instantly edgy.</p><p>Steal the spotlight in these bold eye-catching pumps from Henry Ferrera. These pumps feature a vicious panel of spikes and studs across the back that will make any outfit instantly edgy. </p>",
        //        FullDescription = "<ul><li>Color options: Taupe, red, black</li><li>Style: Pumps</li><li>Man-made upper</li><li>Round toe</li><li>5-inch heel</li><li>Unlined</li><li>Man-made sole</li></ul> <ul><li>Upper/outersole material: Man-made</li><li>Model: Range</li></ul>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productRedShoes);
        //    productRedShoes.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 800M,
        //        Price = 630M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productRedShoes.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Shoes"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productRedShoes.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Ladies Only Sports")
        //    });

        //    //pictures
        //    productRedShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedShoes-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedShoes.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productRedShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedShoes-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedShoes.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productRedShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-RedShoes-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productRedShoes.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    productRedShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Color").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Red").FirstOrDefault()
        //    });
        //    productRedShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Suede").FirstOrDefault()
        //    });

        //    _productRepository.Insert(productRedShoes);


        //    var productDcShoes = new Product()
        //    {
        //        Name = "DC WOMEN'S DESTROYER HI SHOES",
        //        ShortDescription = "<p>The Destroyer HI leaves the others behind. The upper is built from action leather, nubuck and synthetic leather upper, and features a triple-stitched toecap, and welded TPU detailing. </p><p>The DC logo on the side is an injected TPR, and the heel collar and tongue are foam padded for comfort. The outsole is an abrasion-resistant sticky rubber, while DC's trademarked pill pattern adorns the tread. Imported.</p>",
        //        FullDescription = "<div><p></p><div>+ Hightop&nbsp;</div><div>+ Action Leather, Nubuck Synthetic Upper&nbsp;</div><div>+ Triple Stitched Toecap&nbsp;</div><div>+ Molded TPU Eyelets&nbsp;</div><div>+ Welded TPU Details&nbsp;</div><div>+ Injection TPR Quarter Logo&nbsp;</div><div>+ Foam Padded Collar Tongue for Comfort&nbsp;</div><div>+ Cup Sole Construction&nbsp;</div><div>+ Abrasion-Resistant Sticky Rubber&nbsp;</div><div>+ DC's Trademarked 'Pill Pattern' Tread.</div><p></p></div>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = true,
        //    };
        //    allProducts.Add(productDcShoes);
        //    productDcShoes.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 120M,
        //        Price = 110M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productDcShoes.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Shoes"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productDcShoes.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Ladies Only Sports")
        //    });

        //    //pictures
        //    productDcShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-DCShoes-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productDcShoes.Name), true),
        //        DisplayOrder = 0,
        //    });

        //    productDcShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Color").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Multi").FirstOrDefault()
        //    });
        //    productDcShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Leather").FirstOrDefault()
        //    });

        //    _productRepository.Insert(productDcShoes);


        //    var productPinkShoes = new Product()
        //    {
        //        Name = "Russian valenki felt boots",
        //        ShortDescription = "<p>Russian valenki is a precious gift for yourself, your children and family. Valenki are traditional Russian felt boots invented a long time ago as the best solution for cold Russian winters. </p><p>First valenki were made centuries ago from sheep wool to keep feet warm even with 40 degrees Celcius below zero. Genius invention was to make felt from 100% natural materials always available in poor villages. Firm and high, they served perfect for many-many years in one family and were even given as heritage to younger generations.</p>",
        //        FullDescription = "<p>With the time people could not go without them. It is said, that Peter the Great wore valenki as the best remedy against hangover. True or not, we can't prove it but tend to believe. As a fact, Soviet soldiers could resist during II World War partly thanks to the best possible winter uniform with a must pair of valenki. Since then valenki boots are mass manufactured for Russian army and there is not another alternative because of their frost resistance, warmness and longevity. </p><p>Russian valenki are to be worn inside and outside. If you wear valenki inside, they are just like perfect light wool socks with a special healing effect through wool warmth and micro massage. If you wear valenki outside, then you enjoy frosty winters days without getting cold. For outside, you can take valenki as is, or with galoshes (rubber shoes) or on rubber soles to be protected from getting wet.</p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productPinkShoes);
        //    productPinkShoes.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 140M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productPinkShoes.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Shoes"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productPinkShoes.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Ladies Only Sports")
        //    });

        //    //pictures
        //    productPinkShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-PinkShoes-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPinkShoes.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productPinkShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-PinkShoes-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPinkShoes.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productPinkShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-PinkShoes-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPinkShoes.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    productPinkShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Color").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Pink").FirstOrDefault()
        //    });
        //    productPinkShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Wool").FirstOrDefault()
        //    });

        //    _productRepository.Insert(productPinkShoes);


        //    var productFootballBoots = new Product()
        //    {
        //        Name = "Puma Football Boots",
        //        ShortDescription = "<p>Engineered to enhance power!</p><p>These football boots from PUMA are engineered to enhance power and perfect for firm natural surfaces. This new PowerCat 1 is the perfect combination of excellent engineering and a striking artistic approach: It features a graphic story which depicts the tribal qualities that bond players and fans together - to the power, energy, and loyalty on the pitch and in the stands. The double-density PUMA 3D PST DUO technology in combination with the super soft and lightweight microfibre material makes the PowerCat 1 the boot of choice for players all over the world. PUMA PowerLast and the less asymmetrical lacing system allow for a snug yet comfortable fit.</p>",
        //        FullDescription = "<ul><li>powerCELL maximises your power and precision when kicking, and improves your level of play.</li><li>PUMA 3D PST DUO Technology helps improve ball grip and kicking accuracy.</li><li>PUMA PowerLast provides more volume in the forefoot and instep. It follows the foot's natural contours and provides a glove-like fit.</li><li>Premium microfibre upper enhances fit, comfort, and touch.</li><li>Lightweight outsole with bladed/pointed studs: Delivers on traction, manoeuvrability, and pressure distribution - for firm natural surfaces.</li><li>External heel counter protects and stabilises the heel.</li><li>Striking PUMA and PowerCat branding.</li></ul>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productFootballBoots);
        //    productFootballBoots.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 300M,
        //        Price = 249M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productFootballBoots.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Shoes"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productFootballBoots.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Ladies Only Sports")
        //    });

        //    //pictures
        //    productFootballBoots.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-FootballBoots-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productFootballBoots.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productFootballBoots.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-FootballBoots-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productFootballBoots.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productFootballBoots.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-FootballBoots-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productFootballBoots.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    productFootballBoots.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Color").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Multi").FirstOrDefault()
        //    });
        //    productFootballBoots.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Leather").FirstOrDefault()
        //    });

        //    _productRepository.Insert(productFootballBoots);


        //    var productChildShoes = new Product()
        //    {
        //        Name = "Sesame Street® by Stride Rite Crib Elmo",
        //        ShortDescription = "<p><div>Tiny feet meet Elmo’s World in the Sesame Street® by Stride Rite® Elmo crib shoe. Part of our NEW Crib Collection for swaddling tiny feet in comfort and style.<ul><li>Leather upper</li><li>Patent-pending, easy-on closure swaddles the foot in place for a secure fit and comforting feel</li><li>Internal gore in the rear of the shoe adjusts to the foot</li><li>Microfiber, soft-soled bottom for protection and durability</li><li>Padded heel collar for added comfort</li><li>Makes a great baby shower gift!<br></li><li>Imported</li></ul></div></p>",
        //        FullDescription = "<div><strong>Leather</strong><ul><li>Brush off dry dirt, then use a damp cloth with a little mild detergent<br></li><li>Air dry<br></li></ul><br><strong>Textile/Canvas</strong><ul><li>Brush off dry dirt, then hand wash in cold water with mild detergent<br></li><li>Air dry<br></li></ul><br><strong>Faux Leather/Synthetic</strong><ul><li>Brush off dry dirt, then use a damp cloth with a little mild detergent<br></li><li>Air dry<br></li></ul><br></div>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productChildShoes);
        //    productChildShoes.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 65M,
        //        Price = 45M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productChildShoes.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Shoes"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productChildShoes.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productChildShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-ChildShoes-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productChildShoes.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productChildShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-ChildShoes-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productChildShoes.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productChildShoes.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-ChildShoes-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productChildShoes.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    productChildShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Color").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Pink").FirstOrDefault()
        //    });
        //    productChildShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Leather").FirstOrDefault()
        //    });
        //    productChildShoes.ProductSpecificationAttributes.Add(new ProductSpecificationAttribute()
        //    {
        //        AllowFiltering = true,
        //        DisplayOrder = 1,
        //        SpecificationAttributeOption = _specificationAttributeRepository.Table.Where(sa => sa.Name == "Material").FirstOrDefault()
        //        .SpecificationAttributeOptions.Where(sao => sao.Name == "Suede").FirstOrDefault()
        //    });

        //    _productRepository.Insert(productChildShoes);


        //    var productSnakeBag = new Product()
        //    {
        //        Name = "Amazing Tod’s D Bag",
        //        ShortDescription = "<p>Our latest and generously-sized tote goes anywhere and everywhere! This reliable companion will be there for work, the gym, shopping and overnights! Dimensions: 18\" x 13\" x 7\" in</p><ul><li>Recessed top zip entry</li><li>Double carrying handles have a 7\" drop</li><li>Adjustable shoulder strap with a maximum of 26\" drop</li><li>Exterior rear zipped pocket</li><li>Main compartments holds an interior wall zipped pocket, cell phone pouch and 2 pen sleeves</li><li>Interior key clasp</li></ul>",
        //        FullDescription = "<p>A terrific little cross-body bag for a look that is both functional and fashion forward. Deceptively practical with 2 zipped main compartments and extra zipped pockets on the front. Dimensions: 61/4\"L x 63/4\"H x 11/2\"D</p><ul><li>Adjustable shoulder strap</li><li>2 horizontal zipped front pockets</li><li>Main compartment contains: 1 open pouch pocket and 1 zipped pocket</li><li>Fabric: Nylon</li><li>Imported</li></ul>",
        //        ProductTemplateId = productTemplateInGrid.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productSnakeBag);
        //    productSnakeBag.ProductVariants.Add(new ProductVariant()
        //    {
        //        Name = "Violet",
        //        OldPrice = 130M,
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });
        //    productSnakeBag.ProductVariants.Add(new ProductVariant()
        //    {
        //        Name = "Yellow",
        //        OldPrice = 130M,
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        PictureId = _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-SnakeBag-6.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSnakeBag.Name), true).Id
        //    });
        //    productSnakeBag.ProductVariants.Add(new ProductVariant()
        //    {
        //        Name = "Orange",
        //        OldPrice = 130M,
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        PictureId = _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-SnakeBag-5.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSnakeBag.Name), true).Id
        //    });
        //    productSnakeBag.ProductVariants.Add(new ProductVariant()
        //    {
        //        Name = "Black",
        //        OldPrice = 130M,
        //        Price = 99M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        PictureId = _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-SnakeBag-4.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSnakeBag.Name), true).Id
        //    });

        //    //categories
        //    productSnakeBag.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Accessories"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productSnakeBag.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productSnakeBag.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-SnakeBag-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSnakeBag.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productSnakeBag.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-SnakeBag-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSnakeBag.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productSnakeBag.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-SnakeBag-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productSnakeBag.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productSnakeBag);


        //    var productBrownBag = new Product()
        //    {
        //        Name = "Christian Louboutin Schoolita Framed Bag",
        //        ShortDescription = "<p>Style meets function with the Downtown East/West Tote. Made of Vaquetta leather, it has been designed especially for organization.</p><ul><li>Vaquetta leather</li><li>Double strap tote</li><li>Designed for organization</li><li>Fully lined interior with organizer and zip pocket</li><li>Firm, flat bottom with squared corners</li><li>Dimensions: 10.5\" H x 13\" W x 3.5\" D</li></ul>",
        //        FullDescription = "<table><tbody><tr><td>Construction:</td><td>Soft-Sided</td></tr><tr><td>Other:</td><td>Additional Colors</td></tr><tr><td>Product Category:</td><td>Travel Totes</td></tr><tr><td>Type:</td><td>All Purpose Tote</td></tr><tr><td>Wheels:</td><td>No Wheels</td></tr></tbody></table>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productBrownBag);
        //    productBrownBag.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 1599M,
        //        Price = 1159M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productBrownBag.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Accessories"),
        //        DisplayOrder = 1,
        //    });
        //    productBrownBag.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productBrownBag.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productBrownBag.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BrownBag-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBrownBag.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productBrownBag.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BrownBag-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBrownBag.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productBrownBag.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-BrownBag-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productBrownBag.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productBrownBag);


        //    var productMirror = new Product()
        //    {
        //        Name = "Cookie Kawaii Oreo Mirror Keychain",
        //        ShortDescription = "<p>Mirror, mirror in my pocket who's the fairest of them all - that'll be you my darling for you have a pocket mirror that tells you how wonderful you are where ever you are.</p><p> Whether it be on the work, party, riding a horse, or simply swimming in the pool – You will always look stunning. Pocket Mirror is light and slim, it adds no uncomfortable bulge or weight and small enough to stash in a credit card slot in your wallet. The little handle also doubles as a tab that peeks out from the pockets of your billfold for quick, chivalrous removal.</p>",
        //        FullDescription = "<p>A great Lilly Pulitzer design. This is perfect for the summer time gifts you have been looking for. These pocket mirrors are the perfect size at 2.25 inches. They are quality mirrors made with real glass. This listing is for the first picture, The last picture is to show the bottom side of the mirror. <p><You can just buy 1 for yourself or 10 to give to your bridesmaids! They are wonderful to have on hand when you are out on the town and need to take a sec to do a makeup check. They are a perfect to add a little pazazz to your makeup bag, everyone will compliment you on it and want one for themselves!</p><p>Powder tones the face and gives an even appearance. Besides toning the face, some powders with sunscreen can also reduce skin damage from sunlight and environmental stress. It comes packaged either as a compact or as loose powder. It can be applied with a sponge, brush, or powder puff.</p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productMirror);
        //    productMirror.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 15M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productMirror.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Accessories"),
        //        DisplayOrder = 1,
        //    });
        //     productMirror.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Beauty"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productMirror.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productMirror.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Mirror-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMirror.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productMirror.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Mirror-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMirror.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productMirror.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Mirror-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMirror.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productMirror);

        //    var productGlasses = new Product()
        //    {
        //        Name = "Medium Full-Rim Fashion Eyeglasses",
        //        ShortDescription = "<p>Sunglasses or sun glasses are a form of protective eyewear designed primarily to prevent bright sunlight and high-energy visible light from damaging or discomforting the eyes. </p><p>They can sometimes also function as a visual aid, as variously termed spectacles or glasses exist, featuring lenses that are colored, polarized or darkened. In the early 20th century they were also known as sun cheaters (cheaters being an American slang term for glasses).</p>",
        //        FullDescription = "<p>Sunglasses offer protection against excessive exposure to light, including its visible and invisible components. The most widespread protection is against ultraviolet radiation, which can cause short-term and long-term ocular problems such as photokeratitis, snow blindness, cataracts, pterygium, and various forms of eye cancer.[8] Medical experts advise the public on the importance of wearing sunglasses to protect the eyes from UV;[8] for adequate protection, experts recommend sunglasses that reflect or filter out 99-100% of UVA and UVB light, with wavelengths up to 400 nm. Sunglasses which meet this requirement are often labeled as 0\"UV400.\" This is slightly more protection than the widely used standard of the European Union (see below), which requires that 95% of the radiation up to only 380 nm must be reflected or filtered out.[9] Sunglasses are not sufficient to protect the eyes against permanent harm from looking directly at the Sun, even during a solar eclipse.</p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productGlasses);
        //    productGlasses.ProductVariants.Add(new ProductVariant()
        //    {
        //        Price = 150M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productGlasses.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Accessories"),
        //        DisplayOrder = 1,
        //    });
        //     productGlasses.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Beauty"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productGlasses.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productGlasses.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Glasses-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productGlasses.Name), true),
        //        DisplayOrder = 0,
        //    });
        //    productGlasses.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Glasses-2.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productGlasses.Name), true),
        //        DisplayOrder = 1,
        //    });
        //    productGlasses.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Glasses-3.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productGlasses.Name), true),
        //        DisplayOrder = 2,
        //    });

        //    _productRepository.Insert(productGlasses);


        //    var productMascara = new Product()
        //    {
        //        Name = "Bunny Rouge Mascara",
        //        ShortDescription = "<p>Mascara is a cosmetic commonly used to enhance the eyes. It may darken, thicken, lengthen, and/or define the eyelashes. Normally in one of three forms—liquid, cake, or cream—the modern mascara product has various formulas; however, most contain the same basic components of pigments, oils, waxes, and preservatives.</p><p>The increased demand for mascara led to the development of the many formulas seen in the current market. Despite the many variations, all formulas contain the same basic elements: pigmentation, oils, and waxes.</p>",
        //        FullDescription = "<p>The Collins English Dictionary defines mascara as \"a cosmetic substance for darkening, curling, coloring, and thickening the eyelashes, applied with a brush or rod.\" The Oxford English Dictionary (OED) adds that mascara is occasionally used on the eyebrows as well. The OED also references mascaro from works published in the late 1400s. In 1886, the Peck & Snyder Catalogue advertises, “Mascaro or Water Cosmetique… For darkening the eyebrow and moustaches without greasing them and making them prominent.” In 1890, the Century Dictionary defined mascara as “a kind of paint used for the eyebrows and eyelashes by actors.” And in 1894, N. Lynn advises in Lynn’s Practical Hints for Making-up, “to darken eyelashes, paint with mascara, or black paint, with a small brush.</p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productMascara);
        //    productMascara.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 39M,
        //        Price = 30M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productMascara.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Beauty"),
        //        DisplayOrder = 1,
        //    });
        //    productMascara.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productMascara.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productMascara.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Mascara-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productMascara.Name), true),
        //        DisplayOrder = 0,
        //    });
            
        //    _productRepository.Insert(productMascara);


        //    var productPowder = new Product()
        //    {
        //        Name = "Facefinity Compact №006",
        //        ShortDescription = "<p>Face powder is a cosmetic powder applied to the face to set a foundation after application. It can also be reapplied throughout the day to minimize shininess caused by oily skin. There is translucent sheer powder, and there is pigmented powder. Certain types of pigmented facial powders are meant be worn alone with no base foundation.</p><p> Powder tones the face and gives an even appearance. Besides toning the face, some powders with sunscreen can also reduce skin damage from sunlight and environmental stress. It comes packaged either as a compact or as loose powder. It can be applied with a sponge, brush, or powder puff.</p>",
        //        FullDescription = "<p>Because of the wide variation among human skin tones, there is a corresponding variety of colors of face powder. There are also several types of powder. A common powder used in beauty products is talc (or baby powder), which is absorbent and provides toning to the skin. </p><p>Face powder should be carefully chosen to match the skin tone in order to show the best results.</p><p>The increased demand for mascara led to the development of the many formulas seen in the current market. Despite the many variations, all formulas contain the same basic elements: pigmentation, oils, and waxes.</p>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productPowder);
        //    productPowder.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 25M,
        //        Price = 20M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productPowder.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Accessories"),
        //        DisplayOrder = 1,
        //    });
        //    productPowder.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Beauty"),
        //        DisplayOrder = 1,
        //    });
        //    productPowder.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productPowder.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productPowder.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Powder-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productPowder.Name), true),
        //        DisplayOrder = 0,
        //    });

        //    _productRepository.Insert(productPowder);


        //    var productTone = new Product()
        //    {
        //        Name = "Foundation Smooth Effect №80",
        //        ShortDescription = "<p>Gently and effectively fades skin discolorations for a more even skin tone. Completely balanced formula includes antioxidant vitamins E & C (Tocopheryl Acetate & Ascorbic Acid) to protect against damage due to free radicals. Full spectrum UVA & UVB sunscreens also help prevent the recurrence of dark spots. Moisturizing emollients plus aloe vera are balanced to your skin type.</p><ul> <li>With antioxidant vitamins to fight skin aging&nbsp;&nbsp;  </li><li>Specially balanced for normal skin</li></ul> <p>Made in USA</p>",
        //        FullDescription = "<ul> <li>If skin is sensitive, test on a small area inside the elbow overnight before use  </li><li>Adults and children 12 years and older: Apply as a thin layer to affected area twice daily or as directed by a doctor  </li><li>Children under 12 years of age: Ask a doctor before use</li></ul></br> <div><div><div>Active Ingredients: </div>Hydroquinone, Oxybenzone, Homosalate<br><br><div>Inactive Ingredients:</div>Water, Glyceryl Stearate, Triethanolamine, Lactic Acidan Alpha Hydroxy Acid, Decyl Oleate, PEG-100 Stearate, Salicylic Acida Beta Hydroxy Acid, Cetyl Alcohol, Stearic Acid, Palmitic Acid, Magnesium Aluminum Silicate, Tocopheryl Acetate (Vitamin E), Ascorbic Acidvitamin C, Aloe Vera Extract, Dimethicone, Xanthan Gum, Sodium Sulfite, Methylparaben, Fragrance<br><br></div></div>",
        //        ProductTemplateId = productTemplateSingleVariant.Id,
        //        //SeName = "",
        //        AllowCustomerReviews = true,
        //        Published = true,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //        ShowOnHomePage = false,
        //    };
        //    allProducts.Add(productTone);
        //    productTone.ProductVariants.Add(new ProductVariant()
        //    {
        //        OldPrice = 25M,
        //        Price = 20M,
        //        IsShipEnabled = true,
        //        Weight = 5,
        //        Length = 19,
        //        Width = 35,
        //        Height = 31,
        //        ManageInventoryMethod = ManageInventoryMethod.ManageStock,
        //        StockQuantity = 10000,
        //        NotifyAdminForQuantityBelow = 1,
        //        AllowBackInStockSubscriptions = false,
        //        DisplayStockAvailability = true,
        //        LowStockActivity = LowStockActivity.DisableBuyButton,
        //        BackorderMode = BackorderMode.NoBackorders,
        //        OrderMinimumQuantity = 1,
        //        OrderMaximumQuantity = 10000,
        //        Published = true,
        //        DisplayOrder = 1,
        //        CreatedOnUtc = DateTime.UtcNow,
        //        UpdatedOnUtc = DateTime.UtcNow,
        //    });

        //    //categories
        //    productTone.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Beauty"),
        //        DisplayOrder = 1,
        //    });
        //    productTone.ProductCategories.Add(new ProductCategory()
        //    {
        //        Category =
        //            _categoryRepository.Table.FirstOrDefault(c => c.Name == "Sale"),
        //        DisplayOrder = 1,
        //    });

        //    //manufacturers
        //    productTone.ProductManufacturers.Add(new ProductManufacturer()
        //    {
        //        DisplayOrder = 0,
        //        Manufacturer = _manufacturerRepository.Table.FirstOrDefault(c => c.Name == "Kiddy Wear")
        //    });

        //    //pictures
        //    productTone.ProductPictures.Add(new ProductPicture()
        //    {
        //        Picture =
        //            _pictureService.InsertPicture(
        //                File.ReadAllBytes(sampleImagesPath +
        //                                "product-Tone-1.jpg"), "image/jpeg",
        //                _pictureService.GetPictureSeName(productTone.Name), true),
        //        DisplayOrder = 0,
        //    });

        //    _productRepository.Insert(productTone);


        //    //search engine names
        //    foreach (var product in allProducts)
        //    {
        //        _urlRecordRepository.Insert(new UrlRecord()
        //        {
        //            EntityId = product.Id,
        //            EntityName = "Product",
        //            LanguageId = 0,
        //            IsActive = true,
        //            Slug = product.ValidateSeName("", product.Name, true)
        //        });
        //    }


        //    //related products
        //    var relatedProducts = new List<RelatedProduct>()
        //                              {
        //                                  new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productDcShoes.Id,
        //                                          ProductId2 = productFittedWithDaisiesDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productDcShoes.Id,
        //                                          ProductId2 = productFootballBoots.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productDcShoes.Id,
        //                                          ProductId2 = productGlasses.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productDcShoes.Id,
        //                                          ProductId2 = productGreenTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productDcShoes.Id,
        //                                          ProductId2 = productHarvestEmbroideryDress.Id,
        //                                      },new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productDcShoes.Id,
        //                                          ProductId2 = productPanties.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productHarvestEmbroideryDress.Id,
        //                                          ProductId2 = productGreenTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productHarvestEmbroideryDress.Id,
        //                                          ProductId2 = productMascara.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productHarvestEmbroideryDress.Id,
        //                                          ProductId2 = productMenBrownShoes.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productHarvestEmbroideryDress.Id,
        //                                          ProductId2 = productMirror.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productHarvestEmbroideryDress.Id,
        //                                          ProductId2 = productPanties.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productHarvestEmbroideryDress.Id,
        //                                          ProductId2 = productPajamas.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productMulticolorTop.Id,
        //                                          ProductId2 = productMirror.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productMulticolorTop.Id,
        //                                          ProductId2 = productOrnamentDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productMulticolorTop.Id,
        //                                          ProductId2 = productPajamas.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productMulticolorTop.Id,
        //                                          ProductId2 = productPanties.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productMulticolorTop.Id,
        //                                          ProductId2 = productPinkJeans.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productMulticolorTop.Id,
        //                                          ProductId2 = productPowder.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productFittedWithDaisiesDress.Id,
        //                                          ProductId2 = productPanties.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productFittedWithDaisiesDress.Id,
        //                                          ProductId2 = productGlasses.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productFittedWithDaisiesDress.Id,
        //                                          ProductId2 = productWhiteTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productFittedWithDaisiesDress.Id,
        //                                          ProductId2 = productFootballBoots.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productFittedWithDaisiesDress.Id,
        //                                          ProductId2 = productRedJeans.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productFittedWithDaisiesDress.Id,
        //                                          ProductId2 = productShorts.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productBlueLineDress.Id,
        //                                          ProductId2 = productRedShoes.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productBlueLineDress.Id,
        //                                          ProductId2 = productCassiopeiaEmbroideredDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productBlueLineDress.Id,
        //                                          ProductId2 = productBlackPinkDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productBlueLineDress.Id,
        //                                          ProductId2 = productSnakeBag.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productBlueLineDress.Id,
        //                                          ProductId2 = productTone.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productBlueLineDress.Id,
        //                                          ProductId2 = productTriangleDress.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productPanties.Id,
        //                                          ProductId2 = productSnakeBag.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productPanties.Id,
        //                                          ProductId2 = productMulticolorTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productPanties.Id,
        //                                          ProductId2 = productHarvestEmbroideryDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productPanties.Id,
        //                                          ProductId2 = productDcShoes.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productPanties.Id,
        //                                          ProductId2 = productSocks.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productPanties.Id,
        //                                          ProductId2 = productRedSleepwear.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productWhiteTop.Id,
        //                                          ProductId2 = productDcShoes.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productWhiteTop.Id,
        //                                          ProductId2 = productTriangleDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productWhiteTop.Id,
        //                                          ProductId2 = productSocks.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productWhiteTop.Id,
        //                                          ProductId2 = productBandoDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productWhiteTop.Id,
        //                                          ProductId2 = productSnakeBag.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productWhiteTop.Id,
        //                                          ProductId2 = productTone.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productGreenTop.Id,
        //                                          ProductId2 = productBandoDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productGreenTop.Id,
        //                                          ProductId2 = productMascara.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productGreenTop.Id,
        //                                          ProductId2 = productPajamas.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productGreenTop.Id,
        //                                          ProductId2 = productRedShoes.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productGreenTop.Id,
        //                                          ProductId2 = productWhiteTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productGreenTop.Id,
        //                                          ProductId2 = productTone.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productOrnamentDress.Id,
        //                                          ProductId2 = productPanties.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productOrnamentDress.Id,
        //                                          ProductId2 = productPinkShoes.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productOrnamentDress.Id,
        //                                          ProductId2 = productSocks.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productOrnamentDress.Id,
        //                                          ProductId2 = productWhiteTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productOrnamentDress.Id,
        //                                          ProductId2 = productSnakeBag.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productOrnamentDress.Id,
        //                                          ProductId2 = productPowder.Id,
        //                                      },

        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productRedShoes.Id,
        //                                          ProductId2 = productWhiteTop.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productRedShoes.Id,
        //                                          ProductId2 = productOrnamentDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productRedShoes.Id,
        //                                          ProductId2 = productHarvestEmbroideryDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productRedShoes.Id,
        //                                          ProductId2 = productCassiopeiaEmbroideredDress.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productRedShoes.Id,
        //                                          ProductId2 = productPowder.Id,
        //                                      },
        //                                      new RelatedProduct()
        //                                      {
        //                                          ProductId1 = productRedShoes.Id,
        //                                          ProductId2 = productPinkShoes.Id,
        //                                      },
        //                                      };
        //    relatedProducts.ForEach(rp => _relatedProductRepository.Insert(rp));

        //    //product tags
        //    AddProductTag(productBandoDress, "awesome");
        //    AddProductTag(productBlackPinkDress, "awesome");
        //    AddProductTag(productBlueJeans, "awesome");
        //    AddProductTag(productBlueLineDress, "awesome");
        //    AddProductTag(productBra, "awesome");
        //    AddProductTag(productBrownBag, "awesome");
        //    AddProductTag(productCassiopeiaEmbroideredDress, "awesome");
        //    AddProductTag(productChildShoes, "awesome");
        //    AddProductTag(productDcShoes, "awesome");
        //    AddProductTag(productFittedWithDaisiesDress, "awesome");
        //    AddProductTag(productFootballBoots, "summer");
        //    AddProductTag(productGlasses, "summer");
        //    AddProductTag(productGreenTop, "summer");
        //    AddProductTag(productHarvestEmbroideryDress, "summer");
        //    AddProductTag(productMascara, "summer");
        //    AddProductTag(productMenBrownShoes, "summer");
        //    AddProductTag(productMirror, "child");
        //    AddProductTag(productMulticolorTop, "child");
        //    AddProductTag(productOrnamentDress, "child");
        //    AddProductTag(productPajamas, "child");
        //    AddProductTag(productPanties, "child");
        //    AddProductTag(productPinkJeans, "cotton");
        //    AddProductTag(productPinkShoes, "cotton");
        //    AddProductTag(productPowder, "cotton");
        //    AddProductTag(productRedJeans, "cotton");
        //    AddProductTag(productRedShoes, "cotton");
        //    AddProductTag(productRedSleepwear, "cotton");
        //    AddProductTag(productShorts, "whole");
        //    AddProductTag(productSnakeBag, "whole");
        //    AddProductTag(productSocks, "whole");
        //    AddProductTag(productTone, "man");
        //    AddProductTag(productMenBrownShoes, "man");
        //    AddProductTag(productBlueJeans, "man");
        //    AddProductTag(productTriangleDress, "awesome");
        //    AddProductTag(productWhiteTop, "awesome");
        //    AddProductTag(productDcShoes, "girl");
        //    AddProductTag(productFittedWithDaisiesDress, "girl");
        //    AddProductTag(productFootballBoots, "girl");
        //    AddProductTag(productGlasses, "girl");
        //    AddProductTag(productGreenTop, "girl");
        //    AddProductTag(productHarvestEmbroideryDress, "girl");
        //    AddProductTag(productMascara, "girl");
        //    AddProductTag(productMenBrownShoes, "girl");
        //    AddProductTag(productMirror, "girl");
        //    AddProductTag(productMulticolorTop, "girl");
        //    AddProductTag(productOrnamentDress, "girl");
        //    AddProductTag(productShorts, "funny");
        //    AddProductTag(productSnakeBag, "funny");
        //    AddProductTag(productSocks, "funny");
        //    AddProductTag(productTone, "funny");
        //    AddProductTag(productMenBrownShoes, "funny");
        //    AddProductTag(productBlueJeans, "funny");
        //    AddProductTag(productPinkShoes, "beautiful");
        //    AddProductTag(productPowder, "beautiful");
        //    AddProductTag(productRedJeans, "beautiful");
        //    AddProductTag(productRedShoes, "beautiful");
        //    AddProductTag(productRedSleepwear, "beautiful");
        //    AddProductTag(productShorts, "beautiful");
        //    AddProductTag(productSnakeBag, "beautiful");
        //    AddProductTag(productSocks, "beautiful");
        //    AddProductTag(productTone, "beautiful");
        //    AddProductTag(productMenBrownShoes, "beautiful");
        //    AddProductTag(productBlueJeans, "beautiful");
        //    AddProductTag(productTriangleDress, "beautiful");
        //    AddProductTag(productPinkJeans, "cocktail");
        //    AddProductTag(productPinkShoes, "cocktail");
        //    AddProductTag(productPowder, "cocktail");
        //    AddProductTag(productRedJeans, "cocktail");
        //    AddProductTag(productRedShoes, "cocktail");
        //    AddProductTag(productRedSleepwear, "cocktail");
        //    AddProductTag(productShorts, "cocktail");
        //    AddProductTag(productSnakeBag, "cocktail");
        //    AddProductTag(productSocks, "cocktail");
        //    AddProductTag(productTone, "cocktail");
        //    AddProductTag(productMenBrownShoes, "cocktail");
        //    AddProductTag(productFootballBoots, "girl");
        //    AddProductTag(productGlasses, "party");
        //    AddProductTag(productGreenTop, "party");
        //    AddProductTag(productHarvestEmbroideryDress, "party");
        //    AddProductTag(productMascara, "party");
        //    AddProductTag(productMenBrownShoes, "party");
        //    AddProductTag(productMirror, "party");
        //    AddProductTag(productMulticolorTop, "party");
        //    AddProductTag(productOrnamentDress, "party");
        //}

        public void InstallSampleData()
        {
            InstallBlogPosts();
            InstallNews();
            InstallSpecificationAttributes();
            InstallProductAttributes();
            InstallManufacturers();
            InstallCategories();
            //InstallProducts();
        }

        public void Preconfigure()
        {
            //media settings
            _mediaSettings.ProductThumbPictureSize = 330;
            _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage = 180;
            _mediaSettings.ProductDetailsPictureSize = 600;
            _settingService.SaveSetting(_mediaSettings);

            //language settings
            _localizationSettings.UseImagesForLanguageSelection = true;
            _settingService.SaveSetting(_localizationSettings);

            //catalog settings
            _catalogSettings.ProductsByTagPageSizeOptions = "6, 3, 12, 24";
            _catalogSettings.ManufacturersBlockItemsToDisplay = 15;
            _catalogSettings.ProductsAlsoPurchasedNumber = 6;
            _settingService.SaveSetting(_catalogSettings);

            //disable Nivo Slider widget
            var nivoSliderWidget = _widgetService.LoadWidgetBySystemName("Widgets.NivoSlider");
            if (nivoSliderWidget != null && nivoSliderWidget.IsWidgetActive(_widgetSettings))
            {
                //mark as disabled
                _widgetSettings.ActiveWidgetSystemNames.Remove(nivoSliderWidget.PluginDescriptor.SystemName);
                _settingService.SaveSetting(_widgetSettings);
            }
        }

        #endregion
    }
}
