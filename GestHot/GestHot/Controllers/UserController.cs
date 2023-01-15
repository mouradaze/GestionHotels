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
    public class UserController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();


        

        public ActionResult owner()
        {
            return View();
        }
        // GET: User
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Reservation(int? id) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user.Reservations);
        }
        public ActionResult Favorites(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //var hot = user.Favorites.Select(e => e.idH);
            //var hot = db.Hotels.Include(e => e.idH);
            //List<Hotel> pQuery = (from h in db.Hotels join ho in user.Favorites on h.idH equals ho.idH select h).ToList();
            return View(user.Favorites);
        }
        public ActionResult Information(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult normal(string name, string last, string mail, string pass)
        {
            User user = new Models.User(name,last,mail,pass,-1);
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(string mail, string pass)
        {
            var pQuery = db.Users.FirstOrDefault(e => e.Email.Equals(mail) && e.Password.Equals(pass));
            var x = db.Users.FirstOrDefault(e => e.Email.Equals(mail) && e.Password.Equals(pass))?.idU;
            if(pQuery != null)
            {
                User usr = pQuery as Models.User;
                if(pQuery.Role == 0)
                {
                    Session["userId"] = x.ToString();
                    return RedirectToAction("Index", "Owner", new {id = x});
                }
                else if(pQuery.Role == -1)
                {
                    Session["userId"] = x.ToString();
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    Session["userId"] = x.ToString();
                    return RedirectToAction("Index", "Admin");
                }
                
            }
            else
                return RedirectToAction("Index", "Home");

        }
        public ActionResult Logout()
        {
            Session["userId"] = null;
            return RedirectToAction("Index", "Home");
        }


        // POST: User/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult owner([Bind(Include = "Name,LastName,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = 0;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idU,Name,LastName,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
    }
}
