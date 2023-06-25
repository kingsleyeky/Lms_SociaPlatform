using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace LMS_Project.ViewModel
{
    public class AccountDTO
    {
        public int PlatformId { get; set; }
        public string? Name { get; set; }
        public string? PlatformName { get; set; }
    }
}
