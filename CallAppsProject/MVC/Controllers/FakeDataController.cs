using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.ViewModels;
using System.Text.Json;
using System.Xml.Linq;

namespace MVC.Controllers
{
    public class FakeDataController : Controller
    {
        public async Task<IActionResult> Index()
        {

            using(var client = new HttpClient())
            {
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<FakeUser>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return View(users);
            }

        }

        public async Task<IActionResult> FakeUserProfile(int userId)
        {
            using(var client = new HttpClient())
            {

                var usersResponse = await client.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
                var content = await usersResponse.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<FakeUser>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if(user == null || user.Id == 0)
                    return NotFound("ჩანაწერი არ მოიძებნა");

                var VM = new FakeUserProfileVM { User = user };

                var postsResponse = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts?userId={userId}");
                var postsContent = await postsResponse.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<Post>?>(postsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                VM.UserPosts = posts;

                if(VM.UserPosts != null)
                {
                    var commentsResponse = await client.GetAsync($"https://jsonplaceholder.typicode.com/comments");
                    var commentsContent = await commentsResponse.Content.ReadAsStringAsync();
                    var comments = JsonSerializer.Deserialize<List<Comment>?>(commentsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if(comments != null)
                        for(int i = 0; i < VM.UserPosts.Count(); i++)
                            VM.UserPosts[i].Comments = comments.Where(c => c.postId == VM.UserPosts[i].id).ToList();
                    
                }

                var AlbumsResponse = await client.GetAsync($"https://jsonplaceholder.typicode.com/albums?userId={userId}");
                var AlbumsContent = await AlbumsResponse.Content.ReadAsStringAsync();
                var AlbumsList = JsonSerializer.Deserialize<List<Album>?>(AlbumsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                VM.Albums = AlbumsList;

                if(VM.Albums != null)
                {
                    var PhotosResponse = await client.GetAsync($"https://jsonplaceholder.typicode.com/photos");
                    var PhotosContent = await PhotosResponse.Content.ReadAsStringAsync();
                    var PhotosList = JsonSerializer.Deserialize<List<Photo>?>(PhotosContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (PhotosList != null)
                        for (int i = 0; i < VM.Albums.Count(); i++)
                            VM.Albums[i].Photos = PhotosList.Where(p => p.albumId == VM.Albums[i].id).ToList();
                }

                var ToDosResponse = await client.GetAsync($"https://jsonplaceholder.typicode.com/todos?userId={userId}");
                var ToDosContent = await ToDosResponse.Content.ReadAsStringAsync();
                var ToDosList = JsonSerializer.Deserialize<List<Todo>?>(ToDosContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                VM.Todos = ToDosList;



                return View(VM);
            }
        }


    }
}
