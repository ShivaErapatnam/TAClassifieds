using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAClassifieds.Models;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        TAC_Team4Entities dbContext = new TAC_Team4Entities();
        public ActionResult Index(int? categoryID, int? pageNumber)
        {           
            ViewBag.CategoryId = categoryID;
            int pageSize = 3;
            var lstClassifieds = new List<MyAccountClassifieds>();
            int pagecount = 0;
            ViewAds viewAdsModel = new ViewAds();
            viewAdsModel.nextButton = 2;
            viewAdsModel.prevButton = 1;
            if (pageNumber == null || pageNumber == 1)
            {
                pageNumber = 1;
                viewAdsModel.nextButton = 2; viewAdsModel.prevButton = 1;
            }

            if (categoryID == null)
            {
                lstClassifieds = GetMyAccountClassifiedFromPosts(ClassifiedApi.GetAllPosts()).OrderBy(x => x.PostedDate).Skip(((int)pageNumber - 1) * pageSize).Take((int)pageSize).ToList();
                pagecount = (int)Math.Ceiling((decimal)ClassifiedApi.GetAllPosts().ToList().Count / (decimal)pageSize);
                //  var pager = new Pager(ClassifiedApi.GetAllPosts().ToList().Count, pageNumber);

            }
            else
            {
                lstClassifieds = GetMyAccountClassifiedFromPosts(ClassifiedApi.GetAllPosts()).Where(x => x.CategoryId == categoryID).OrderBy(x => x.PostedDate).Skip(((int)pageNumber - 1) * pageSize).Take((int)pageSize).ToList();
                pagecount = (int)Math.Ceiling((decimal)ClassifiedApi.GetAllPosts().Where(x => x.CategoryId == categoryID).ToList().Count / (decimal)pageSize);
            }
            if (pageNumber == pagecount)
            {
                viewAdsModel.nextButton = pagecount;
                if (pagecount == 1) viewAdsModel.prevButton = 1;
                else viewAdsModel.prevButton = Convert.ToInt32(pageNumber) - 1;
            }
            else
            {
                viewAdsModel.nextButton = Convert.ToInt32(pageNumber) + 1;
                if (pageNumber == 1)
                {
                    viewAdsModel.prevButton = Convert.ToInt32(pageNumber);
                }
                else
                {
                    viewAdsModel.prevButton = Convert.ToInt32(pageNumber) - 1;
                }

            }

            viewAdsModel.pageCount = pagecount;
            viewAdsModel.lst = lstClassifieds;
            viewAdsModel.lstCategory = ClassifiedApi.GetAllCategory();
           
            return View(viewAdsModel);
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
    }
}