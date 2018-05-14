using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_master.Models;

namespace Booking_master.Controllers
{
    public class RegistrationController : Controller
    {
        private BookingDbContext db = new BookingDbContext();

        // GET: Registration
        public ActionResult Index()
        {
            return View(db.registrations.ToList());
        }

        // GET: Registration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userId,Username,firstName,surName,Email,phoneNum,Password,ConfirmPassword,position,forRef")] Registration registration)
        {
            //if (ModelState.IsValid)
            //{
            //    db.registrations.Add(registration);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(registration);

            if (ModelState.IsValid)
            {
                var user = db.registrations.ToList().Find(x => x.Username == registration.Username);
                if (user != null)
                {

                    ModelState.Clear();
                    ViewBag.Messagea = "Username already exists! Please try again!";
                }

                var ph = db.registrations.ToList().Find(x => x.phoneNum == registration.phoneNum);
                if (ph != null)
                {

                    ModelState.Clear();
                    ViewBag.Messageb = "Contact Number already exists! Please try again!";
                }


                var all = db.registrations.ToList().Find(x => x.Username == registration.Username && x.phoneNum == registration.phoneNum);
                if (all != null)
                {

                    ModelState.Clear();
                    ViewBag.Messagec = "Username and Contact Number already exists! Please try again!";
                }

                else
                {
                    registration.forRef = registration.forRefNo();

                    db.registrations.Add(registration);
                    db.SaveChanges();

                    //  dynamic email = new Email("Confirm");
                    //email.To = registration.Email;
                    //email.name = registration.firstName;
                    //email.pass = registration.Password;
                    //email.userName = registration.Username;
                    //email.position = registration.position;
                    //email.message = "Have a great day";
                    //email.Send();

                    using (BookingDbContext dbs = new BookingDbContext())
                    {
                        var us = dbs.registrations.ToList().Find(u => u.Username == registration.Username && u.Password == registration.Password);
                        if (us != null)
                        {
                            Session["userId"] = us.userId.ToString();
                            Session["Username"] = us.Username;
                            Session["firstName"] = us.firstName;
                            Session["surName"] = us.surName;
                            Session["Email"] = us.Email;
                            Session["phoneNum"] = us.phoneNum;
                            Session["forRef"] = us.forRef;
                            TempData["Ref"] = Session["forRef"];
                            return RedirectToAction("ClientLoggedIn");
                        }
                    }
                }
            }

            return View(registration);
        }

        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin([Bind(Include = "userId,Username,firstName,surName,Email,phoneNum,Password,ConfirmPassword,position,forRef")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                var user = db.registrations.ToList().Find(x => x.Username == registration.Username);
                if (user != null)
                {

                    ModelState.Clear();
                    ViewBag.Messagea = "Username already exists! Please try again!";
                }

                var ph = db.registrations.ToList().Find(x => x.phoneNum == registration.phoneNum);
                if (ph != null)
                {

                    ModelState.Clear();
                    ViewBag.Messageb = "Contact Number already exists! Please try again!";
                }


                var all = db.registrations.ToList().Find(x => x.Username == registration.Username && x.phoneNum == registration.phoneNum);
                if (all != null)
                {

                    ModelState.Clear();
                    ViewBag.Messagec = "Username and Contact Number already exists! Please try again!";
                }

                else
                {
                    registration.forRef = registration.forRefNo();

                    db.registrations.Add(registration);
                    db.SaveChanges();
                    using (BookingDbContext dbs = new BookingDbContext())
                    {
                        var us = dbs.registrations.ToList().Find(u => u.Username == registration.Username && u.Password == registration.Password);
                        if (us != null)
                        {
                            Session["userId"] = us.userId.ToString();
                            Session["Username"] = us.Username;
                            Session["firstName"] = us.firstName;
                            Session["surName"] = us.surName;
                            Session["Email"] = us.Email;
                            Session["phoneNum"] = us.phoneNum;
                            Session["forRef"] = us.forRef;
                            TempData["Ref"] = Session["forRef"];
                            TempData["ID"] = Session["userId"];

                            return RedirectToAction("Index", "Registration");
                        }
                    }
                }
            }

            return View(registration);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Registration user)
        {
            using (BookingDbContext dbs = new BookingDbContext())
            {

                var usr = dbs.registrations.Where(u => u.Username == user.Username && u.Password == user.Password && u.position/* == user.position && user.position */== "Admin").FirstOrDefault();
                if (usr != null)
                {
                    Session["userId"] = usr.userId.ToString();
                    Session["Username"] = usr.Username;
                    Session["firstName"] = usr.firstName;
                    Session["surName"] = usr.surName;
                    Session["Email"] = usr.Email;
                    Session["phoneNum"] = usr.phoneNum;
                    Session["forRef"] = usr.forRef;
                    TempData["Ref"] = Session["forRef"];
                    return RedirectToAction("Index");
                }

                var us = dbs.registrations.Where(u => u.Username == user.Username && u.Password == user.Password && u.position /*== user.position && user.position*/ == "Client").FirstOrDefault();
                if (us != null)
                {
                    Session["userId"] = us.userId.ToString();
                    Session["Username"] = us.Username;
                    Session["firstName"] = us.firstName;
                    Session["surName"] = us.surName;
                    Session["Email"] = us.Email;
                    Session["phoneNum"] = us.phoneNum;
                    Session["forRef"] = us.forRef;
                    TempData["Ref"] = Session["forRef"].ToString();
                    return RedirectToAction("ClientLoggedIn");
                }

                else
                {
                    ModelState.AddModelError("", "Please enter correct username and password ");
                    ModelState.Clear();
                    ViewBag.Message = "Username and/or Password is Incorrect! Please try again!";
                }

            }
            return View();
        }
        public ActionResult ClientLoggedIn(Registration user)
        {
            if (Session["userId"] != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult RecoverPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RecoverPassword([Bind(Include = "Username,Password,Email")]Registration registraion)
        {
            using (BookingDbContext dbs = new BookingDbContext())
            {

                var udr = db.registrations.Where(u => u.Email == registraion.Email && u.Username == registraion.Username).FirstOrDefault();
                if (udr != null)
                {
                    // dynamic email = new Email("View");

                    //email.To = registraion.Email;
                    //var udg = db.registrations.Where(m => m.Password == registraion.Password && m.firstName == registraion.firstName);
                    //email.pass = udr.Password;
                    //email.name = registraion.firstName;
                    //email.userName = registraion.Username;
                    //email.message = "Have a great day";
                    //email.Send();
                    //return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email Address");

                }
            }
            return View(registraion);
        }


        // GET: Registration/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,Username,firstName,surName,Email,phoneNum,Password,ConfirmPassword,position,forRef")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                registration.forRef = registration.forRefNo();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registration);
        }

        // GET: Registration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.registrations.Find(id);
            db.registrations.Remove(registration);
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
