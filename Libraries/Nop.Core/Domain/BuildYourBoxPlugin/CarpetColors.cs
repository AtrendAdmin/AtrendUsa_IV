using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.BuildYourBoxPlugin
{
    public partial class CarpetColors : BaseEntity
    {
        /// <summary>
        /// Gets or sets CarpetColor property
        /// </summary>
        public string CarpetColor { get; set; } 

        public string ImageURL { get; set; }
    }
}
