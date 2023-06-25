using LMS_Project.Models;

namespace LMS_Project.Interface
{
    public interface IDocUpload
    {
        Task<ServiceResponse> ImageUpload(List<IFormFile> image, string systemPath, string title);
    }
}
