using TAClassifieds.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAClassifieds.Models
{
    public class MyAccountModel
    {
        public List<MyAccountClassifieds> myAccountClassifieds;        

        public List<TAClassifieds.Models.DAL.TAC_Category> lstCategory { get; set; }

        // public Pager Pager { get; set; }
        public int pageCount { get; set; }
        public int prevButton { get; set; }
        public int nextButton { get; set; }
    }
}