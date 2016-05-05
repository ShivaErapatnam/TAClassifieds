using TAClassifieds.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAClassifieds.Models
{
    public class MyAccountClassifieds : TAC_Classified
    {
        public string Location { get; set; }
    }
}