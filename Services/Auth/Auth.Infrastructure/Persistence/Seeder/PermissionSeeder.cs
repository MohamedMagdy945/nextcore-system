using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Common.Shared.Constant;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence.Seeder;

public class PermissionSeeder
{
    private readonly IAuthDbContext _context;

    public PermissionSeeder(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Permissions.AnyAsync())
        {
            return;
        }

        var allPermissions = new List<string>();

        allPermissions.AddRange(Users.GetAll());
        allPermissions.AddRange(Products.GetAll());

        var userPermissions = new List<string>
        {
            Users.View,
            Products.View,
            Products.Rate
        };

        var adminRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == Roles.Admin);

        var userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == Roles.User);

        if (adminRole is null || userRole is null)
        {
            return;
        }

        var permissions = allPermissions
            .Select(p => new Permission
            {
                Name = p
            })
            .ToList();

        await _context.Permissions.AddRangeAsync(permissions);

        var adminRolePermissions = permissions
            .Select(p => new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = p.Id
            })
            .ToList();

        var userRolePermissions = permissions
            .Where(p => userPermissions.Contains(p.Name))
            .Select(p => new RolePermission
            {
                RoleId = userRole.Id,
                PermissionId = p.Id
            })
            .ToList();

        await _context.RolePermissions.AddRangeAsync(adminRolePermissions);

        await _context.RolePermissions.AddRangeAsync(userRolePermissions);

        await _context.SaveChangesAsync();
    }
}