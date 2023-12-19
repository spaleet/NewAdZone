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

    public void Initialize()
    {
        try
        {
            // don't use EnsureCreated with Migrate
            _context.Database.Migrate();

            SeedCategories();

            _context.SaveChanges();

            _logger.LogInformation("Successfully initialized Ad Db!");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }

    private void SeedCategories()
    {
        if (!_context.AdCategories.Any())
        {
            var mobileCategory = new AdCategory("موبایل");

            _context.AdCategories.Add(mobileCategory);
            _context.SaveChanges();

            var laptopCategory = new AdCategory("لپتاپ");

            _context.AdCategories.Add(laptopCategory);
            _context.SaveChanges();

            var headphoneCategory = new AdCategory("هدفون");

            _context.AdCategories.Add(headphoneCategory);
            _context.SaveChanges();

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

            _context.AdCategories.AddRange(seeds);
            _context.SaveChanges();
        }
    }
}