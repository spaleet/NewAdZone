using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using BuildingBlocks.Persistence.Ef.Base;
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

    // can't inherit BaseDbContext so here we go
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntries = this.ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            var baseEntity = insertedEntry as AuditableBase;

            // if insertedEntry inherits BaseEntity
            if (baseEntity != null)
            {
                baseEntity.CreateDate = DateTime.Now;
            }
        }

        var modifiedEntries = this.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Modified)
                   .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries)
        {
            //if modifiedEntry inherits BaseEntity.
            var baseEntity = modifiedEntry as AuditableBase;
            if (baseEntity != null)
            {
                baseEntity.LastUpdateDate = DateTime.Now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}