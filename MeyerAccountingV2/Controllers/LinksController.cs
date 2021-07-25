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
    public class LinksController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Links
        public ActionResult Manage()
        {
            var links = db.Links.Include(l => l.LinkType);
            return View(links.ToList());
        }

        // GET: Links/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // GET: Links/Create
        public ActionResult Create()
        {
            ViewBag.LinkTypeId = new SelectList(db.LinkTypes, "LinkTypeId", "Name");
            Link link = new Link() { IsActive = true };
            return View(link);
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LinkId,Name,Url,LinkTypeId,IsActive,Filename")] Link link, HttpPostedFileBase filename)
        {
            if (filename != null)
            {
                // Temporarily set URL to filename so it passes validation
                link.Url = filename.FileName;
            }

            if (ModelState.IsValid)
            {
                #region FileUpload
                if (filename != null)
                {
                    string fileName = filename.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.'));

                    if (ext == ".exe" || ext == ".dll")
                    {
                        ModelState.AddModelError("filename", ".exe and .dll files not allowed");

                        ViewBag.LinkTypeId = new SelectList(db.LinkTypes, "LinkTypeId", "Name", link.LinkTypeId);
                        return View(link);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    filename.SaveAs(Server.MapPath("~/Content/Download/" + fileName));

                    File file = new File()
                    {
                        Name = link.Name,
                        Filename = fileName,
                        LinkId = link.LinkId
                    };

                    db.Files.Add(file);
                    link.Url = "/Content/Download/" + file.Filename;
                }
                #endregion

                db.Links.Add(link);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            ViewBag.LinkTypeId = new SelectList(db.LinkTypes, "LinkTypeId", "Name", link.LinkTypeId);
            return View(link);
        }

        // GET: Links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            ViewBag.LinkTypeId = new SelectList(db.LinkTypes, "LinkTypeId", "Name", link.LinkTypeId);
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LinkId,Name,Url,LinkTypeId,IsActive")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            ViewBag.LinkTypeId = new SelectList(db.LinkTypes, "LinkTypeId", "Name", link.LinkTypeId);
            return View(link);
        }

        // GET: Links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Link link = db.Links.Find(id);
            List<File> files = db.Files.Where(x => x.LinkId == link.LinkId).ToList();

            foreach (var file in files)
            {
                file.LinkId = null;
                db.Entry(file).State = EntityState.Modified;
            }

            db.Links.Remove(link);
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
