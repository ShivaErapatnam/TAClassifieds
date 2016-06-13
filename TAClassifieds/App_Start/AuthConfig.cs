using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Microsoft.AspNet.Membership.OpenAuth;

namespace TAClassifieds
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "109703303033517",
            //    appSecret: "3e89661f786583061958f203c5d87af8");

            //OAuthWebSecurity.RegisterGoogleClient();

            OpenAuth.AuthenticationClients.AddFacebook(
                appId: "109703303033517",
                appSecret: "3e89661f786583061958f203c5d87af8");

            //Dictionary<string, object> FacebooksocialData = new Dictionary<string, object>();
            //FacebooksocialData.Add("Icon", "~?Resources/images/fb.png");
            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "someid",
            //    appSecret: "somesecret",
            //    displayName: "Facebook",
            //    extraData: FacebooksocialData);
        }
    }
}
