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
    public class StaffQualificationsController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: StaffQualifications
        public ActionResult Manage(int? id)
        {
            ViewBag.Id = id;
            ViewBag.Staff = db.Staffs.Find(id);
            var staffQualifications = db.StaffQualifications.Include(s => s.Staff);

            if (id != null)
            {
                staffQualifications = staffQualifications.Where(x => x.StaffId == id);
            }

            return View(staffQualifications.ToList());
        }

        // GET: StaffQualifications/Create
        public ActionResult Create(int? id)
        {
            ViewBag.Id = id;
            StaffQualification staffQualification = new StaffQualification();

            if (id != null)
            {
                staffQualification.StaffId = (int)id;
            }

            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", staffQualification.StaffId);
            return View(staffQualification);
        }

        // POST: StaffQualifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QualificationId,StaffId,Description")] StaffQualification staffQualification)
        {
            if (ModelState.IsValid)
            {
                db.StaffQualifications.Add(staffQualification);
                db.SaveChanges();
                return RedirectToAction("Manage", new { id = staffQualification.StaffId });
            }

            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", staffQualification.StaffId);
            return View(staffQualification);
        }

        // GET: StaffQualifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffQualification staffQualification = db.StaffQualifications.Find(id);
            if (staffQualification == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", staffQualification.StaffId);
            return View(staffQualification);
        }

        // POST: StaffQualifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QualificationId,StaffId,Description")] StaffQualification staffQualification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffQualification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage", new { id = staffQualification.StaffId });
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", staffQualification.StaffId);
            return View(staffQualification);
        }

        // GET: StaffQualifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffQualification staffQualification = db.StaffQualifications.Find(id);
            if (staffQualification == null)
            {
                return HttpNotFound();
            }
            return View(staffQualification);
        }

        // POST: StaffQualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffQualification staffQualification = db.StaffQualifications.Find(id);
            db.StaffQualifications.Remove(staffQualification);
            db.SaveChanges();
            return RedirectToAction("Manage", new { id = staffQualification.StaffId });
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
