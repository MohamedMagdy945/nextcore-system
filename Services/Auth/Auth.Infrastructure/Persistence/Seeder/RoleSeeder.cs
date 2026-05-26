using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Common.Shared.Constant;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence.Seeder;

public class RoleSeeder
{
    private readonly IAuthDbContext _context;

    public RoleSeeder(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Roles.AnyAsync())
        {
            return;
        }
        var roles = Roles.GetAll();

        var rolesToDb = roles.Select(r => new Role { Name = r }).ToList();



        await _context.Roles.AddRangeAsync(rolesToDb);

        await _context.SaveChangesAsync();
    }
}