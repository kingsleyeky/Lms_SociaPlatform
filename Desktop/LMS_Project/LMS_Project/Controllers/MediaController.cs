using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.Models.Security;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS_Project.Controllers
{

    [Authorization]
    public class MediaController : Controller
    {
        private readonly ILogger<MediaController> _logger;

        private readonly IMedia _media;
        private readonly IHttpContextAccessor _contextAccessor;

        public MediaController(ILogger<MediaController> logger, IMedia media, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _media = media;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> GetPostForm()
        {
            ViewData["Platforms"] = await _media.GetPlatform();
            return View();
        }

        public async Task<IActionResult> Index()
        {
            string email = HttpContext.Session.GetString("EmailAddress");

            var data = await _media.Index();
            if (data != null)
            {

                return View(data);
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent(List<IFormFile> image, PostMediaDTO media)
        {
            string email = HttpContext.Session.GetString("EmailAddress");
            var check = await _media.CreateContent(image, media, email);
            if (check.success == false)
            {
                TempData["error"] = check.message;
                return RedirectToAction("Index", "Media");
            }
            TempData["success"] = check.message;
            return RedirectToAction("Index", "Media");
        }

        [HttpGet]
        public async Task <IActionResult> Edit(int? Id)
        {

            var existingData = await _media.EditByID(Id); 
            if (existingData != null)
            {
                return View(existingData);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(PostMediaDTO account)
        {
            if (account != null)
            {
               

                var result = await _media.Edit(account);
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
            if (Id > 0)
            {
                var result = await _media.DeleteById(Id);
                if (result != null)
                {
                    return RedirectToAction("Index", "Media");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }




    }
}