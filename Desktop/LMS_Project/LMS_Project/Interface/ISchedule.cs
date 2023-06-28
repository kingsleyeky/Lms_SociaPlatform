using LMS_Project.Models;

namespace LMS_Project.Interface
{
    public interface ISchedule
    {
        Task<ServiceResponse> Edit(EventShow eventShow);
    }
}
