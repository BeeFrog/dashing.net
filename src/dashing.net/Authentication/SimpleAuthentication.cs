using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace dashing.net.Authentication
{
    /// <summary>
    /// This is a really basic and rubish authentication method.
    /// You should not use this in production and implement your own security.
    /// </summary>
    public class SimpleAuthentication : IAuthenticater
    {
        private const string cookieid = "authToken";
        private string authID;

        StringProtector protector = new StringProtector("SimpleAuthentication");

        public System.Security.Principal.IPrincipal GetAuthenticatedUser(HttpRequest request)
        {
            var cookie = request.Cookies[cookieid];
            var cookieValue = cookie?.Value;
            if(!string.IsNullOrWhiteSpace(cookieValue))
            {
                cookieValue = protector.Unprotect(cookieValue);
                if (cookieValue == this.AuthToken())
                {
                    var user = new BasicIdentity() { IsAuthenticated = true, Name = "AuthToken" };
                    return user;
                }
            }

            return null;
        }

        internal bool Validate(NameValueCollection form)
        {
            var authcode = form["authCode"];
            return authcode == this.AuthToken();
        }

        public string AuthToken()
        {
            if (string.IsNullOrWhiteSpace(this.authID))
            {
                authID = System.Configuration.ConfigurationManager.AppSettings.GetValues("AuthToken")?.FirstOrDefault();
            }

            return authID;
        }

        public void AuthenticateUser(HttpResponseBase response)
        {
            var token = protector.Protect(this.AuthToken());
            response.SetCookie(new HttpCookie(cookieid, token));
        }
    }
}