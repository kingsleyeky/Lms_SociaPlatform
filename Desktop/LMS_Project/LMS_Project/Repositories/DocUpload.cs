using Azure;
using LMS_Project.Interface;
using LMS_Project.Models;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;

namespace LMS_Project.Repositories
{
    public class DocUpload:IDocUpload
    {
        private readonly IWebHostEnvironment _env;

        public DocUpload(IWebHostEnvironment env)
        {
            _env= env;
        } 

        ServiceResponse res = new ServiceResponse();
        private (string, string) GetContentPath(string systemPath, string title, FileInfo newFileName)
        {
            try
            {
                string filePath = systemPath;
                var path = Path.Combine(_env.WebRootPath, filePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var uniqueNo = new Random().Next(0, 100000);

                var pathName = title.Replace(" ", "_").Replace("-", "_").Replace("'", "_") + "_" + uniqueNo + newFileName;
                var defaultPath = Path.Combine(filePath, pathName);
                string imagePath = Path.Combine(_env.WebRootPath, defaultPath);
                
                return (defaultPath, imagePath);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceResponse> ImageUpload(List<IFormFile> image, string systemPath, string title)
        {
            try
            {

                ImageFile imageFile = new ImageFile();

                foreach (IFormFile file in image)
                {

                    FileInfo fi = new FileInfo(file.FileName);

                    var newFilename = fi;

                    var (defaultPath, imagePath) = GetContentPath("Images", title, newFilename);

                    var stream = new FileStream(imagePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    imageFile.FileTitle = title;
                    imageFile.FileCode = "";
                    imageFile.FileTitle = file.FileName;
                    imageFile.ImagePath = '/' + defaultPath.Replace("\\", "/");

                    StringContent content = new StringContent(JsonConvert.SerializeObject(imageFile), Encoding.UTF8, "application/json");

                }
                res.Data = imageFile.ImagePath;
                res.success= true;
                return res;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
    }
}
