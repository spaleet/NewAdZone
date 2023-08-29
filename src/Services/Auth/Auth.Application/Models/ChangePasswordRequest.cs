namespace Auth.Application.Models;

public record ChangePasswordRequest
{
    public string Id { get; set; }

    public string CurrentPassword { get; set; }

    public string NewPassword { get; set; }
}
