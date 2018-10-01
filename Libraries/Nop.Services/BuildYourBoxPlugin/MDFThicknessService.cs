using Nop.Core.Data;
using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.BuildYourBoxPlugin
{
    public partial class MDFThicknessService : IMDFThicknessService
    {
        public readonly IRepository<MDFThicknesses> _mDFThicknessesRepository;

        public MDFThicknessService(IRepository<MDFThicknesses> MDFThicknessesRepository)
        {
            _mDFThicknessesRepository = MDFThicknessesRepository;
        }

        public virtual IList<MDFThicknesses> SelectAllMDFThicknesses()
        {
            var MDFThicknesseses = _mDFThicknessesRepository.Table.ToList();

            return MDFThicknesseses;
        }
    }
}
