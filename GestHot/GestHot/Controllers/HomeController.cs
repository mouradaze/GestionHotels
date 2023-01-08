using GestHot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestHot.Controllers
{
    public class HomeController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();
        public ActionResult Index()
        {
            
            return View(db.Hotels.ToList());
        }

        public ActionResult AllHotels()
        {
            return View();
        }

        public ActionResult hotel()
        {
            return View();
        }

    

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}