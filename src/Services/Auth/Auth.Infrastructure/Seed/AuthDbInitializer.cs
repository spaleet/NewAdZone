using Auth.Domain.Entities;
using Auth.Domain.Enums;
using Auth.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Seed;
public class AuthDbInitializer
{
    private readonly DatabaseContext _context;
    private readonly ILogger<AuthDbInitializer> _logger;
    private readonly RoleManager<UserRole> _roleManager;

    public AuthDbInitializer(ILogger<AuthDbInitializer> logger, DatabaseContext context,
        RoleManager<UserRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while seeding the database : {ex}", ex.Message);
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
        {
            await _roleManager.CreateAsync(new UserRole
            {
                Name = nameof(Roles.Admin),
                NormalizedName = nameof(Roles.Admin).ToUpper()
            });
        }

        if (!await _roleManager.RoleExistsAsync(Roles.VerifiedUser.ToString()))
        {
            await _roleManager.CreateAsync(new UserRole
            {
                Name = nameof(Roles.VerifiedUser),
                NormalizedName = nameof(Roles.VerifiedUser).ToUpper()
            });
        }

        if (!await _roleManager.RoleExistsAsync(Roles.BasicUser.ToString()))
        {
            await _roleManager.CreateAsync(new UserRole
            {
                Name = nameof(Roles.BasicUser),
                NormalizedName = nameof(Roles.BasicUser).ToUpper()
            });
        }
    }
}