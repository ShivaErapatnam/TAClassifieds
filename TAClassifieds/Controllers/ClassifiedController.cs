using TAClassifieds.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Controllers
{
    [Authorize]
    public class ClassifiedController : Controller
    {
        #region Global Variables
        TAC_Team4Entities dbContext = new TAC_Team4Entities();
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
            return View();
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
                    postAdModel.Classified.CategoryId = (int)Session["categoryID"];
                    TAC_User model = (TAC_User)Session["User"];
                    postAdModel.Classified.CreatedBy = model.UserId;


                    //fileupload logic
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("../Resources/Uploaded_Images/"), fileName);
                            file.SaveAs(path);
                            postAdModel.Classified.ClassifiedImage = Path.Combine("../Resources/Uploaded_Images/", fileName);
                        }
                    }

                    dbContext.TAC_Classified.Add(postAdModel.Classified);
                    dbContext.SaveChanges();
                    postAdModel.User.ClassifiedId = postAdModel.Classified.ClassifiedId;
                    dbContext.TAC_ClassifiedContact.Add(postAdModel.User);
                    dbContext.SaveChanges();

                    return RedirectToAction("PostAd");
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public ActionResult MyAccount()
        {
            MyAccountModel myAccountModel = new MyAccountModel();
            myAccountModel.myAccountClassifieds = ClassifiedApi.GetAllPosts();
            return View(myAccountModel);
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
    }
}
