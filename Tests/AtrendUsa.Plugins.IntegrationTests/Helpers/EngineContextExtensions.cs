//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using Nop.Core.Configuration;
//using Nop.Core.Infrastructure;
//using Nop.Core.Infrastructure.DependencyManagement;

//namespace AtrendUsa.Plugins.IntegrationTests.Helpers
//{
//   public static class EngineContextExtensions
//    {
//       public static void TestInitalize(this EngineContext engineContext)
//       {
//           var typeFinder = new AppDomainTypeFinder();
//           var builder = new ContainerBuilder();
//           new DependencyRegistrar().Register(builder, typeFinder);
//           var container = builder.Build();
//           EngineContext.Initialize(true);
//           //builder.Update(container);
//           engineContext.

//           _containerManager = new ContainerManager(container);
//       }

//       private static IEngine CreateEngineInstance(NopConfig config)
//       {
//           if (config != null && !string.IsNullOrEmpty(config.EngineType))
//           {
//               var engineType = Type.GetType(config.EngineType);
//               if (engineType == null)
//                   throw new ConfigurationErrorsException("The type '" + config.EngineType + "' could not be found. Please check the configuration at /configuration/nop/engine[@engineType] or check for missing assemblies.");
//               if (!typeof(IEngine).IsAssignableFrom(engineType))
//                   throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Nop.Core.Infrastructure.IEngine' and cannot be configured in /configuration/nop/engine[@engineType] for that purpose.");
//               return Activator.CreateInstance(engineType) as IEngine;
//           }

//           return new NopEngine();
//       }

//       public void Initialize(NopConfig config)
//       {
//           //register dependencies
//           RegisterDependencies(config);

//           //startup tasks
//           if (!config.IgnoreStartupTasks)
//           {
//               RunStartupTasks();
//           }

//       }
//    }
//}

//        ///// <summary>
//        ///// Initializes a static instance of the Nop factory.
//        ///// </summary>
//        ///// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
//        //[MethodImpl(MethodImplOptions.Synchronized)]
//        //public static IEngine Initialize(bool forceRecreate)
//        //{
//        //    if (Singleton<IEngine>.Instance == null || forceRecreate)
//        //    {
//        //        var config = ConfigurationManager.GetSection("NopConfig") as NopConfig;
//        //        Singleton<IEngine>.Instance = CreateEngineInstance(config);
//        //        Singleton<IEngine>.Instance.Initialize(config);
//        //    }
//        //    return Singleton<IEngine>.Instance;
//        //}

