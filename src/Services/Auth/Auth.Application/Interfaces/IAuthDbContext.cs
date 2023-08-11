using Auth.Domain.Entities;
using BuildingBlocks.Persistence.Ef;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces;

public interface IAuthDbContext : IBaseDbContext
{
    DbSet<AuthToken> AuthTokens { get; }
}
