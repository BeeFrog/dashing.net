using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dashing.net.Authentication
{
    public interface IAuthenticater
    {
        System.Security.Principal.IPrincipal GetAuthenticatedUser(HttpRequest request);
    }
}