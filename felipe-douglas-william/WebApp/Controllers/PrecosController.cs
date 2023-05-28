using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class PrecosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Preços do produto";

            string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
            ViewBag.ApiUrl = apiUrl;

            return View();
        }
    }
}