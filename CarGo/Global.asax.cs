using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CarGo.Areas.Admin;
using CarGo.Areas.Default;
using System.Threading;
using CarGo.Model;

namespace CarGo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private Thread mailThread { get; set; }

        protected void Application_Start()
        {
            var adminArea = new AdminAreaRegistration();
            var adminAreaContext = new AreaRegistrationContext(adminArea.AreaName, RouteTable.Routes);
            adminArea.RegisterArea(adminAreaContext);

            var defaultArea = new DefaultAreaRegistration();
            var defaultAreaContext = new AreaRegistrationContext(defaultArea.AreaName, RouteTable.Routes);
            defaultArea.RegisterArea(defaultAreaContext);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            mailThread = new Thread(new ThreadStart(ThreadFunc));
            mailThread.Start();
        }

        private static void ThreadFunc()
        {
            while (true)
            {
                try
                {
                    var mailThread = new Thread(new ThreadStart(MailThread));
                    mailThread.Start();
                    logger.Info("Wait for end mail thread");
                    mailThread.Join();
                    logger.Info("Sleep 60 seconds");
                }
                catch (Exception ex)
                {
                    logger.ErrorException("Thread period error", ex);
                }
                Thread.Sleep(60000);
            }
        }

        private static void MailThread()
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            /*while (MailProcessor.SendNextMail(repository)) { };*/
        }
    }
}