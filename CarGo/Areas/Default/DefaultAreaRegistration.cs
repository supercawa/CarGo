using System.Web.Mvc;

namespace CarGo.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                url: "Error",
                defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CarGo.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                url: "NotFoundPage",
                defaults: new { controller = "Error", action = "NotFoundPage", id = UrlParameter.Optional },
                namespaces: new[] { "CarGo.Areas.Default.Controllers" }
            );

            context.MapRoute(
                name : "default",
                url : "{controller}/{action}/{id}",
                defaults : new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces : new [] { "CarGo.Areas.Default.Controllers" }
            );
        }
    }
}
