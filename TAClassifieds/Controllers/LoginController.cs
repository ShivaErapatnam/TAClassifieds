﻿using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TAClassifieds.Models;
using TAClassifieds.Models.DAL;
using System.Data;
using System;
using System.Web;
using System.Collections.Generic;
//using Microsoft.AspNet.Membership.OpenAuth;
//using Microsoft.Web.WebPages.OAuth;

namespace TAClassifieds.Controllers
{
    public class LoginController : Controller
    {
        TAC_Team4Entities db = new TAC_Team4Entities();
        int count = 0;

        public Dictionary<string, int> usersList = new Dictionary<string, int>();

        // GET: Login
        public ActionResult Login(string returnUrl)
        {
            Session["returnUrl"] = returnUrl;
            Session["LockEmailList"] = usersList;
            return View(checkCookie());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                var emailEntered = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault();
                if (emailEntered == null)
                {
                    ViewBag.Message = "Entered Email ID does not exist. Please click on register.";
                }
                else
                {
                    string password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.UPassword, "SHA1");
                    #region To Lock Acccout if more than 3 wrong passwords are entered

                    Dictionary<string, int> myDictionary = (Dictionary<string, int>)Session["LockEmailList"];
                    if (Session["LockEmailList"] != null)
                    {
                        if (!myDictionary.ContainsKey(model.Email))
                        {
                            myDictionary.Add(model.Email, 0);
                        }
                        else
                        {
                            string actualPassword = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault().UPassword;
                            if (!password.Equals(actualPassword) && myDictionary.ContainsKey(model.Email))
                            {
                                count = myDictionary[model.Email];
                                count++;
                                myDictionary[model.Email] = count;
                            }
                        }
                        Session["LockEmailList"] = myDictionary;
                    }

                    #region lock account using cookie

                    //var list = usersList.ToList().Where(x => x.StringData.Equals(model.Email));
                    //if (!model.UPassword.Equals(actualPassword))
                    //    count++;

                    //HttpCookie chkLock = new HttpCookie("lock");
                    //chkLock.Expires = DateTime.Now.AddSeconds(3600);
                    //chkLock.Value = count.ToString();
                    //Response.Cookies.Add(chkLock);

                    //HttpCookie enteredEmail = new HttpCookie("newEmail");
                    //enteredEmail.Expires = DateTime.Now.AddSeconds(3600);
                    //enteredEmail.Value = model.Email;
                    //Response.Cookies.Add(enteredEmail);

                    //if (Request.Cookies["lock"] != null)
                    //{
                    //    string actualPassword = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault().UPassword;
                    //    if (!model.UPassword.Equals(actualPassword) && model.Email.Equals(enteredEmail.Value))
                    //        count++;
                    //    chkLock.Value = count.ToString();
                    //}

                    #endregion

                    var element = db.TAC_User.ToList().Where(
                        x => x.Email.Equals(model.Email) &&
                        x.UPassword.Equals(password)).FirstOrDefault();

                    if (myDictionary[model.Email] >= 3)
                    {
                        Guid id = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault().UserId;
                        var record = db.TAC_User.Find(id);
                        record.IsLocked = true;
                        db.TAC_User.Attach(record);
                        db.Entry(record).Property(e => e.IsLocked).IsModified = true;
                        db.SaveChanges();

                        element = db.TAC_User.ToList().Where(x => x.Email.Equals(model.Email)).FirstOrDefault();
                    }
                    #endregion

                    Guid id1 = element != null ? element.UserId : Guid.Empty;
                    var record1 = db.TAC_User.Find(id1);
                    if (record1 != null)
                    {
                        if (record1.IsVerified == null || record1.IsVerified == false)
                        {
                            ViewBag.Message = "Your account has not been verified yet. Please contact the Administrator";
                        }
                        else if (record1.IsLocked == true)
                        {
                            ViewBag.Message = "You have entered 3 incorrect passwords. So your account has been locked. Please contact the Administrator";
                        }
                        else if (record1.IsActive == false || record1.IsActive == null)
                        {
                            ViewBag.Message = "Your account has been de-activated. Please contact the Administrator";
                        }
                        else
                        {
                            Session["User"] = element;
                            #region Code for "Remember me" checkbox

                            HttpCookie chkEmail = new HttpCookie("email");

                            if (model.RememberMe)
                            {
                                chkEmail.Expires = DateTime.Now.AddSeconds(3600);
                                chkEmail.Value = model.Email;
                                Response.Cookies.Add(chkEmail);
                            }
                            else
                            {
                                if (Response.Cookies["email"] != null)
                                {
                                    chkEmail.Expires = DateTime.Now.AddDays(-1D);
                                    Response.Cookies.Add(chkEmail);
                                }
                            }
                            #endregion
                            // Success, create non-persistent authentication cookie.
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.AddMinutes(5), false, "UserData");
                            HttpCookie cookie1 = new HttpCookie(
                              FormsAuthentication.FormsCookieName,
                              FormsAuthentication.Encrypt(ticket1));
                            Response.Cookies.Add(cookie1);

                            if (Request.QueryString["ReturnUrl"] == null && record1.IsAdmin == true)
                            {
                                Response.Redirect("/Admin/Index");
                            }
                            else if (Request.QueryString["ReturnUrl"] == null && record1.IsAdmin == false && Session["returnUrl"] != null)
                            {
                                Response.Redirect(Session["returnUrl"].ToString());
                            }
                            else
                            {
                                Response.Redirect("/Home/Index");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Please Enter correct Email/Password.";
                    }
                }
            }
            catch(Exception e)
            {
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public LoginModel checkCookie()
        {
            LoginModel l = null;
            string email = string.Empty;
            string pwd = string.Empty;
            if (Request.Cookies["email"] != null)
                email = Request.Cookies["email"].Value;
            if (!String.IsNullOrEmpty(email))
                l = new LoginModel { Email = email };

            return l;
        }

        //public ActionResult ExternalLogin()
        //{           
        //    AuthConfig.RegisterAuth();
        //    IEnumerable<ProviderDetails> model = OpenAuth.AuthenticationClients.GetAll();
        //    return PartialView("_ExternalLogin", model);
        //}
        //public void FBLogin()
        //{

        //}
    }
}