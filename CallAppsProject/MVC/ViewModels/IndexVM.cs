using SharedLibrary.DTOs;

namespace MVC.ViewModels
{
    public class IndexVM
    {
        public List<IndexUserProfileVM>? IndexUserProfileVMs { get; set; }
        public string? CurrentUserPersonalNumber { get; set; }
    }
}
