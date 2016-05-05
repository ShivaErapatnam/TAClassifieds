using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAClassifieds.Models
{
    public class ViewAds
    {
        public List<TAClassifieds.Models.MyAccountClassifieds> lst { get; set; }

        public List<TAClassifieds.Models.DAL.TAC_Category> lstCategory { get; set; }

       // public Pager Pager { get; set; }
        public int pageCount { get; set; }
    }
}