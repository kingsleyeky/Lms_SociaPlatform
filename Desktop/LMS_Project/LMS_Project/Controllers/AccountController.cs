using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.Models.Security;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace LMS_Project.Controllers
{
    [Authorization]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccount _account;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountController(ILogger<AccountController> logger,IAccount account, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _account = account;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        ServiceResponse res = new ServiceResponse();
        public async Task< IActionResult> Index()
        {  

            string email = HttpContext.Session.GetString("EmailAddress");

            var data = await _account.Index();
            if (data != null)
            { 

                return View(data);
            }


            return View();
        }

        [HttpGet]
        public  IActionResult Create()
        {
            //var data = await _account.Get();


            //if (data != null)
            //{
               
            //    ViewBag.da = new SelectList(data, "PostId", "Name");
            //    return View();
            //}
            return View();

            
        }
        [HttpPost]
        public async Task<IActionResult> CreatePlatform(AccountDTO platform)
        {
            string email = HttpContext.Session.GetString("EmailAddress");
            var data = await _account.SavePlatform(platform,email); 

            if (data != null)
            {
                TempData["success"] = data.message;
                return RedirectToAction("Index", "Account");
            }
            TempData["error"] = data.message;
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }


        public async Task<IActionResult> Edit(AccountDTO account, int PlatformId)
        {
            if(account != null)
            {
                var result = await _account.Edit(account);
                if (result.success == true)
                {
                    TempData["success"] = result.message;
                    return RedirectToAction("Index", "Account");
                }
                TempData["error"] = result.message;
                return RedirectToAction("Index");
            }
            TempData["error"] = "Validation Error!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id>0)
            {
                var result = await _account.DeleteById(Id);
                if (result != null)
                {
                    TempData["success"] = result.message;
                    return RedirectToAction("Index", "Media");
                }
                TempData["error"] = result.message;
                return RedirectToAction("Index");
            }
            TempData["error"] = "Validation Error!";
            return RedirectToAction("Index");
        }

    }
}