using System.Net;

namespace MVC.Models
{
    public class FakeUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public Address? Address { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public Company? Company { get; set; }
    }

    public class Address
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public Geo? Geo { get; set; }

    }

    public class Geo
    {
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }

    public class Company
    {
        public string? Name { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }

    }

    public class Post
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }
        public List<Comment>? Comments { get; set; }
    }

    public class Comment
    {
        public int postId { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
    }

    public class Album
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string? title { get; set; }
        public List<Photo>? Photos { get; set; }
    }

    public class Photo
    {
        public int albumId { get; set; }
        public int id { get; set; }
        public string? title { get; set; }
        public string? url { get; set; }
        public string? thumbnailUrl { get; set; }
    }

    public class Todo
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string? title { get; set; }
        public bool completed { get; set; }
    }



}
