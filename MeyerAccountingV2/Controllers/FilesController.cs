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
    public class FilesController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Files
        public ActionResult Manage()
        {
            return View(db.Files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            //ViewBag.LinkId = new SelectList(db.Links, "LinkId", "Name");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileId,Name,Description,Filename,LinkId")] File file, HttpPostedFileBase filename)
        {
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

                        return View(file);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    filename.SaveAs(Server.MapPath("~/Content/Download/" + fileName));

                    file.Filename = fileName;
                }
                #endregion

                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            //ViewBag.LinkId = new SelectList(db.Links, "LinkId", "Name", file.LinkId);
            return View(file);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            //ViewBag.LinkId = new SelectList(db.Links, "LinkId", "Name", file.LinkId);
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileId,Name,Description,Filename,Filename2,LinkId")] File file, HttpPostedFileBase filename2)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (filename2 != null)
                {
                    string fileName = filename2.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.'));

                    if (ext == ".exe" || ext == ".dll")
                    {
                        ModelState.AddModelError("filename", ".exe and .dll files not allowed");

                        return View(file);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    filename2.SaveAs(Server.MapPath("~/Content/Download/" + fileName));

                    file.Filename = fileName;
                }
                #endregion

                #region UpdateLink
                if (file.LinkId != null)
                {
                    Link link = db.Links.Find(file.LinkId);

                    link.Url = "/Content/Download/" + file.Filename;
                    db.Entry(link).State = EntityState.Modified;
                }
                #endregion

                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            //ViewBag.LinkId = new SelectList(db.Links, "LinkId", "Name", file.LinkId);
            return View(file);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
            db.SaveChanges();

            #region DeleteFileFromServer
            string path = Server.MapPath("~/Content/Download/" + file.Filename);
            
            try
            {
                System.IO.File.Delete(path);
            }
            catch
            {
                // If it can't delete the file, just move on
            }
            #endregion

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
