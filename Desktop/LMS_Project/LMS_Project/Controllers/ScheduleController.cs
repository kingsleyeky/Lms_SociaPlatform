using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.Models.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace LMS_Project.Controllers
{
    [Authorization]
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly ISchedule _schedule;

        public ScheduleController(ILogger<ScheduleController> logger, ApplicationDbContext dbContext,ISchedule schedule)
        {
            _logger = logger;
            _dbContext = dbContext; 
            _schedule = schedule;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  JsonResult GetEvents()
        {
            var listOfDate = _dbContext.PostMedias.Where(x => x.ScheduledTime != null).ToList();
            var events = new List<EventShow>();
            foreach (var date in listOfDate)
            {
               
                events.Add(new EventShow { Id = date.Id, Title = date.Title, StartDate = date.ScheduledTime?.ToString("dd-MMM-yy"), EndDate = date.ScheduledTime?.ToString("dd-MMM-yy"), ThemeColor = "blue", Description = date.Content, IsFullDay = true });
               // return  Json(events);
            }
            return new JsonResult(events);




            //var events = new List<EventShow>();


            //events.Add(new EventShow { Id = 2, Title = "Preparation", StartDate = "12-Jun-23", EndDate = "12-Jun-23", Description = "Testing B", IsFullDay = false, ThemeColor = "yellow" });


            //events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" }); 


            //events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });

            //events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });

            //events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });

            //events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });
            //return new JsonResult(events);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EventShow eventShow)
        {
            if (eventShow != null)
            {


                var result = await _schedule.Edit(eventShow);
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

    }
}