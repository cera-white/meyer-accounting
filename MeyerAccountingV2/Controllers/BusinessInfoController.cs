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
    public class BusinessInfoController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: BusinessInfo
        public ActionResult Manage()
        {
            return View(db.BusinessInfoes.Where(x => x.IsActive == true).ToList());
        }

        // GET: BusinessInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessInfo businessInfo = db.BusinessInfoes.Find(id);
            if (businessInfo == null)
            {
                return HttpNotFound();
            }
            return View(businessInfo);
        }

        // POST: BusinessInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InfoId,Name,Value,IsActive")] BusinessInfo businessInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(businessInfo);
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
