using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AuthToken> AuthTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}
