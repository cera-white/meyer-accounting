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
    public class StaffController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Staff
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Staffs.Where(x => x.IsActive == true).OrderBy(x => x.Rank).ToList());
        }

        public ActionResult Manage()
        {
            return View(db.Staffs.OrderBy(x => x.Rank).ToList());
        }

        // GET: Staff/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            Staff staff = new Staff() { IsActive = true, Rank = (db.Staffs.Max(x => x.Rank) + 1) };
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View(staff);
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,UserId,FirstName,MiddleName,LastName,Title,Image,Description,Rank,IsActive")] Staff staff, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (image != null)
                {
                    string fileName = image.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();

                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError("image", "File must be a .jpg or .png");

                        ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.UserId);
                        return View(staff);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    image.SaveAs(Server.MapPath("~/Content/Image/team/" + fileName));

                    staff.Image = fileName;
                }
                else
                {
                    staff.Image = "personIcon.png";
                }
                #endregion

                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.UserId);
            return View(staff);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.UserId);
            return View(staff);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,UserId,FirstName,MiddleName,LastName,Title,Image,Image2,Description,Rank,IsActive")] Staff staff, HttpPostedFileBase image2)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (image2 != null)
                {
                    string fileName = image2.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();

                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError("image", "File must be a .jpg or .png");

                        ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.UserId);
                        return View(staff);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    image2.SaveAs(Server.MapPath("~/Content/Image/team/" + fileName));

                    staff.Image = fileName;
                }
                #endregion

                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", staff.UserId);
            return View(staff);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            #region DeleteRelatedQualifications
            var qualifications = db.StaffQualifications.Where(x => x.StaffId == id);

            foreach (var qualification in qualifications)
            {
                db.StaffQualifications.Remove(qualification);
            }
            db.SaveChanges();
            #endregion

            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
            db.SaveChanges();

            #region DeleteFileFromServer
            string path = Server.MapPath("~/Content/Image/team/" + staff.Image);

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
