using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Context;

public class DatabaseContext : IdentityDbContext<User, UserRole, Guid>, IAuthDbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AuthToken> AuthTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
