using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Interfaces;
using Common.Shared.Constant;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(
            IAuthDbContext context,
            ITokenGenerator tokenGenerator,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request,
            CancellationToken cancellationToken)
        {

            var exists = await _context.Users
             .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (exists)
                return Result<TokenResponse>.Failure("Email is already in use.");

            var defaultRole = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Name == Roles.User, cancellationToken);

            if (defaultRole == null)
                return Result<TokenResponse>.Failure("Default role not found.");

            var permissions = defaultRole.RolePermissions.Select(rp => rp.Permission.Name).ToList();


            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                PasswordHash = _passwordHasher.Hash(request.Password),
                PhoneNumber = request.PhoneNumber,
            };


            await _context.Users.AddAsync(user, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            TokenResponse tokenResponse = _tokenGenerator.GenerateTokenPair(user, permissions);


            await _context.UserRoles.AddAsync(new UserRole
            {
                UserId = user.Id,
                RoleId = defaultRole.Id
            }, cancellationToken);

            await _context.RefreshTokens.AddAsync(new RefreshToken()
            {
                UserId = user.Id,
                TokenHash = _tokenGenerator.HashToken(tokenResponse.RefreshToken),
                ExpiresAt = tokenResponse.RefreshTokenExpiration,
                CreatedByIp = request.IpAddress,
                DeviceInfo = request.DeviceInfo
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<TokenResponse>.Success(tokenResponse);
        }

        public Task<Result<TokenResponse>> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }


        public Task<Result<LogoutResponse>> LogoutAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }



        public Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }


    }
}
