using CarGo.Model;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using CarGo.Model.Helpers;

namespace CarGo.Areas.Default.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("index");
        }

        [HttpPost]
        public JsonResult Region(string searchquery)
        {

            return Json(from i in CommonDataHelper.GlobalInstance.SearchRegions(searchquery, 5) select new { k = i.Id, v = i.Region });
        }

        [HttpPost]
        public JsonResult City(int regionId, string searchquery)
        {
            return Json(from i in CommonDataHelper.GlobalInstance.SearchCities(regionId, searchquery, 5) select new { k = i.Id, v = string.Format("{0} {1}.", i.City, i.CityType) });
        }

        [HttpPost]
        public JsonResult Street(int cityId, string searchquery)
        {
            return Json(from i in CommonDataHelper.GlobalInstance.SearchStreets(cityId, searchquery, 5) select new { k = i.Id, v = string.Format("{0} {1}.", i.Street, i.StreetType) });
        }
    }
}