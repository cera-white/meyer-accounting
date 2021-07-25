using MeyerAccountingV2.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeyerAccountingV2.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private MeyerAccountingEntities db = new MeyerAccountingEntities();

        public ActionResult Index()
        {
            var sections = db.Sections.Where(x => x.PageId == 1).ToList();

            ViewBag.Slides = db.Slideshows.Where(x => x.IsActive == true).ToList();
            ViewBag.IndividualServices = db.Services.Where(x => x.ServiceTypeId == 1 && x.IsActive == true).ToList();
            ViewBag.BusinessServices = db.Services.Where(x => x.ServiceTypeId == 2 && x.IsActive == true).ToList();
            ViewBag.Testimonials = db.Testimonials.Where(x => x.IsActive == true).OrderByDescending(x => x.DateSubmitted).ToList();
            ViewBag.Staff = db.Staffs.Where(x => x.IsActive == true).OrderBy(x => x.Rank).ToList();
            ViewBag.QuickLinks = db.Links.Where(x => x.LinkTypeId == 1 && x.IsActive == true).ToList();
            ViewBag.RefundLinks = db.Links.Where(x => x.LinkTypeId == 2 && x.IsActive == true).ToList();
            ViewBag.ChecklistLinks = db.Links.Where(x => x.LinkTypeId == 3 && x.IsActive == true).ToList();

            return View(sections);
        }

        public ActionResult About()
        {
            var sections = db.Sections.Where(x => x.PageId == 2).ToList();

            ViewBag.AddressOneLine = string.Format("{0}, {1}, {2}",
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "Address").Value,
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "City").Value,
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "State").Value);

            return View(sections);
        }

        public ActionResult Contact()
        {
            var sections = db.Sections.Where(x => x.PageId == 2).ToList();

            ViewBag.AddressOneLine = string.Format("{0}, {1}, {2}",
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "Address").Value,
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "City").Value,
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "State").Value);
            ViewBag.GoogleMaps = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Google Maps").Value;
            ViewBag.MapLatitude = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Map Latitude").Value;
            ViewBag.MapLongitude = db.BusinessInfoes.FirstOrDefault(x => x.Name == "Map Longitude").Value;

            return View(sections);
        }

        [HttpPost]
        public ActionResult SendMessage([Bind(Include = "Name,Email,Subject,Message")] ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                var body = Regex.Replace(contact.Message, @"\r\n?|\n", "<br />");

                MailMessage msg = new MailMessage(
                string.Format("{0} {1}", contact.Name.Trim(), "noreply@taxhelp4less.com"), //from address
                db.BusinessInfoes.FirstOrDefault(x => x.Name == "Email").Value, //to address
                contact.Subject.Trim(), //subject
                body);

                //configure the Mail Message object
                msg.IsBodyHtml = true;
                msg.ReplyToList.Add(contact.Email.Trim());

                //create and configure SMTPClientObject
                SmtpClient client = new SmtpClient("mail.taxhelp4less.com", 25)
                {
                    Credentials = new NetworkCredential("noreply@taxhelp4less.com", "nyS5v87$"),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                client.Send(msg);

                return Json(new { success = true, message = "Success! Your message has been sent." });
            }
            return Json(new { success = false, message = "Oops! Something went wrong. Please look over the form and try again." });
        }
    }
}
