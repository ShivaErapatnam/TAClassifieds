using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAClassifieds.Models.DAL;
namespace TAClassifieds.Models
{
    public class PostAdModel
    {
        public TAClassifieds.Models.DAL.TAC_ClassifiedContact User { get; set; }
        public TAClassifieds.Models.DAL.TAC_Classified Classified { get; set; }
    }
}