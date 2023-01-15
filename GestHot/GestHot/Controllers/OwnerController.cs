using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestHot.Models;

namespace GestHot.Controllers
{
    public class OwnerController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();

        // GET: Owner
        public ActionResult Index(int?id)
        {
            
            return View(db.Hotels.Where(e => e.idU== id));
        }

        

        // GET: Owner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Owner/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idU,Name,LastName,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["userId"].ToString() });
            }

            return View(user);
        }

        


        //See Comments
        public ActionResult Comments(int? id)
        {
            var hotel = db.Hotels.Find(id);
            if(hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel.Comments);
        }

        //Edit hotel
        //Get
        public ActionResult Edit(int? id)
        {
            var hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idH,name,adresse,description,nbCH,note,idU")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                hotel.idU = int.Parse((string)Session["userID"]);
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["userId"].ToString() });
            }
            return View(hotel);
        }

        //new Hotel

        //Get 
        public ActionResult newHotel()
        {
            
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult newHotel([Bind(Include = "name,adresse,description,nbCH,idU")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                hotel.idU = int.Parse((string)Session["userID"]);
                hotel.note = 0;
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["userId"].ToString() });
            }
            return HttpNotFound();
        }

        //See details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel= db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
            
        }
        public ActionResult Delete(int? id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
            db.SaveChanges();
            return RedirectToAction("Index", new {id = Session["userId"].ToString() });
        }
    }
}
