using Ad.Application.Interfaces;
using Ad.Domain.Entities;
using BuildingBlocks.Persistence.Ef;
using Microsoft.EntityFrameworkCore;

namespace Ad.Infrastructure.Context;

public class AdDbContext : BaseDbContext, IAdDbContext
{
    public AdDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Ad> Ads { get; set; }

    public DbSet<AdGallery> AdGalleries { get; set; }

    public DbSet<AdCategory> AdCategories { get; set; }
}
