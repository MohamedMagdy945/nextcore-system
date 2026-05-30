using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Common.Shared.Constant;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence.Seeder;

public class UserSeeder
{
    private readonly IAuthDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserSeeder(
        IAuthDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (await _context.Users.AnyAsync())
        {
            return;
        }

        var adminRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == Roles.Admin);

        var userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == Roles.User);

        if (adminRole is null || userRole is null)
        {
            return;
        }

        var adminUser = new User
        {
            FirstName = "System",
            LastName = "Admin",
            FullName = "admin",
            Email = "admin@gmail.com",
            PasswordHash = _passwordHasher.Hash("Admin@123"),
        };

        var normalUser = new User
        {
            FirstName = "Test",
            LastName = "User",
            FullName = "user",
            Email = "user@gmail.com",
            PasswordHash = _passwordHasher.Hash("User@123"),
        };

        await _context.Users.AddRangeAsync(adminUser, normalUser);

        await _context.SaveChangesAsync();

        var userRoles = new List<UserRole>
        {
            new()
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            },

            new()
            {
                UserId = normalUser.Id,
                RoleId = userRole.Id
            }
        };

        await _context.UserRoles.AddRangeAsync(userRoles);

        await _context.SaveChangesAsync();
    }
}