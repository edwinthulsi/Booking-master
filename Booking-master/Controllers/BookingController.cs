using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_master.Models;

namespace Booking_master.Controllers
{
    public class BookingController : Controller
    {
        private BookingDbContext db = new BookingDbContext();

        // GET: Booking
        public ActionResult Index()
        {
            return View(db.bookings.ToList());
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,forRef,refNo,Name,Surname,Email,ContactNumber,CheckInDate,CheckOutDate,numberOfPeople,totalPrice,deposit")] Booking booking)
        {
            //if (ModelState.IsValid)
            //{
            //    db.bookings.Add(booking);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(booking);

            booking.CheckInDate = new DateTime(booking.CheckInDate.Year, booking.CheckInDate.Month, booking.CheckInDate.Day);
            if (ModelState.IsValid && isDateValid(booking))
            {
                var use = db.bookings.ToList().Find(x => x.BookingId == booking.BookingId);
                if (use != null)
                {
                    ModelState.AddModelError("", "Booking ID already exists!");
                    ModelState.Clear();
                    ViewBag.Messageb = "Booking ID already exists! Please try again!";
                }
                var user = db.bookings.ToList().Find(x => x.CheckInDate == booking.CheckInDate);
                if (user != null)
                {
                    ModelState.AddModelError("", "Date is already booked!");
                    ModelState.Clear();
                    ViewBag.Messagea = "Date is already booked! Please try again!";
                }

                else
                {

                    string str = TempData["Ref"].ToString();
                    booking.forRef = str;
                    booking.refNo = booking.generateRefNo();
                    booking.venue = booking.chooseRoom();
                    booking.totalPrice = booking.calcTotalPrice();
                    booking.deposit = booking.calcDeposit();
                    db.bookings.Add(booking);
                    db.SaveChanges();

                    using (BookingDbContext dbs = new BookingDbContext())
                    {
                        var us = dbs.bookings.ToList().Find(u => u.BookingId == booking.BookingId);
                        if (us != null)
                        {
                            Session["Total"] = us.totalPrice.ToString();

                            return RedirectToAction("makepay");
                        }
                    }
                }
            }

            return View(booking);
        }

        private bool isDateValid(Booking booking)
        {
            if (IsDatePassed(booking.CheckInDate))
            {
                ModelState.AddModelError("inValidDate", "Date has already passed!");
                return false;
            }
            return true;
        }

        private bool IsDatePassed(DateTime dateToValiate)
        {
            var result = dateToValiate < DateTime.Now;
            return result;
        }

        public string RenderViewAsString(string viewName, object model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
            // create a context to render a view based on a model
            ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    new ViewDataDictionary(model),
                    new TempDataDictionary(),
                    stringWriter
                    );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }
    

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,forRef,refNo,Name,Surname,Email,ContactNumber,CheckInDate,CheckOutDate,numberOfPeople,totalPrice,deposit")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.bookings.Find(id);
            db.bookings.Remove(booking);
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

        //Generate Invoice
        public ActionResult Invoice()
        {
            return View();
        }
        public ActionResult Bookings(Booking ievent)
        {
            return View(db.bookings.ToList());
        }
        public ActionResult makepay()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Client()
        {
            //string search = Session["userId"].ToString();

            //var cli = from e in db.registrations
            //          select e;
            //if(!string.IsNullOrEmpty(search))
            //{
            //    cli = cli.Where(u => u.userId.ToString().Contains(search));
            //}
            return PartialView("_Client", db.registrations.ToList());

        }

        [ChildActionOnly]
        public ActionResult Booking()
        {
            //string str = TempData["Ref"].ToString();
            //var book = from x in db.events
            //          select x;
            //book = book.Where(y => y.forRef.Equals(str));
            //Event eve = new Event();
            return PartialView("_Booking", db.bookings.ToList());

        }
    }
}
