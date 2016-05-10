using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAClassifieds.Models;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        TAC_Team4Entities dbContext = new TAC_Team4Entities();
        public ActionResult Index(int? categoryID, int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            ViewBag.CategoryId = categoryID;
            int pageSize = 3;
            var lstClassifieds = new List<MyAccountClassifieds>();
            int pagecount = 0;

            if (categoryID == null)
            {
                lstClassifieds = ClassifiedApi.GetAllPosts().OrderBy(x => x.PostedDate).Skip(((int)pageNumber - 1) * pageSize).Take((int)pageSize).ToList();
                pagecount = (int)Math.Ceiling((decimal)ClassifiedApi.GetAllPosts().ToList().Count / (decimal)pageSize);
                //  var pager = new Pager(ClassifiedApi.GetAllPosts().ToList().Count, pageNumber);

            }
            else
            {
                lstClassifieds = ClassifiedApi.GetAllPosts().Where(x => x.CategoryId == categoryID).OrderBy(x => x.PostedDate).Skip(((int)pageNumber - 1) * pageSize).Take((int)pageSize).ToList();
                pagecount = (int)Math.Ceiling((decimal)ClassifiedApi.GetAllPosts().Where(x => x.CategoryId == categoryID).ToList().Count / (decimal)pageSize);
            }

            var model = new ViewAds
            {
                pageCount = pagecount,
                lst = lstClassifieds,
                lstCategory = ClassifiedApi.GetAllCategory()
            };
            return View(model);
        }
    }
}