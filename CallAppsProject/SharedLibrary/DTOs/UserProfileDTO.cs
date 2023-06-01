namespace SharedLibrary.DTOs;

//UserProfile entity depends on the User entity so we need the data to for creating both of them
public class UserProfileDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PersonalNumber { get; set; }
}
