using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Models
{
    public class ViewAds
    {
        public List<MyAccountClassifieds> lst { get; set; }

        public List<TAC_Category> lstCategory { get; set; }

       // public Pager Pager { get; set; }
        public int pageCount { get; set; }
        public int prevButton { get; set; }
        public int nextButton { get; set; }
    }
}