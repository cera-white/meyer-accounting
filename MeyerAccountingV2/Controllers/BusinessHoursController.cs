using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeyerAccountingV2.EF;

namespace MeyerAccountingV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BusinessHoursController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: BusinessHours
        public ActionResult Manage()
        {
            return View(db.BusinessHours.ToList());
        }

        // GET: BusinessHours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessHours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BusinessHourId,Day,Hours")] BusinessHour businessHour)
        {
            if (ModelState.IsValid)
            {
                db.BusinessHours.Add(businessHour);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(businessHour);
        }

        // GET: BusinessHours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessHour businessHour = db.BusinessHours.Find(id);
            if (businessHour == null)
            {
                return HttpNotFound();
            }
            return View(businessHour);
        }

        // POST: BusinessHours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BusinessHourId,Day,Hours")] BusinessHour businessHour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessHour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(businessHour);
        }

        // GET: BusinessHours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessHour businessHour = db.BusinessHours.Find(id);
            if (businessHour == null)
            {
                return HttpNotFound();
            }
            return View(businessHour);
        }

        // POST: BusinessHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessHour businessHour = db.BusinessHours.Find(id);
            db.BusinessHours.Remove(businessHour);
            db.SaveChanges();
            return RedirectToAction("Manage");
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
