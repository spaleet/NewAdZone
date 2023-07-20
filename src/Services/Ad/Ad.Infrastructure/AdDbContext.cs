using Ad.Application.Interfaces;
using Ad.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ad.Infrastructure;

public class AdDbContext : DbContext, IAdDbContext
{
    public AdDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Ad> Ads { get; set; }

    public DbSet<AdGallery> AdGalleries { get; set; }

    public DbSet<AdCategory> AdCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
