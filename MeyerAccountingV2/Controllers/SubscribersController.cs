using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeyerAccountingV2.EF;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MeyerAccountingV2.Controllers
{
    public class SubscribersController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        // GET: Subscribers
        public ActionResult Manage()
        {
            return View(db.Subscribers.ToList());
        }

        // GET: Subscribers/Create
        public ActionResult Create()
        {
            var subscriber = new Subscriber() { IsActive = true };
            return View(subscriber);
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubscriberId,Email,EmailConfirmed,IsActive")] Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                db.Subscribers.Add(subscriber);
                db.SaveChanges();

                SendConfirmationEmail(subscriber.Email);
                return RedirectToAction("Manage");
            }

            return View(subscriber);
        }

        // GET: Subscribers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubscriberId,Email,EmailConfirmed,IsActive")] Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                var original = db.Subscribers.AsNoTracking().FirstOrDefault(p => p.SubscriberId == subscriber.SubscriberId);
                // If email has changed, send a new confirmation email
                if (original.Email != subscriber.Email )
                {
                    subscriber.EmailConfirmed = false;
                    SendConfirmationEmail(subscriber.Email);
                }

                db.Entry(subscriber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(subscriber);
        }

        // GET: Subscribers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscriber subscriber = db.Subscribers.Find(id);
            db.Subscribers.Remove(subscriber);
            db.SaveChanges();
            return RedirectToAction("Manage");
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
