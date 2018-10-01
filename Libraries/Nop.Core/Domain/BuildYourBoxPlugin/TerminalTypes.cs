using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.BuildYourBoxPlugin
{
    public partial class TerminalTypes : BaseEntity
    {
        /// <summary>
        /// Gets or sets TerminalType property
        /// </summary>
        public string TerminalType { get; set; }
        
        /// <summary>
        /// Gets or sets ImageURL property
        /// </summary>
        public string ImageURL { get; set; }
    }
}
