using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.Models.Security;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS_Project.Controllers
{

    [Authorization]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly ISettings _Setting;
        private ApplicationDbContext _dbContext; 
        ServiceResponse res = new ServiceResponse();

        public SettingsController(ILogger<SettingsController> logger, ApplicationDbContext dbContext,ISettings settings)
        {
            _logger = logger;
            _dbContext = dbContext; 
            _Setting= settings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ChangePasswordDTO user,string Email)
        {
            string EmailAdress = HttpContext.Session.GetString("EmailAddress");


            var data =await _Setting.ChangePassWord(user, EmailAdress);
            if(data.success == true)
            {
                TempData["success"] = data.message;
                return View();
            }
            TempData["error"] = data.message;
            return View();
        }


        


    }
}