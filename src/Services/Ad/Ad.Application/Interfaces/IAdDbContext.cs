using System.Collections.Generic;
using Ad.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ad.Application.Interfaces;

public interface IAdDbContext
{
    DbSet<Domain.Entities.Ad> Ads { get; }
    DbSet<AdGallery> AdGalleries { get; }
    DbSet<AdCategory> AdCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}
