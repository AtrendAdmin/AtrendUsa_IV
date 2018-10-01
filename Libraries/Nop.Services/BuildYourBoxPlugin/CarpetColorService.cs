using Nop.Core.Data;
using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.BuildYourBoxPlugin
{
    public partial class CarpetColorService : ICarpetColorService
    {
        public readonly IRepository<CarpetColors> _carpetColorRepository;

        public CarpetColorService(IRepository<CarpetColors> CarpetColorRepository)
        {
            _carpetColorRepository = CarpetColorRepository;
        }

        public virtual IList<CarpetColors> SelectAllCarpetColor()
        {
            var carpetColors = _carpetColorRepository.Table.ToList();

            return carpetColors;
        }
    }
}
