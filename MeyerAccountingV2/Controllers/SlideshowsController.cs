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
    public class SlideshowsController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Slideshows
        public ActionResult Manage()
        {
            var slideshows = db.Slideshows.Include(s => s.Link).Include(s => s.StockImage);
            return View(slideshows.ToList());
        }

        // GET: Slideshows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slideshow slideshow = db.Slideshows.Find(id);
            if (slideshow == null)
            {
                return HttpNotFound();
            }
            return View(slideshow);
        }

        // GET: Slideshows/Create
        public ActionResult Create()
        {
            Slideshow slideshow = new Slideshow() { IsActive = true };
            List<Link> links = db.Links.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            List<StockImage> images = db.StockImages.OrderBy(x => x.Name).ToList();

            ViewBag.LinkId = new SelectList(links, "LinkId", "Name");
            ViewBag.ImageId = new SelectList(images, "ImageId", "Name");
            return View(slideshow);
        }

        // POST: Slideshows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SlideshowId,Title,Description,ImageId,IsRightAligned,LinkId,IsActive")] Slideshow slideshow)
        {
            if (ModelState.IsValid)
            {
                db.Slideshows.Add(slideshow);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            List<Link> links = db.Links.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            List<StockImage> images = db.StockImages.OrderBy(x => x.Name).ToList();

            ViewBag.LinkId = new SelectList(links, "LinkId", "Name", slideshow.LinkId);
            ViewBag.ImageId = new SelectList(images, "ImageId", "Name", slideshow.ImageId);
            return View(slideshow);
        }

        // GET: Slideshows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slideshow slideshow = db.Slideshows.Find(id);
            if (slideshow == null)
            {
                return HttpNotFound();
            }

            List<Link> links = db.Links.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            List<StockImage> images = db.StockImages.OrderBy(x => x.Name).ToList();

            ViewBag.LinkId = new SelectList(links, "LinkId", "Name", slideshow.LinkId);
            ViewBag.ImageId = new SelectList(images, "ImageId", "Name", slideshow.ImageId);
            return View(slideshow);
        }

        // POST: Slideshows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SlideshowId,Title,Description,ImageId,IsRightAligned,LinkId,IsActive")] Slideshow slideshow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slideshow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            List<Link> links = db.Links.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            List<StockImage> images = db.StockImages.OrderBy(x => x.Name).ToList();

            ViewBag.LinkId = new SelectList(links, "LinkId", "Name", slideshow.LinkId);
            ViewBag.ImageId = new SelectList(images, "ImageId", "Name", slideshow.ImageId);
            return View(slideshow);
        }

        // GET: Slideshows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slideshow slideshow = db.Slideshows.Find(id);
            if (slideshow == null)
            {
                return HttpNotFound();
            }
            return View(slideshow);
        }

        // POST: Slideshows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slideshow slideshow = db.Slideshows.Find(id);
            db.Slideshows.Remove(slideshow);
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
