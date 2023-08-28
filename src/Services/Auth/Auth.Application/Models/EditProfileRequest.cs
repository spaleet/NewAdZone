using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Auth.Application.Models;
public record EditProfileRequest
{
    public string Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [MaxLength(250)]
    [MinLength(25)]
    public string Bio { get; set; }

    public IFormFile? AvatarSource { get; set; }
}
