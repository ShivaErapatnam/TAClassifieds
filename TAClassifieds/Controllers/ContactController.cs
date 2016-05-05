using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAClassifieds.Models;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact

        TAC_Team4Entities db = new TAC_Team4Entities();

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult SaveToDb(TAC_ContactUs contact)
        {
            contact.ContactId = Guid.NewGuid();
            contact.PostedDate = DateTime.Now;
            db.TAC_ContactUs.Add(contact);
            db.SaveChanges();
            ViewBag.Message = "Your comment has been saved successfully!";
            return View("Contact", contact);
        }
    }
}