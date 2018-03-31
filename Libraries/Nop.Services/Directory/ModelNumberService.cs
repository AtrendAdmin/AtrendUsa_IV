using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Directory;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Directory
{
    /// <summary>
    /// Model Number service    //Added by IV Santosh 
    /// </summary>
    public partial class ModelNumberService : IModelNumberService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        private const string COUNTRIES_ALL_KEY = "Nop.ModelNumber.all-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string COUNTRIES_PATTERN_KEY = "Nop.ModelNumber.";

        #endregion

        #region Fields
        private readonly IRepository<ModelNumber> _modelnumberRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="countryRepository">Country repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        public ModelNumberService(ICacheManager cacheManager,
            IRepository<ModelNumber> modelnumberRepository)
        {
            this._cacheManager = cacheManager;
            this._modelnumberRepository = modelnumberRepository;
        }

        #endregion

        /// <summary>
        /// Gets all model numbers
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<ModelNumber> GetAllModelNumbers(int languageId = 0, bool showHidden = false)
        {
            string key = string.Format(COUNTRIES_ALL_KEY, languageId, showHidden);
            var query = _modelnumberRepository.Table;
            
            var ModelNumbers = query.ToList();

            ModelNumbers = ModelNumbers
                .OrderBy(c => c.ModelNum)
                .ToList();
            return ModelNumbers;
        }
    }
}
