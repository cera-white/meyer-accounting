using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeyerAccountingV2.EF;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity;

namespace MeyerAccountingV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewslettersController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Newsletters
        [AllowAnonymous]
        public ActionResult Index()
        {
            var newsletters = db.Newsletters.Where(x => x.IsActive == true).OrderByDescending(x => x.DateSubmitted).Include(n => n.Staff);
            return View(newsletters.ToList());
        }

        // GET: Newsletters/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newsletter newsletter = db.Newsletters.Find(id);
            if (newsletter == null)
            {
                return HttpNotFound();
            }
            return View(newsletter);
        }

        public ActionResult Manage()
        {
            var newsletters = db.Newsletters.Where(x => x.IsActive == true).OrderByDescending(x => x.DateSubmitted).Include(n => n.Staff);
            return View(newsletters.ToList());
        }

        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newsletter newsletter = db.Newsletters.Find(id);
            if (newsletter == null)
            {
                return HttpNotFound();
            }
            return View(newsletter);
        }

        // GET: Newsletters/Create
        public ActionResult Create()
        {
            var newsletter = new Newsletter() { IsActive = true, DateSubmitted = DateTime.Now };

            var userId = User.Identity.GetUserId();
            var staff = db.Staffs.Where(x => x.UserId == userId).FirstOrDefault();
            
            if (staff != null)
            {
                newsletter.StaffId = staff.StaffId;
            }

            //ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName");

            return View(newsletter);
        }

        // POST: Newsletters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewsletterId,Title,StaffId,DateSubmitted,Description,Image,DownloadLink,Tags,IsActive")] Newsletter newsletter, HttpPostedFileBase image, HttpPostedFileBase downloadLink)
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

                        return View(newsletter);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    image.SaveAs(Server.MapPath("~/Content/Image/blog/" + fileName));

                    newsletter.Image = fileName;
                }

                if (downloadLink != null)
                {
                    string fileName = downloadLink.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.'));

                    if (ext == ".exe" || ext == ".dll")
                    {
                        ModelState.AddModelError("DownloadLink", ".exe and .dll files not allowed");

                        return View(newsletter);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    downloadLink.SaveAs(Server.MapPath("~/Content/Download/" + fileName));

                    newsletter.DownloadLink = fileName;

                    var file = new EF.File()
                    {
                        Filename = fileName,
                        Description = string.Format("Newsletter from {0} in Document format", newsletter.DateSubmitted.ToString("MMMM yyyy")),
                        Name = string.Format("{0} Newsletter", newsletter.Title)
                    };
                    db.Files.Add(file);
                }
                #endregion

                db.Newsletters.Add(newsletter);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            //ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", newsletter.StaffId);
            return View(newsletter);
        }

        // GET: Newsletters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newsletter newsletter = db.Newsletters.Find(id);
            if (newsletter == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", newsletter.StaffId);
            return View(newsletter);
        }

        // POST: Newsletters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsletterId,Title,StaffId,DateSubmitted,Description,Image,Image2,DownloadLink,DownloadLink2,Tags,IsActive")] Newsletter newsletter, HttpPostedFileBase image2, HttpPostedFileBase downloadLink2)
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

                        return View(newsletter);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    image2.SaveAs(Server.MapPath("~/Content/Image/blog/" + fileName));

                    newsletter.Image = fileName;
                }

                if (downloadLink2 != null)
                {
                    string fileName = downloadLink2.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf('.'));

                    if (ext == ".exe" || ext == ".dll")
                    {
                        ModelState.AddModelError("DownloadLink", ".exe and .dll files not allowed");

                        return View(newsletter);
                    }

                    //fileName = Guid.NewGuid() + ext;

                    downloadLink2.SaveAs(Server.MapPath("~/Content/Download/" + fileName));

                    var file = db.Files.Where(x => x.Filename == newsletter.DownloadLink).FirstOrDefault();

                    newsletter.DownloadLink = fileName;
                    file.Filename = fileName;
                    db.Entry(file).State = EntityState.Modified;
                }
                #endregion

                db.Entry(newsletter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            //ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FullName", newsletter.StaffId);
            return View(newsletter);
        }

        // GET: Newsletters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newsletter newsletter = db.Newsletters.Find(id);
            if (newsletter == null)
            {
                return HttpNotFound();
            }
            return View(newsletter);
        }

        // POST: Newsletters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Newsletter newsletter = db.Newsletters.Find(id);
            db.Newsletters.Remove(newsletter);
            db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: Newsletters/Send/5
        public ActionResult Send(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newsletter newsletter = db.Newsletters.Find(id);
            if (newsletter == null)
            {
                return HttpNotFound();
            }
            return View(newsletter);
        }

        // POST: Newsletters/Send/5
        [HttpPost, ActionName("Send")]
        [ValidateAntiForgeryToken]
        public ActionResult SendConfirmed(int id)
        {
            Newsletter newsletter = db.Newsletters.Find(id);
            var subscribers = db.Subscribers.Where(x => x.EmailConfirmed == true && x.IsActive == true);

            foreach (var subscriber in subscribers)
            {
                SendNewsletter(subscriber.Email, newsletter);
            }

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult AddComment([Bind(Include = "NewsletterId,UserName,Comment1")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.DateSubmitted = DateTime.Now;
                comment.IsActive = true;
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = comment.NewsletterId });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddSubscriber([Bind(Include = "Email")] Subscriber subscriber)
        {
            subscriber.Email = subscriber.Email.Trim();
            var pastSubscriber = db.Subscribers.FirstOrDefault(m => m.Email == subscriber.Email);
            if (pastSubscriber != null)
            {
                if (!pastSubscriber.EmailConfirmed)
                {
                    SendConfirmationEmail(pastSubscriber.Email);

                    return Json(new { success = true, message = "Success! A new confirmation email has been sent to you." });
                }
                else if (!pastSubscriber.IsActive)
                {
                    pastSubscriber.IsActive = true;
                    db.Entry(pastSubscriber).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true, message = "Success! Your subscription has been reactivated." });
                }
                return Json(new { success = false, message = "Oops! The email address you entered is already in use." });
            }

            if (ModelState.IsValid)
            {
                subscriber.EmailConfirmed = false;
                subscriber.IsActive = true;
                db.Subscribers.Add(subscriber);
                db.SaveChanges();

                SendConfirmationEmail(subscriber.Email);

                return Json(new { success = true, message = "Success! Please check your inbox to confirm your subscription." });
            }
            return Json(new { success = false, message = "Oops! Something went wrong. Please look over the form and try again." });
        }

        [AllowAnonymous]
        public ActionResult Verify(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userEmail = Decrypt(email);
            var subscriber = db.Subscribers.FirstOrDefault(x => x.Email == userEmail);

            if (subscriber == null)
            {
                return HttpNotFound();
            }

            subscriber.EmailConfirmed = true;
            db.Entry(subscriber).State = EntityState.Modified;
            db.SaveChanges();

            return View(subscriber);
        }

        [AllowAnonymous]
        public ActionResult Unsubscribe(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subscriber = db.Subscribers.FirstOrDefault(x => x.Email == email.Trim());

            if (subscriber == null)
            {
                return HttpNotFound();
            }

            subscriber.IsActive = false;
            db.Entry(subscriber).State = EntityState.Modified;
            db.SaveChanges();

            return View(subscriber);
        }

        private void SendConfirmationEmail(string userEmail)
        {
            var encryptedId = HttpUtility.HtmlEncode(Encrypt(userEmail));
            var callbackUrl = Url.Action("Verify", "Newsletters", new { email = encryptedId }, protocol: Request.Url.Scheme);
            var unsubscribeUrl = Url.Action("Unsubscribe", "Newsletters", new { email = userEmail }, protocol: Request.Url.Scheme);

            var body = string.Format("<h1>Thanks for subscribing to Meyer Accounting!</h1>" +
                "<p>You're one click away from receiving free monthly newsletters, special offers, and more.</p>" +
                "<p>Please click the following link or paste it into your browser to confirm your email address:<br />" +
                "<a href='{0}' target='_blank'>{0}</a></p><br /><br />" +
                "<hr /><p style='font-style:italic'>If you've received this email in error, please disregard or <a href='{1}' target='_blank'>click here to unsubscribe</a>.</p>",
                callbackUrl, unsubscribeUrl);

            MailMessage msg = new MailMessage(
                "noreply@taxhelp4less.com", //from address
                userEmail, //to address
                "Meyer Accounting - Please Confirm Subscription", //subject
                body);

            //configure the Mail Message object
            msg.IsBodyHtml = true;

            //create and configure SMTPClientObject
            SmtpClient client = new SmtpClient("mail.taxhelp4less.com", 25)
            {
                Credentials = new NetworkCredential("noreply@taxhelp4less.com", "nyS5v87$"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(msg);
        }

        private void SendNewsletter(string userEmail, Newsletter newsletter)
        {
            var unsubscribeUrl = Url.Action("Unsubscribe", "Newsletters", new { email = userEmail }, protocol: Request.Url.Scheme);
            var body = newsletter.Description +
                string.Format("<hr /><p style='font-style:italic'>If you no longer wish to receive these emails from Meyer Accounting, please <a href='{0}' target='_blank'>click here to unsubscribe</a>.</p>", unsubscribeUrl);

            MailMessage msg = new MailMessage(
                "noreply@taxhelp4less.com", //from address
                userEmail, //to address
                string.Format("Meyer Accounting - {0}", newsletter.Title), //subject
                body);

            //configure the Mail Message object
            msg.IsBodyHtml = true;

            //create and configure SMTPClientObject
            SmtpClient client = new SmtpClient("mail.taxhelp4less.com", 25)
            {
                Credentials = new NetworkCredential("noreply@taxhelp4less.com", "nyS5v87$"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(msg);
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "S35CX5324PEKJQE";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "S35CX5324PEKJQE";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
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
