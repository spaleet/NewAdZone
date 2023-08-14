using Ad.Domain.Entities;
using BuildingBlocks.Persistence.Ef;

namespace Ad.Application.Interfaces;

public interface IAdDbContext : IBaseDbContext
{
    DbSet<Domain.Entities.Ad> Ads { get; }
    DbSet<AdGallery> AdGalleries { get; }
    DbSet<AdCategory> AdCategories { get; }
}
