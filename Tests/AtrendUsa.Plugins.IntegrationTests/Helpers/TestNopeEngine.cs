using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;

namespace AtrendUsa.Plugins.IntegrationTests.Helpers
{
    public class TestNopeEngine : IEngine  
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Utilities
        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterDependencies(NopConfig config)
        {
            var typeFinder = new AppDomainTypeFinder();
            var builder = new ContainerBuilder();
            new DependencyRegistrar().Register(builder, typeFinder);
            var container = builder.Build();

            _containerManager = new ContainerManager(container);

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        #endregion
        #region Implementation of IEngine

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }
        public void Initialize(NopConfig config)
        {
            //register dependencies
            RegisterDependencies(config);
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion
    }
}
