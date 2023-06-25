using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LMS_Project.Models.Security
{
    public class Authorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string EmailAddress = /*"kingsly.ozoemena@gmail.com";*/ Convert.ToString(context.HttpContext.Session.GetString("EmailAddress")); 

            if(EmailAddress == null)
            {
                RedirectToActionResult result = new RedirectToActionResult("Login","Home",null);

                context.Result= result;
            }
        }
    }
}
