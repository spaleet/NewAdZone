namespace Auth.Application.Models;
public record UserProfileDto
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Bio { get; set; }

    public string Avatar { get; set; }

    public string RegisterDate { get; set; }

    public string[] Roles { get; set; }
}
