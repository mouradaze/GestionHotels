using GestHot.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace GestHot.Controllers
{
    public class HomeController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();



        
        
        

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult search(string search)
        {
            var pQuery = db.Hotels.Where(e => e.name.Contains(search));
            return View(pQuery);
        }
        public ActionResult Index()
        {
            
            return View(db.Hotels.ToList().Take(4));
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