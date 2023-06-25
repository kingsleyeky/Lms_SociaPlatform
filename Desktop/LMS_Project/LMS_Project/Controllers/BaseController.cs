using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS_Project.Controllers
{
    public class BaseController : Controller
    {
        private readonly HttpContext context;

        public BaseController(HttpContext context)
        {
            this.context = context;
        }

        [NonAction]
        public string GetUserId()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            if (claimsIdentity.FindFirst("EmailAddress") != null)
            {
                return claimsIdentity.FindFirst("EmailAddress").Value.ToString();
            }
            return string.Empty;
        }


        [NonAction]
        public string GetRole()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            var role = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).SingleOrDefault();

            return role.Value.ToString();
        }
        [NonAction]
        public string GetToken()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            if (claimsIdentity.FindFirst("Token") != null)
            {
                return claimsIdentity.FindFirst("Token").Value.ToString();
            }
            return string.Empty;
        }

    }
}