﻿using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TAClassifieds.Models;
using TAClassifieds.Models.DAL;
using System.Data;
using System;
using System.Web;

namespace TAClassifieds.Controllers
{
    public class LoginController : Controller
    {
        TAC_Team4Entities db = new TAC_Team4Entities();
        static int count = 0;
        // GET: Login
        public ActionResult Login()
        {
            return View(checkCookie());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            #region To Lock Acccout if more than 3 wrong passwords are entered

            HttpCookie chkLock = new HttpCookie("lock");
            chkLock.Expires = DateTime.Now.AddSeconds(3600);
            chkLock.Value = count.ToString();
            Response.Cookies.Add(chkLock);

            HttpCookie enteredEmail = new HttpCookie("newEmail");
            enteredEmail.Expires = DateTime.Now.AddSeconds(3600);
            enteredEmail.Value = model.Email;
            Response.Cookies.Add(enteredEmail);

            if (Request.Cookies["lock"] != null)
            {
                string actualPassword = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault().UPassword;
                if (!model.UPassword.Equals(actualPassword) && model.Email.Equals(enteredEmail.Value))
                    count++;
                chkLock.Value = count.ToString();
            }

            if (count >= 3)
            {
                Guid id = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault().UserId;
                var record = db.TAC_User.Find(id);
                record.IsLocked = true;
                db.Entry(record).Property(e => e.IsLocked).IsModified = true; ;
                db.SaveChanges();
            }
            #endregion

            var element = db.TAC_User.ToList().Where(
                x => x.Email.Equals(model.Email) &&
                x.UPassword.Equals(model.UPassword)).FirstOrDefault();

            Guid id1 = element != null ? element.UserId : Guid.Empty;
            var record1 = db.TAC_User.Find(id1);
            if (record1 != null)
            {
                if (record1.IsVerified == false)
                {
                    ViewBag.Message = "Your account has been de-activated. Please contact the Administrator";
                }

                else if (record1.IsLocked == true)
                {
                    ViewBag.Message = "You have entered 3 incorrect passwords. So your account has been locked. Please contact the Administrator";
                }

                else if (record1.IsActive == false)
                {
                    ViewBag.Message = "Your account has not been verified yet. Please contact the Administrator";
                }
                else
                {
                    Session["User"] = model;
                    #region Code for "Remember me" checkbox

                    HttpCookie chkEmail = new HttpCookie("email");

                    HttpCookie chkPwd = new HttpCookie("pwd");
                    if (model.RememberMe)
                    {
                        chkEmail.Expires = DateTime.Now.AddSeconds(3600);
                        chkEmail.Value = model.Email;
                        Response.Cookies.Add(chkEmail);

                        chkPwd.Expires = DateTime.Now.AddSeconds(3600);
                        chkPwd.Value = model.UPassword;
                        Response.Cookies.Add(chkPwd);
                    }
                    else
                    {
                        if (Response.Cookies["email"] != null)
                        {
                            chkEmail.Expires = DateTime.Now.AddDays(-1D);
                            Response.Cookies.Add(chkEmail);
                        }

                        if (Response.Cookies["pwd"] != null)
                        {
                            chkPwd.Expires = DateTime.Now.AddDays(-1D);
                            Response.Cookies.Add(chkPwd);
                        }
                    }
                    #endregion

                    Response.Redirect("/Home/Index");
                }
            }
            else
            {
                ViewBag.Message = "Please Enter correct Email/Password.";
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }
        public LoginModel checkCookie()
        {
            LoginModel l = null;
            string email = string.Empty;
            string pwd = string.Empty;
            if (Request.Cookies["email"] != null)
                email = Request.Cookies["email"].Value;
            if (Request.Cookies["pwd"] != null)
                pwd = Request.Cookies["pwd"].Value;
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(pwd))
                l = new LoginModel { Email = email, UPassword = pwd };

            return l;
        }
    }
}