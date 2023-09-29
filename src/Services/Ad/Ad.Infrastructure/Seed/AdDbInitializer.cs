using Ad.Domain.Entities;
using Ad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ad.Infrastructure.Seed;

public class AdDbInitializer
{
    private readonly AdDbContext _context;
    private readonly ILogger<AdDbInitializer> _logger;

    public AdDbInitializer(ILogger<AdDbInitializer> logger, AdDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            bool created = await _context.Database.EnsureCreatedAsync();

            if (created && _context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();

                await SeedCategoriesAsync();

                _logger.LogInformation("Successfully initialized Ad Db!");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }

    private async Task SeedCategoriesAsync()
    {
        if (!await _context.AdCategories.AnyAsync())
        {
            var mobileCategory = new AdCategory("موبایل");

            await _context.AdCategories.AddAsync(mobileCategory);
            await _context.SaveChangesAsync();

            var laptopCategory = new AdCategory("لپتاپ");

            await _context.AdCategories.AddAsync(laptopCategory);
            await _context.SaveChangesAsync();

            var headphoneCategory = new AdCategory("هدفون");

            await _context.AdCategories.AddAsync(headphoneCategory);
            await _context.SaveChangesAsync();

            List<AdCategory> seeds = new List<AdCategory>
            {
                new ("آیفون", parentId: mobileCategory.Id),
                new ("سامسونگ", parentId: mobileCategory.Id),
                new ("شیائومی", parentId: mobileCategory.Id),

                new ("مکبوک", parentId: laptopCategory.Id),
                new ("لنوو", parentId: laptopCategory.Id),
                new ("ایسوس", parentId: laptopCategory.Id),

                new ("ایرپاد", parentId: headphoneCategory.Id),
                new ("گلکسی بادز", parentId: headphoneCategory.Id),
            };

            await _context.AdCategories.AddRangeAsync(seeds);
            await _context.SaveChangesAsync();
        }
    }
}
