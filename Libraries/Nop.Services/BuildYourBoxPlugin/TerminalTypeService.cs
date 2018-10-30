using Nop.Core.Data;
using Nop.Core.Domain.BuildYourBoxPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.BuildYourBoxPlugin
{
    public partial class TerminalTypeService : ITerminalTypeService
    {
        public readonly IRepository<TerminalTypes> _terminalTypeRepository;

        public TerminalTypeService(IRepository<TerminalTypes> TerminalTypeRepository)
        {
            _terminalTypeRepository = TerminalTypeRepository;
        }

        public virtual IList<TerminalTypes> SelectAllTerminalType()
        {
            var termnalTypes = _terminalTypeRepository.Table.ToList();

            return termnalTypes;
        }
    }
}
