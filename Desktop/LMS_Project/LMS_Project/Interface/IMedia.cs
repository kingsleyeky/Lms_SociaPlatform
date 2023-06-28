using LMS_Project.Models;
using LMS_Project.ViewModel;

namespace LMS_Project.Interface
{
    public interface IMedia
    {
        Task<ServiceResponse> CreateContent(List<IFormFile> image, PostMediaDTO mediaDTO, string Email);
        Task<ServiceResponse> Edit(PostMediaDTO account);
        Task<List<PostMedia>> Index();
        Task<ServiceResponse> DeleteById(int Id);
        Task<List<Tbl_Platform>> GetPlatform();
        Task<PostMedia> EditByID(int? Id);
    }
}
