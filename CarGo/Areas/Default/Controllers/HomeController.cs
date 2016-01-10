using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarGo.Model;
using Ninject;

namespace CarGo.Areas.Default.Controllers
{
    public class HomeController : DefaultController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            return View(CurrentUser);
        }
    }
}
