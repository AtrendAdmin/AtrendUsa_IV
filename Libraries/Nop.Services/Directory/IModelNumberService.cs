using Nop.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Directory
{
    public partial interface IModelNumberService
    {
        /// <summary>
        /// Gets all ModelNumbers    //Added by IV Santosh 
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        IList<ModelNumber> GetAllModelNumbers(int languageId = 0, bool showHidden = false);
    }
}
