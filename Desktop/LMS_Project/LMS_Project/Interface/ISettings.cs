using LMS_Project.Models;
using LMS_Project.ViewModel;

namespace LMS_Project.Interface
{
    public interface ISettings
    {
        Task<ServiceResponse> ChangePassWord(ChangePasswordDTO changePassword, string userEmail);
    }
}
