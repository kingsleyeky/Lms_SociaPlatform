namespace LMS_Project.ViewModel
{
    public class AccountMediaDTO
    {
        public string? Name { get; set; }
        public string? PlatformName { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Linkdein { get; set; }
        public int Id { get; set; }
        public string? Content { get; set; }
        //[Required]
        public string? VideoLink { get; set; }

       // [Required]
        public DateTime ScheduledTime { get; set; }
    }
}
