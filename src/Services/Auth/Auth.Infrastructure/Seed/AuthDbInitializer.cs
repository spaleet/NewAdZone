using Auth.Domain.Entities;
using Auth.Domain.Enums;
using Auth.Infrastructure.Context;
using BuildingBlocks.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Seed;
public class AuthDbInitializer
{
    private readonly DatabaseContext _context;
    private readonly ILogger<AuthDbInitializer> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;

    public AuthDbInitializer(ILogger<AuthDbInitializer> logger, DatabaseContext context,
        UserManager<User> userManager, RoleManager<UserRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
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

            await SeedRolesAsync();
            await SeedUsersAsync();

            _logger.LogError("Successfully initialized Auth Db!");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while initializing the database : {ex}", ex.Message);
            throw;
        }
    }

    private async Task SeedRolesAsync()
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

    private async Task SeedUsersAsync()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            //--------- Admin User
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "ادمین",
                LastName = "ادمینی",
                PhoneNumber = "09123456789",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(adminUser, "123Pa$$word!");
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
            await _userManager.AddToRoleAsync(adminUser, Roles.VerifiedUser.ToString());
            await _userManager.AddToRoleAsync(adminUser, Roles.BasicUser.ToString());

            //--------- Verified User
            var verifiedUser = new User
            {
                UserName = "cooluser",
                Email = "cooluser@gmail.com",
                FirstName = "کاربر فعال",
                LastName = "کاربری",
                PhoneNumber = "09987654321",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(verifiedUser, "123Pa$$word!");
            await _userManager.AddToRoleAsync(verifiedUser, Roles.VerifiedUser.ToString());
            await _userManager.AddToRoleAsync(verifiedUser, Roles.BasicUser.ToString());


            //--------- Basic User
            var basicUser = new User
            {
                UserName = "newuser",
                Email = "newuser@gmail.com",
                FirstName = "کاربر",
                LastName = "کاربری",
                PhoneNumber = "09321651789",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(basicUser, "123Pa$$word!");
            await _userManager.AddToRoleAsync(basicUser, Roles.BasicUser.ToString());
        }
    }
}