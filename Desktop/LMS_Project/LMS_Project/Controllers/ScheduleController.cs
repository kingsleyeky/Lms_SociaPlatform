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

        public ScheduleController(ILogger<ScheduleController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public  JsonResult GetEvents()
        {

            //var listOfDate = _dbContext.PostMedias.Where(x => x.ScheduledTime != null).ToList();

            //foreach (var date in listOfDate)
            //{
                var events = new List<EventShow>();
            //    events.Add(new EventShow { Id = date.Id, Title = date.Content, StartDate = date.ScheduledTime?.ToString("DD-MMM-YYYY HH:mm a"), EndDate = date.ScheduledTime?.ToString("DD-MMM-YYYY HH:mm a"), ThemeColor = "yellow", Description = date.Content, IsFullDay = false });
            //    return new JsonResult(events);
            //}
            //return new JsonResult(listOfDate);



            events.Add(new EventShow { Id = 2, Title = "Preparation", StartDate = "12-Jun-23", EndDate = "12-Jun-23", Description = "Testing B", IsFullDay = false, ThemeColor = "yellow" });


            events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });
            events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });
            events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });
            events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });
            events.Add(new EventShow { Id = 2, Title = "Election", StartDate = "25-Jun-23", EndDate = "30-Jun-23", Description = "high B", IsFullDay = true, ThemeColor = "blue" });
            return new JsonResult(events);
        }
    }
}