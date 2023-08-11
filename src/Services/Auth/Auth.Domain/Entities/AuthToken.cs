using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Persistence.Ef.Base;

namespace Auth.Domain.Entities;

public class AuthToken : AuditableBase
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string AccessTokenHash { get; set; }

    public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

    public string RefreshTokenIdHash { get; set; }

    public string RefreshTokenIdHashSource { get; set; }

    public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}
