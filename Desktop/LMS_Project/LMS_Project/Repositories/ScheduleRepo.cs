using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Project.Repositories
{
    public class ScheduleRepo:ISchedule
    {

        public string today = DateTime.Today.ToString();

        public ApplicationDbContext _Context { get; set; }
       
      
        public EventShow Events { get; set; }

        public JsonResult result { get; set; }

        public ScheduleRepo(ApplicationDbContext context)

        {
            _Context = context;

        }
        ////public void OnGet()
        ////{
        ////    ViewData["events"] = new[]
        ////    {
        ////        new EventShow { Id = 1, Title = "Video for Marisa", StartDate = "2023-06-14"},
        ////        new EventShow { Id = 2, Title = "Preparation", StartDate = "2023-06-12"},
        ////    };
        ////}
    }
}
