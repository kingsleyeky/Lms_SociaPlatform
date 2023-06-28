using LMS_Project.Interface;
using LMS_Project.Models;
using LMS_Project.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_Project.Repositories
{
    public class ScheduleRepo:ISchedule
    {

        public ApplicationDbContext _dbContext { get; set; }
        ServiceResponse res = new ServiceResponse();

        public ScheduleRepo(ApplicationDbContext context)

        {
            _dbContext = context;

        }
        public async Task<ServiceResponse> Edit(EventShow eventShow)
        {
            var update = await _dbContext.PostMedias.Where(x => x.Title == eventShow.Title).FirstOrDefaultAsync();
            if (update != null)
            {
               
                update.Content = eventShow.Content;
                _dbContext.Entry(update).State = EntityState.Modified;
                int result = await _dbContext.SaveChangesAsync();
                if (result > 0)
                {

                    res.message = "updated successfully";
                    res.Data = update;
                    res.success = true;
                    return res;
                }

                res.message = "update Failed ";
                res.Data = update;
                res.success = false;
                return res;
            }

            else
            {
                res.success = false;
                res.message = "Does not exist";
                res.Data = null;
                return res;
            }
        }
    }
}
