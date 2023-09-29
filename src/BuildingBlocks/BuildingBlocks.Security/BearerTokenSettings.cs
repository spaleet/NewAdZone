using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Security;

public class BearerTokenSettings
{
    [Required]
    public string Secret { set; get; }

    [Required]
    public int AccessTokenExpirationMinutes { set; get; }

    [Required]
    public int RefreshTokenExpirationHours { set; get; }

    [Required]
    public bool AllowMultipleLoginsFromTheSameUser { set; get; }

    [Required]
    public bool AllowSignoutAllUserActiveClients { set; get; }
}