namespace LMS_Project.ViewModel
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
