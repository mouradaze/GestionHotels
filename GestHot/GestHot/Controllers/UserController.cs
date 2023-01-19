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
using static System.Net.WebRequestMethods;

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

        public ActionResult Favorites()
        {
            int id = int.Parse(Session["userId"].ToString());
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ICollection<Hotel> fav = new List<Hotel>();
            foreach(var it in user.Favorites)
            {
                fav.Add(db.Hotels.First(e => e.idH == it.idH));
            }
            return View(fav);
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
        public ActionResult normal(string name, string last, string mail, string pass, HttpPostedFileBase fileU)
        {
            User user = new Models.User(name, last, mail, pass, -1);
            if (db.BlackLists.FirstOrDefault(e => e.emailB.Equals(mail)) != null)
            {
                TempData["register"] = "This e-mail is blacklisted";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (db.Users.FirstOrDefault(e => e.Email.Equals(mail)) != null)
                {
                    TempData["register"] = "This user already exists";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (fileU != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileU.FileName));
                        fileU.SaveAs(path);
                        user.fileU = "/UploadedFiles/" + fileU.FileName;

                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                    TempData["register"] = "Registered successfully";
                    return RedirectToAction("Index", "Home");
                }
            }
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
                    Session["own"] = "yes";
                    return RedirectToAction("Index", "Owner", new {id = x});
                }
                else if(pQuery.Role == -1)
                {
                    Session["userId"] = x.ToString();
                    TempData["register"] = "Logged In";
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
            TempData["register"] = "Logged out";
            Session["own"] = null;
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
                if (db.BlackLists.FirstOrDefault(e => e.emailB.Equals(user.Email)) != null)
                {
                    TempData["register"] = "This e-mail is blacklisted";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (db.Users.FirstOrDefault(e => e.Email.Equals(user.Email)) != null)
                    {
                        TempData["register"] = "This user already exists";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["register"] = "Registered successfully";
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index","Home");
                    }
                }
                
            }
            TempData["register"] = "Unable to register";
            return RedirectToAction("Index", "Home");
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
        public ActionResult Edit([Bind(Include = "idU,Name,LastName,Email,Password,fileU,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Information", new { id = user.idU });
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
