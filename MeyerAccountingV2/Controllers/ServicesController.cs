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
    public class ServicesController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Services
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.IndividualServices = db.Services.Where(x => x.ServiceTypeId == 1 && x.IsActive == true).ToList();
            ViewBag.BusinessServices = db.Services.Where(x => x.ServiceTypeId == 2 && x.IsActive == true).ToList();

            return View();
        }

        public ActionResult Manage()
        {
            var services = db.Services.ToList();

            return View(services);
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            var service = new Service() { IsActive = true };
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "ServiceTypeId", "Name");
            return View(service);
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServiceId,Name,Summary,Description,Icon,ServiceTypeId,IsActive")] Service service, HttpPostedFileBase icon)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (icon != null)
                {
                    string fileName = icon.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();

                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError("icon", "File must be a .jpg or .png");

                        ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
                        return View(service);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    icon.SaveAs(Server.MapPath("~/Content/Image/icons/" + fileName));

                    service.Icon = fileName;
                }
                else
                {
                    service.Icon = "default-icon.png";
                }
                #endregion
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceId,Name,Summary,Description,Icon,Icon2,ServiceTypeId,IsActive")] Service service, HttpPostedFileBase icon2)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (icon2 != null)
                {
                    string fileName = icon2.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();

                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError("icon", "File must be a .jpg or .png");

                        ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
                        return View(service);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    icon2.SaveAs(Server.MapPath("~/Content/Image/icons/" + fileName));

                    service.Icon = fileName;
                }
                #endregion

                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "ServiceTypeId", "Name", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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
