using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using CarGo.Global.Auth;
using CarGo.Mappers;
using CarGo.Model;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using CarGo.Global.Config;
using CarGo.Model.Helpers;

[assembly: WebActivator.PreApplicationStartMethod(typeof(CarGo.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(CarGo.App_Start.NinjectWebCommon), "Stop")]
namespace CarGo.App_Start
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            CommonDataHelper.SetGlobalInstance(new CarGoDbDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
            kernel.Bind<CarGoDbDataContext>().ToMethod(c => new CarGoDbDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
            kernel.Bind<IRepository>().To<SqlRepository>().InRequestScope();
            kernel.Bind<IMapper>().To<CommonMapper>().InSingletonScope();
            kernel.Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
            kernel.Bind<IConfig>().To<Config>().InSingletonScope();
        }        
    }
}
