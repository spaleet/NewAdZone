using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Persistence.Ef.Base;

namespace Auth.Domain.Entities;

public class AuthToken : EfEntityBase<Guid>
{
    public Guid UserId { get; set; }

    public string AccessTokenHash { get; set; }

    public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

    public string RefreshTokenIdHash { get; set; }

    public string RefreshTokenIdHashSource { get; set; }

    public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

    //================================== Relations
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}
