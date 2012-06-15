using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.ImportGen;

namespace QRTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            //ImportGen.ImportGenetator gen = new ImportGenetator();
            //gen.ImportGen();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
