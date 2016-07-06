using TAClassifieds.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAClassifieds.Models.DAL;
using NLog;

namespace TAClassifieds.Controllers
{
    [Authorize]
    public class ClassifiedController : Controller
    {
        #region Global Variables
        TAC_Team4Entities dbContext = new TAC_Team4Entities();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion  

        // GET: Classified
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Index1()
        //{

        //    return View(ClassifiedApi.GetAllPosts());
        //}

        // GET: Classified/Create
        public ActionResult PostAd(int? categoryID)
        {
            Session["categoryID"] = categoryID;

            if (Session["User"] == null)
                return RedirectToAction("Login", "Login");

            PostAdModel postAdModel = new PostAdModel();
            postAdModel.User = new TAC_ClassifiedContact();
            TAC_User model = (TAC_User)Session["User"];
            var record = dbContext.TAC_User.Find(model.UserId);
            postAdModel.User.ContactName = record.First_Name + " " + record.Last_Name;
            postAdModel.User.ContactPhone = record.Phone;
            postAdModel.User.ContactCity = record.City;
            return View(postAdModel);
        }

        // POST: Classified/Create
        [HttpPost]
        public ActionResult PostAd(PostAdModel postAdModel, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    postAdModel.Classified.Summary = postAdModel.Classified.Description;
                    postAdModel.Classified.PostedDate = DateTime.Now;
                    if (Session["User"] != null)
                    {
                        TAC_User model = (TAC_User)Session["User"];
                        postAdModel.Classified.CreatedBy = model.UserId;
                    }
                    else return View();
                    if (Session["categoryID"] != null)
                    {
                        postAdModel.Classified.CategoryId = (int)Session["categoryID"];
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryId", "Please select category");
                        return View(model: postAdModel);
                    }
                    //fileupload logic
                    if (Request.Files.Count > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //3 MB
                        var file = Request.Files[0];

                        if (file != null && file.ContentLength > 0 && file.ContentLength < MaxContentLength)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("../Resources/Uploaded_Images/"), fileName);
                            file.SaveAs(path);
                            postAdModel.Classified.ClassifiedImage = Path.Combine("../Resources/Uploaded_Images/", fileName);
                        }
                        else
                        {
                            if (file.ContentLength > MaxContentLength)
                            {
                                ViewBag.Message = "Please upload an image less 3MB ";
                            }
                        }
                    }

                    dbContext.TAC_Classified.Add(postAdModel.Classified);
                    dbContext.SaveChanges();
                    postAdModel.User.ClassifiedId = postAdModel.Classified.ClassifiedId;
                    dbContext.TAC_ClassifiedContact.Add(postAdModel.User);
                    dbContext.SaveChanges();

                    return RedirectToAction("MyAccount");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Sorry, your data was not saved.";
            }
            return View(postAdModel);
        }

        public ActionResult MyAccount(int? categoryID, int? pageNumber)
        {
            Logs();
            MyAccountModel myAccountModel = new MyAccountModel();
            TAC_User model = new TAC_User();
            if (Session["User"] != null)
            {
                model = (TAC_User)Session["User"];
            }
            else
            {
                ModelState.AddModelError("User", "Please Login to continue");
                return View();
            }

            int totalPageCount = 0;
            myAccountModel.nextButton = 2;
            myAccountModel.prevButton = 1;

            ViewBag.CategoryId = categoryID;
            int pageSize = 3;
            var lstClassifieds = new List<MyAccountClassifieds>();
            int pagecount = 0;
            if (pageNumber == null || pageNumber == 1)
            {
                pageNumber = 1;
                myAccountModel.nextButton = 2; myAccountModel.prevButton = 1;
            }
            if (categoryID == null)
            {
                lstClassifieds = GetMyAccountClassifiedFromPosts(ClassifiedApi.GetAllPosts().Where(x => x.CreatedBy.ToString().ToLower() == model.UserId.ToString().ToLower()).ToList()).OrderBy(x => x.PostedDate).Skip(((int)pageNumber - 1) * pageSize).Take((int)pageSize).ToList();
                pagecount = (int)Math.Ceiling((decimal)ClassifiedApi.GetAllPosts().Where(x => x.CreatedBy.ToString().ToLower() == model.UserId.ToString().ToLower()).ToList().Count / (decimal)pageSize);
            }
            else
            {
                lstClassifieds = GetMyAccountClassifiedFromPosts(ClassifiedApi.GetAllPosts().Where(x => x.CreatedBy.ToString().ToLower() == model.UserId.ToString().ToLower()).ToList()).Where(x => x.CategoryId == categoryID).OrderBy(x => x.PostedDate).Skip(((int)pageNumber - 1) * pageSize).Take((int)pageSize).ToList();
                pagecount = (int)Math.Ceiling((decimal)ClassifiedApi.GetAllPosts().Where(x => x.CreatedBy.ToString().ToLower() == model.UserId.ToString().ToLower()).ToList().Where(x => x.CategoryId == categoryID).ToList().Count / (decimal)pageSize);
            }
            //if (lstClassifieds.Count % 3 == 0)
            //{
            //    totalPageCount = lstClassifieds.Count / 3;
            //}
            //else
            //{
            //    totalPageCount = (lstClassifieds.Count / 3) + 1;
            //}

            if (pageNumber == pagecount)
            {
                myAccountModel.nextButton = pagecount;
                if (pagecount == 1) myAccountModel.prevButton = 1;
                else myAccountModel.prevButton = Convert.ToInt32(pageNumber) - 1;
            }
            else
            {
                myAccountModel.nextButton = Convert.ToInt32(pageNumber) + 1;
                if (pageNumber == 1)
                {
                    myAccountModel.prevButton = Convert.ToInt32(pageNumber);
                }
                else
                {
                    myAccountModel.prevButton = Convert.ToInt32(pageNumber) - 1;
                }

            }


            myAccountModel.myAccountClassifieds = lstClassifieds;
            myAccountModel.pageCount = pagecount;
            myAccountModel.lstCategory = ClassifiedApi.GetAllCategory();
            return View(myAccountModel);
        }

        public List<MyAccountClassifieds> GetMyAccountClassifiedFromPosts(List<TAC_Classified> lstTAcClassified)
        {
            List<MyAccountClassifieds> lstClassifieds = new List<MyAccountClassifieds>();
            foreach (var item in lstTAcClassified)
            {

                lstClassifieds.Add(new MyAccountClassifieds()
                {
                    CategoryId = item.CategoryId,
                    ClassifiedId = item.ClassifiedId,
                    ClassifiedImage = item.ClassifiedImage,
                    ClassifiedPrice = item.ClassifiedPrice,
                    ClassifiedTitle = item.ClassifiedTitle,
                    CreatedBy = item.CreatedBy,
                    Description = item.Description,
                    PostedDate = item.PostedDate,
                    Summary = item.Summary,
                    Location = ((ClassifiedApi.GetContactByClassified(item.ClassifiedId) == null) ? string.Empty : ClassifiedApi.GetContactByClassified(item.ClassifiedId).ContactCity)
                });
            }
            return lstClassifieds;
        }

        public ActionResult ViewDetail(int? classifiedId)
        {
            int id;
            MyAccountClassifieds classified = ClassifiedApi.GetClassifiedById((int.Parse(classifiedId.ToString()) != 0) ? int.Parse(classifiedId.ToString()) : 1);
            if (classified != null)
            {
                return View(classified);
            }
            else return View("MyAccount");

        }

        public ActionResult CategoriesList()
        {
            IEnumerable<TAC_Category> model = dbContext.TAC_Category;
            return PartialView("_CategoriesList", model);
        }

        public void Logs()
        {
            int k = 42;
            int l = 100;

            logger.Trace("Sample trace message, k={0}, l={1}", k, l);
            logger.Debug("Sample debug message, k={0}, l={1}", k, l);
            logger.Info("Sample informational message, k={0}, l={1}", k, l);
            logger.Warn("Sample warning message, k={0}, l={1}", k, l);
            logger.Error("Sample error message, k={0}, l={1}", k, l);
            logger.Fatal("Sample fatal error message, k={0}, l={1}", k, l);
            logger.Log(LogLevel.Info, "Sample informational message, k={0}, l={1}", k, l);
        }
    }
}
