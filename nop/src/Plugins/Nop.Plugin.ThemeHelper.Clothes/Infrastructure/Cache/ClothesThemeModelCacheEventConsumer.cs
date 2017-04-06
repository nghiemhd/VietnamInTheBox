using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Events;
using Nop.Core.Infrastructure;
using Nop.Services.Events;

namespace Nop.Plugin.ThemeHelper.Clothes.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching presentation layer models)
    /// </summary>
    public partial class ClothesThemeModelCacheEventConsumer :
        IConsumer<EntityInserted<Language>>,
        IConsumer<EntityUpdated<Language>>,
        IConsumer<EntityDeleted<Language>>,
        IConsumer<EntityInserted<Setting>>,
        IConsumer<EntityUpdated<Setting>>,
        IConsumer<EntityDeleted<Setting>>,
        IConsumer<EntityInserted<Category>>,
        IConsumer<EntityUpdated<Category>>,
        IConsumer<EntityDeleted<Category>>,
        IConsumer<EntityInserted<Manufacturer>>,
        IConsumer<EntityUpdated<Manufacturer>>,
        IConsumer<EntityDeleted<Manufacturer>>,
        IConsumer<EntityUpdated<CustomerRole>>,
        IConsumer<EntityDeleted<CustomerRole>>
    {
        /// <summary>
        /// Key for ManufacturerNavigationModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string MANUFACTURER_HOMEPAGE_NAVIGATION_MODEL_KEY = "Nop.pres.themes.clothes.manufacturer.homepagenavigation-{0}-{1}-{2}";
        public const string MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY = "Nop.pres.themes.clothes.manufacturer.homepagenavigation";

        /// <summary>
        /// Key for CategoryNavigationModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : comma separated list of customer roles
        /// {2} : current store ID
        /// </remarks>
        public const string CATEGORY_HOMEPAGE_NAVIGATION_MODEL_KEY = "Nop.pres.themes.clothes.category.homepagenavigation-{0}-{1}-{2}";
        public const string CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY = "Nop.pres.themes.clothes.category.homepagenavigation";

        private readonly ICacheManager _cacheManager;

        public ClothesThemeModelCacheEventConsumer()
        {
            //TODO inject static cache manager using constructor
            this._cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
        }

        public void HandleEvent(EntityInserted<Language> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<Language> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<Language> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityInserted<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityInserted<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<CustomerRole> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<CustomerRole> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityInserted<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_HOMEPAGE_NAVIGATION_PATTERN_KEY);
        }
    }
}
