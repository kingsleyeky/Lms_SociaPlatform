using LMS_Project.Models;
using LMS_Project.ViewModel;

namespace LMS_Project.Interface
{
    public interface IHome
    {
        Task<ServiceResponse> Register(RegisterDTO user);
        Task<ServiceResponse> Login(LoginDTO userdto);
        Task<bool> IsEmailExist(string Email);
    }
}
