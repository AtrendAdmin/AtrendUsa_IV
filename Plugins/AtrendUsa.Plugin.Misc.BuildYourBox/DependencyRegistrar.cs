using Nop.Core.Infrastructure.DependencyManagement;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using AtrendUsa.Plugin.Misc.BuildYourBox.Services;
using Nop.Services.BuildYourBoxPlugin;

namespace AtrendUsa.Plugin.Misc.BuildYourBox
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<BuildYourBoxMessageService>().As<IBuildYourBoxMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<BuildYourBoxService>().As<IBuildYourBoxService>().InstancePerLifetimeScope();
            builder.RegisterType<CarpetColorService>().As<ICarpetColorService>().InstancePerLifetimeScope();
            builder.RegisterType<TerminalTypeService>().As<ITerminalTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MDFThicknessService>().As<IMDFThicknessService>().InstancePerLifetimeScope();
        }
    }
}
