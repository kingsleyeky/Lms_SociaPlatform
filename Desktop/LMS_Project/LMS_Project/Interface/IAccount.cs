using LMS_Project.Models;
using LMS_Project.ViewModel;

namespace LMS_Project.Interface
{
    public interface IAccount
    {
        Task<List<Account>> Get();
        Task<ServiceResponse> SavePlatform(AccountDTO platform, string Email);
        Task<List<AccountDTO>> Index();
        Task<ServiceResponse> Edit(AccountDTO account);
        Task<ServiceResponse> DeleteById(int Id);
    }
}
