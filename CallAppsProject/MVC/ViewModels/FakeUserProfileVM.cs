using MVC.Models;

namespace MVC.ViewModels
{
    public class FakeUserProfileVM
    {
        public required FakeUser User { get; set; }
        public List<Post>? UserPosts { get; set; }
        public List<Album>? Albums { get; set; }
        public List<Todo>? Todos { get; set; }
    }
}
