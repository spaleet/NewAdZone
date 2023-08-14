using BuildingBlocks.Persistence.Ef;

namespace Auth.Application.Interfaces;

public interface IAuthDbContext : IBaseDbContext
{
    DbSet<AuthToken> AuthTokens { get; }
}
