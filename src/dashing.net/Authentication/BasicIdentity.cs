using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace dashing.net.Authentication
{
    public class BasicIdentity : System.Security.Principal.IPrincipal, System.Security.Principal.IIdentity
    {
        public string AuthenticationType
        {
            get
            {
                return "VeryBasic"; 
            }
        }

        public IIdentity Identity
        {
            get
            {
                return this;
            }
        }

        public bool IsAuthenticated { get; set; }
        
        public string Name { get; set; }


        public bool IsInRole(string role)
        {
            return true;
        }
    }
}