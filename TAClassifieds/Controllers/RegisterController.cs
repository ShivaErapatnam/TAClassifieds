using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TAClassifieds.Models;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Controllers
{
    public class RegisterController : Controller
    {
        TAC_Team4Entities db = new TAC_Team4Entities();
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(TAC_User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var list = db.TAC_User.ToList();
                    if ((list.Where(x => x.Email.Equals(user.Email)).Count() <= 0) && user.TermsAndConditions)
                    {
                        user.UserId = Guid.NewGuid();
                        user.IsAdmin = false;
                        user.IsActive = true;
                        user.IsLocked = false;
                        user.CreatedDate = DateTime.Now;
                        user.UPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.UPassword, "SHA1");
                        db.TAC_User.Add(user);
                        db.SaveChanges();
                        SendMail(user);
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Email ID already exists. Please try another one.";
                        //ViewBag.ErrorMessage.ForeColor = System.Drawing.Color.Red;
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Technical Problem. Please try again.";
                // ViewBag.ErrorMessage.ForeColor = System.Drawing.Color.Red;
            }
            return View();
        }

        public void SendMail(TAC_User user)
        {
            try
            {
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Dear " + user.First_Name + ",<br/><br/>");
                sbEmailBody.Append("Please click on the following link to activate your account");
                sbEmailBody.Append("<br/>");
                sbEmailBody.Append("http://www.taclassifieds.com/Login/Login/ConfirmMail?UID=" + user.UserId);
                sbEmailBody.Append("<br/><br/>");
                sbEmailBody.Append("<b>TechAspect Solutions</b>");

                HelperClasses.SendEmail obj = new HelperClasses.SendEmail();
                obj.SendEmailMessage(user.Email, sbEmailBody.ToString(), "Account Activation");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessagee = "Technical Problem while sending an confirmation Email. Please try again.";
            }
        }

        public ActionResult ConfirmMail(Guid UID)
        {
            try
            {
                TAC_User user = new TAC_User();
                if (Request.QueryString.Count > 0 && Request.QueryString.Keys[0] == "UID")
                {
                    var CurrentUser = db.TAC_User.Find(UID);
                    CurrentUser.RepeatPassword = CurrentUser.UPassword;
                    CurrentUser.IsVerified = true;
                    db.TAC_User.Attach(CurrentUser);
                    var entry = db.Entry(CurrentUser);
                    entry.Property(e => e.IsVerified).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Login", "Login");

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "This Link is Invalid";
                Console.WriteLine(ex.InnerException);
                throw;
            }
            return View();
        }
    }
}