using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestHot.Models;
using Microsoft.Ajax.Utilities;

namespace GestHot.Controllers
{
    public class OwnerController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();

        // GET: Owner
        public ActionResult Index(int?id)
        {
            //return View(hotelNote(id));
            hotelNote();
            return View(db.Hotels.Where(e => e.idU== id));
        }

        public void hotelNote()
        {
            db.Hotels.ForEach(hot => {
                int good = hot.Comments.Where(e => e.prediction == 1).Count();
                int neutral = hot.Comments.Where(e => e.prediction == 0).Count();
                int bad = hot.Comments.Where(e => e.prediction == -1).Count();
                int all = good + bad + neutral;
                int res;
                if (all == 0) { res = 0; }
                else
                {
                    res = (good * 100) / all;
                }
                if (res == 0)
                {
                    hot.note = 0;
                }
                if (res == 100)
                {
                    hot.note = 5;
                }
                if (res > 0 && res < 25)
                {
                    hot.note = 1;
                }
                else if (res >= 25 && res < 50)
                {
                    hot.note = 2;
                }
                else if (res >= 50 && res < 75)
                {
                    hot.note = 3;
                }
                else if (res >= 75 && res < 100)
                {
                    hot.note = 4;
                }
                
            });db.SaveChanges();
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
        public ActionResult Edit([Bind(Include = "idH,name,adresse,description,nbCH,note,prix,idU")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                hotel.idU = int.Parse((string)Session["userID"]);
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                TempData["hotel"] = "Hotel edited successfully";
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

        public ActionResult newHotel([Bind(Include = "name,adresse,description,nbCH,idU,fileH,prix")] Hotel hotel, HttpPostedFileBase fileH)
        {
            if (ModelState.IsValid)
            {
                hotel.idU = int.Parse((string)Session["userID"]);
                hotel.note = 0;
                if (db.Hotels.FirstOrDefault(e => e.name.Equals(hotel.name) && e.adresse.Equals(hotel.adresse)) != null)
                {
                    TempData["hotel"] = "This hotel already exists";
                    return RedirectToAction("Index", new { id = Session["userId"].ToString() });
                }
                else
                {

                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileH.FileName));
                    fileH.SaveAs(path);
                    hotel.fileH = "/UploadedFiles/" + fileH.FileName;
                    db.Hotels.Add(hotel);
                    db.SaveChanges();
                    TempData["hotel"] = "Hotel created successfully";
                    return RedirectToAction("Index", new { id = Session["userId"].ToString() });
                }
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
            var cmt = db.Comments.Where(e => e.idH == id);
            var favs = db.Favorites.Where(e => e.idH == id);
            var rese = db.Reservations.Where(e => e.idH == id);
            foreach( var cm in cmt)
            {
                db.Comments.Remove(cm);
            }
            foreach (var fav in favs)
            {
                db.Favorites.Remove(fav);
            }
            foreach (var fav in rese)
            {
                db.Reservations.Remove(fav);
            }
            db.Hotels.Remove(hotel);

            db.SaveChanges();
            return RedirectToAction("Index", new {id = Session["userId"].ToString() });
        }
    }
}
