using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.BuildYourBoxPlugin
{
    public partial interface IMDFThicknessService
    {
        IList<MDFThicknesses> SelectAllMDFThicknesses();
    }
}
