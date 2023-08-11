using BuildingBlocks.Persistence.Ef.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Ef;

public interface IBaseDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}


public abstract class BaseDbContext : DbContext, IBaseDbContext 
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntries = this.ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            var baseEntity = insertedEntry as EfEntityBase;
            //If the inserted object is an Auditable. 
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
            //If the inserted object is an Auditable. 
            var baseEntity = modifiedEntry as EfEntityBase;
            if (baseEntity != null)
            {
                baseEntity.LastUpdateDate = DateTime.Now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
