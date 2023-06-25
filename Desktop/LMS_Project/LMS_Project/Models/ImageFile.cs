namespace LMS_Project.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string? ImagePath { get; set; }
        public string? UserEmail { get; set; }
        public string? FileCode { get; set; }
        public string? FileTitle { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
