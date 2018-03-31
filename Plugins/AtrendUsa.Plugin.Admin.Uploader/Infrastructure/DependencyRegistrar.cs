using AtrendUsa.Plugin.Admin.Uploader.Controllers;
using AtrendUsa.Plugin.Admin.Uploader.Services.Implementation;
using AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces;
using Autofac;
using Autofac.Core;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Services.Logging;

namespace AtrendUsa.Plugin.Admin.Uploader.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //AtrendUsa.Plugins.Admin.Uploader
            builder.RegisterType<ProductUploader>().As<IProductUploader>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static")).InstancePerLifetimeScope();
            builder.RegisterType<ProductValidator>().As<IProductValidator>().InstancePerLifetimeScope(); ;
            builder.RegisterType<ProductResolver>().As<IProductResolver>().InstancePerLifetimeScope();
        }

        public int Order { get { return 0; } }
    }
}
