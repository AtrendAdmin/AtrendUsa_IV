using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Data;
using Nop.Core.Domain.BuildYourBoxPlugin;
using Nop.Services.Events;

namespace Nop.Services.BuildYourBoxPlugin
{
    public partial class BuildYourBoxService : IBuildYourBoxService
    {
        private readonly IRepository<BuildYourBox> _buildYourBoxRepository;
        private readonly IEventPublisher _eventPublisher;

        public BuildYourBoxService(
            IRepository<BuildYourBox> buildYourBoxRepository,
            IEventPublisher eventPublisher)
        {
            _buildYourBoxRepository = buildYourBoxRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Inserts BuildYourBox form data
        /// </summary>
        /// <param name="buildYourBox">BuildYourBox item</param>
        public virtual void InsertBuildYourBox(BuildYourBox buildYourBox)
        {
            if (buildYourBox == null)
                throw new ArgumentNullException("buildYourBox");

            _buildYourBoxRepository.Insert(buildYourBox);

            _eventPublisher.EntityInserted(buildYourBox);
        }
    }
}
