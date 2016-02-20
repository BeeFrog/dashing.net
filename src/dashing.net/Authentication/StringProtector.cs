using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace dashing.net.Authentication
{
    public class StringProtector
    {
        private string Purpose;

        public StringProtector(string purpose)
        {
            this.Purpose = purpose;
        }        

        public string Protect(string unprotectedText)
        {
            var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedText);
            var protectedBytes = MachineKey.Protect(unprotectedBytes, Purpose);
            var protectedText = Convert.ToBase64String(protectedBytes);
            return protectedText;
        }

        public string Unprotect(string protectedText)
        {
            try
            {
                var protectedBytes = Convert.FromBase64String(protectedText);
                var unprotectedBytes = MachineKey.Unprotect(protectedBytes, Purpose);
                var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
                return unprotectedText;
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }
    }
}