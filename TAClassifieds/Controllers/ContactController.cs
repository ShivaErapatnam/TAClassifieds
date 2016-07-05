using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            HelperClasses.SendEmail obj = new HelperClasses.SendEmail();
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Dear Admin</b>, <br/>");
            sb.Append("A contact has just visited our site. Here are the details <br/><br/>");
            sb.Append("<b>Contact Name</b> : " + contact.Name + "<br/>");
            sb.Append("<b>Contact ID : </b>" + contact.ContactId + "<br/>");
            sb.Append("<b>Contact Email : </b>" + contact.Email + "<br/>");
            sb.Append("<b>Contact Comment : </b>" + contact.Comment + "<br/>");
            sb.Append("<b>Posted Date : </b>" + contact.PostedDate + "<br/>");
            obj.SendEmailMessage(GetAdminEmails(), sb.ToString(), "Contact Details");
            return View("Contact", contact);
        }
        public string GetAdminEmails()
        {
            TAC_Team4Entities db = new TAC_Team4Entities();
            List<string> list = db.TAC_User.Where(x => x.IsAdmin == true).Select(x => x.Email).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (string item in list)
            {
                sb.Append(item);
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}