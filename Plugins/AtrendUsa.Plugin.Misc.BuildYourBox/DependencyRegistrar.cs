using Nop.Core.Infrastructure.DependencyManagement;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;

namespace AtrendUsa.Plugin.Misc.BuildYourBox
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //builder.RegisterType<SupportMessageService>().As<ISupportMessageService>().InstancePerLifetimeScope();
        }
    }
}
