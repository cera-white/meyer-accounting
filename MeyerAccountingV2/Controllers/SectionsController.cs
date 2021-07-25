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
    public class SectionsController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Sections
        public ActionResult Manage(int? id)
        {
            ViewBag.Id = id;
            ViewBag.Page = db.Pages.Find(id);
            var sections = db.Sections.Include(s => s.Page).Include(s => s.StockImage);

            if (id != null)
            {
                sections = sections.Where(x => x.PageId == id);
            }

            return View(sections.ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            List<StockImage> images = db.StockImages.OrderBy(x => x.Name).ToList();

            ViewBag.PageId = new SelectList(db.Pages, "PageId", "Name", section.PageId);
            ViewBag.ImageId = new SelectList(images, "ImageId", "Name", section.ImageId);
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionId,PageId,Name,Description,ImageId,IsActive,Content")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage", new { id = section.PageId });
            }
            List<StockImage> images = db.StockImages.OrderBy(x => x.Name).ToList();

            ViewBag.PageId = new SelectList(db.Pages, "PageId", "Name", section.PageId);
            ViewBag.ImageId = new SelectList(images, "ImageId", "Name", section.ImageId);
            return View(section);
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
