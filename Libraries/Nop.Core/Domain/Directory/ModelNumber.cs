using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Directory
{
    /// <summary>
    /// Represents a ModelNumber     //Added by IV Santosh 
    /// </summary>
    public partial class ModelNumber : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string ModelNum { get; set; }
        public int StoreId { get; set; }
    }
}
