using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Models
{
    public class LoginModel : TAC_User
    {
        [Display(Name = "Register Here")]
        public string RegisterHere { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public IEnumerable<ProviderDetails> GetProviderNames()
        {
            return OpenAuth.AuthenticationClients.GetAll();
        }
    }
}