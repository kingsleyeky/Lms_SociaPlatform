using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Text;

namespace LMS_Project.Models.Security
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private IHttpContextAccessor httpContextAccessor;

        public CustomCookieAuthenticationEvents(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            //Encrypt Return Url
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(context.Request.Path))
                {
                    string redirectUri = context.RedirectUri;
                    string[] parts = redirectUri.Split("ReturnUrl=");
                    string decodedPath = WebUtility.UrlDecode(parts[1]);
                    var encryptedBase = Encoding.UTF8.GetBytes(decodedPath);
                    string encryptedPath = Convert.ToBase64String(encryptedBase);
                    context.RedirectUri = parts[0] + "ReturnUrl=" + encryptedPath;
                }
            }

            return base.RedirectToLogin(context);
        }

        //public override Task SignedIn(CookieSignedInContext context)
        //{
        //    DateTime expiryDate = DateTime.Now.AddSeconds(context.Options.ExpireTimeSpan.TotalSeconds);
        //    double ttl = TimeSpan.FromTicks(expiryDate.Ticks).TotalMilliseconds;
        //    httpContextAccessor.HttpContext.Session.SetString("TTL", ttl.ToString());
        //    return base.SignedIn(context);
        //}

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                return this.RedirectToLogin(context);
            }
            else
            {
                string[] parts = context.RedirectUri.Split("AzureAD");
                context.RedirectUri = parts[0] + "Home/RestrictedAccess";
                return base.RedirectToAccessDenied(context);
            }
        }
    }
}
