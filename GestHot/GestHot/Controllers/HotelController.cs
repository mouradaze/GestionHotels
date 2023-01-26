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
using Newtonsoft.Json.Linq;
//[DataType(DataType.DateTime)] [DataType(DataType.Password)]
namespace GestHot.Controllers
{
    public class HotelController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();
        private int api(string comment)
        {
            var url = "https://localhost:62950/predict";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.ContentType = "application/json";

            var data = "{\"new_reviews\": \"" + comment + "\",\"score\": 0}";
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            JObject json;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                json = JObject.Parse(result);

            }
            return Convert.ToInt32(json["prediction"]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addCmt(string cmt, int idH)
        {
            if (Session["userId"] != null)
            {
                Comment cmtt = new Comment(int.Parse(Session["userId"].ToString()),idH,cmt);
                cmtt.prediction = api(cmt);
                db.Comments.Add(cmtt);
                db.SaveChanges();
                return RedirectToAction("Details", "Hotel", new {id = idH});
            }
            else
            {
                return RedirectToAction("Details", "Hotel", new { id = idH });
            }
        }

        

        // GET: Hotel
        public ActionResult Index()
        {
            new OwnerController().hotelNote();
            
            return View(db.Hotels.ToList());
        }

        // GET: Hotel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            var cmts = db.Comments.Where(x => x.idH == id);
            //List<Comment> cm = (List<Comment>)cmts;
            //if(cmts.Count != 0)
            TempData["cmtNb"] = cmts.Count();
            TempData["comments"] = cmts;
            TempData["hots"] = db.Hotels.ToList().Take(4);
            return View(hotel);
        }

        // GET: Hotel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotel/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idH,name,adresse,description,nbCH,note")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                
                hotel.idU = int.Parse((string)Session["userID"]);
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        // GET: Hotel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idH,name,adresse,description,nbCH,note")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost, ActionName("reservation")]
        [ValidateAntiForgeryToken]
        public ActionResult reservation(DateTime dateS, DateTime dateE, int nbrP, int nbrR, int idU, int idH) 
        {
            Reservation res = new Reservation(idU,idH,dateS, dateE);
            db.Reservations.Add(res);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Fav(int idU, int idH)
        {
            Favorite fav = new Favorite(idU, idH);
            var pQuery = db.Favorites.FirstOrDefault(e => e.idU.Equals(idU) && e.idH.Equals(idH));
            if(pQuery != null)
            {
                TempData["fav"] = "This hotel is already in your favorites";
                return RedirectToAction("Index");
            }
            db.Favorites.Add(fav);
            db.SaveChanges();
            TempData["fav"] = "Hotel added to your favorites";
            return RedirectToAction("Index");
        }
    }
}
