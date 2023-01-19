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
    public class AdminController : Controller
    {
        private GestHotEntities1 db = new GestHotEntities1();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                var pQuery = db.Users.Where(e => e.Role == -1);
                return View(pQuery);
            }
            else
                return RedirectToAction("login");
        }

        public ActionResult Users()
        {
            if (Session["admin"] != null)
            {
                var pQuery = db.Users.Where(e => e.Role == -1);
                return View(pQuery);
            }
            else
                return RedirectToAction("login");
        }
        public ActionResult Hotel()
        {
            if (Session["admin"] != null)
            {
                return View(db.Hotels.ToList());
            }
            else
                return RedirectToAction("login");
        }
        public ActionResult Owner()
        {
            if (Session["admin"] != null)
            {
                var pQuery = db.Users.Where(e => e.Role == 0);
                return View(pQuery);
            }
            else
                return RedirectToAction("login");
        }
        public ActionResult Review()
        {
            if (Session["admin"] != null)
            {
                return View(db.Comments.ToList());
            }
            else
                return RedirectToAction("login");
        }
        public ActionResult Reservation()
        {
            if (Session["admin"] != null)
            {
                return View(db.Reservations.ToList());
            }
            else
                return RedirectToAction("login");
        }
        public ActionResult BlackList()
        {
            if (Session["admin"] != null)
            {
                return View(db.BlackLists.ToList());
            }
            else
                return RedirectToAction("login");
        }

        // LOGIN
        // GET
        public ActionResult login()
        {
            return View();
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(string email,string pass)
        {
            var pQuery = db.Users.FirstOrDefault(e => e.Email.Equals(email) && e.Password.Equals(pass) && e.Role == 1);
            if (pQuery != null)
            {
                Session["admin"] = "good";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        /*public ActionResult login([Bind(Include = "Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var pQuery = db.Users.FirstOrDefault(e => e.Email.Equals(user.Email) && e.Password.Equals(user.Password) && e.Role == 1);
                if(pQuery != null)
                {
                    Session["admin"] = "good";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            else
            {
                return RedirectToAction("Reservation");
            }
        }*/

        public ActionResult logout()
        {
            Session["admin"] = null;
            return RedirectToAction("login");
        }


        //user,hotel,review
        public ActionResult deleteU(int? id)
        {
            User usr = db.Users.Find(id);
            db.Users.Remove(usr);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult deleteH(int? id)
        {
            Hotel usr = db.Hotels.Find(id);
            db.Hotels.Remove(usr);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult deleteR(int? id)
        {
            Comment usr = db.Comments.Find(id);
            db.Comments.Remove(usr);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult blacklistU(int? id)
        {
            User usr = db.Users.Find(id);
            BlackList bl = new BlackList(usr.Email);

            db.BlackLists.Add(bl);
            db.Users.Remove(usr);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
