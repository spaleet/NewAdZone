using BuildingBlocks.Persistence.Ef.Base;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities;

public class UserRole : IdentityRole<Guid>, AuditableBase
{
    public DateTime CreateDate { get; set; } = DateTime.Now;

    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}