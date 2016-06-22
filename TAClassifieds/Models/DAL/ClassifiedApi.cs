using TAClassifieds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAClassifieds.Models.DAL
{
    public class ClassifiedApi
    {
        private static TAC_Team4Entities dbContext = new TAC_Team4Entities();
        public static List<TAC_Classified> GetAllPosts()
        {
            //assuming user ,Once login implemented, we could take user from session
            List<TAC_Classified> lstAds = new List<TAC_Classified>();
            //Guid userId = new Guid("1976511D-DAF5-4F27-BBE0-29305E5C4E99");
            //lstAds = dbContext.TAC_Classified.Where(x => x.CreatedBy.ToString().ToLower() == userId.ToString().ToLower()).ToList();
            lstAds = dbContext.TAC_Classified.ToList();
           
            return lstAds;
        }

        public static TAC_ClassifiedContact GetContactByClassified(int classifiedId)
        {
            TAC_ClassifiedContact classifiedContact = dbContext.TAC_ClassifiedContact.Where(x => x.ClassifiedId == classifiedId).FirstOrDefault();
            return classifiedContact;
        }

        public static MyAccountClassifieds GetClassifiedById(int classifiedId)
        {
            TAC_Classified classified = new TAC_Classified();
            if (classifiedId != 0)
            {
                classified = dbContext.TAC_Classified.Where(x => x.ClassifiedId == classifiedId).FirstOrDefault();
                return new MyAccountClassifieds()
                {
                    CategoryId = classified.CategoryId,
                    ClassifiedId = classified.ClassifiedId,
                    ClassifiedImage = classified.ClassifiedImage,
                    ClassifiedPrice = classified.ClassifiedPrice,
                    ClassifiedTitle = classified.ClassifiedTitle,
                    CreatedBy = classified.CreatedBy,
                    Description = classified.Description,
                    PostedDate = classified.PostedDate,
                    Summary = classified.Summary,
                    Location = ((GetContactByClassified(classified.ClassifiedId) == null) ? string.Empty : GetContactByClassified(classified.ClassifiedId).ContactCity)
                };
            }
            return null;
        }

        public static List<TAC_Category> GetAllCategory()
        {

            return dbContext.TAC_Category.ToList();
        }

    }
}