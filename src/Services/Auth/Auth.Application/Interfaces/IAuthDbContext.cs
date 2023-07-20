using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces;

public interface IAuthDbContext
{
    DbSet<AuthToken> AuthTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}
