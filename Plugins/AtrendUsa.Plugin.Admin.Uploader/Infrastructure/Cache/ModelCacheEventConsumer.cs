using Nop.Core.Caching;
using Nop.Core.Infrastructure;

namespace AtrendUsa.Plugin.Admin.Uploader.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer
    {
        /// <summary>
        /// Key for caching
        /// </summary>
        public const string UPLOADER_PATTERN_KEY = "AtrendUsa.Plugin.Admin.Uploader";

        private readonly ICacheManager _cacheManager;

        public ModelCacheEventConsumer()
        {
            //TODO inject static cache manager using constructor
            this._cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
        }
    }
}
