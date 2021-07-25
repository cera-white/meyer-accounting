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
    public class StockImagesController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: StockImages
        public ActionResult Manage()
        {
            return View(db.StockImages.OrderBy(x => x.Name).ToList());
        }

        // GET: StockImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockImage stockImage = db.StockImages.Find(id);
            if (stockImage == null)
            {
                return HttpNotFound();
            }
            return View(stockImage);
        }

        // GET: StockImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImageId,Name,Description,Filename")] StockImage stockImage, HttpPostedFileBase filename)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (filename != null)
                {
                    string fileName = filename.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();

                    if (ext !=".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError("filename", "File must be a .jpg or .png");

                        return View(stockImage);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    filename.SaveAs(Server.MapPath("~/Content/Image/stock/" + fileName));

                    stockImage.Filename = fileName;
                }
                #endregion

                db.StockImages.Add(stockImage);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(stockImage);
        }

        // GET: StockImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockImage stockImage = db.StockImages.Find(id);
            if (stockImage == null)
            {
                return HttpNotFound();
            }
            return View(stockImage);
        }

        // POST: StockImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImageId,Name,Description,Filename,Filename2")] StockImage stockImage, HttpPostedFileBase filename2)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (filename2 != null)
                {
                    string fileName = filename2.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();

                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError("filename", "File must be a .jpg or .png");

                        return View(stockImage);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    filename2.SaveAs(Server.MapPath("~/Content/Image/stock/" + fileName));

                    stockImage.Filename = fileName;
                }
                #endregion

                db.Entry(stockImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(stockImage);
        }

        // GET: StockImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockImage stockImage = db.StockImages.Find(id);
            if (stockImage == null)
            {
                return HttpNotFound();
            }
            return View(stockImage);
        }

        // POST: StockImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockImage stockImage = db.StockImages.Find(id);
            db.StockImages.Remove(stockImage);
            db.SaveChanges();

            #region DeleteFileFromServer
            string path = Server.MapPath("~/Content/Image/stock/" + stockImage.Filename);

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
