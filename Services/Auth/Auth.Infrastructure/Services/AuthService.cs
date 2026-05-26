using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Constants;
using Auth.Domain.Entities;
using Auth.Infrastructure.Interfaces;
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
            var existingUser = await _context.Users
             .Where(u => u.Email == request.Email || u.UserName == request.UserName)
             .Select(u => new { u.Email, u.UserName })
             .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email == request.Email)
                    return Result<TokenResponse>.Failure("Email already exists.");

                if (existingUser.UserName == request.UserName)
                    return Result<TokenResponse>.Failure("Username already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var permissions = await _context.Roles
             .Where(r => r.Name == Roles.User)
             .SelectMany(r => r.RolePermissions)
             .Select(rp => rp.Permission.Name)
             .ToListAsync(cancellationToken);


            var tokenResponse = await GenerateAndSaveTokensAsync(user, permissions,
                request.IpAddress, request.DeviceInfo, cancellationToken);

            return Result<TokenResponse>.Success(tokenResponse);
        }

        public Task<Result<LogoutResponse>> LogoutAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TokenResponse>> RegisterAsync(string username, string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TokenResponse>> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
        private async Task<TokenResponse> GenerateAndSaveTokensAsync(
            User user,
            List<string> permissions,
            string ipAddress,
            string deviceInfo,
            CancellationToken cancellationToken)
        {
            var tokenResponse = _tokenGenerator.GenerateTokenPair(user, permissions);

            var refreshTokenHash = _tokenGenerator.HashToken(tokenResponse.RefreshToken);

            var refreshTokenEntity = new RefreshToken
            {
                TokenHash = refreshTokenHash,
                User = user,
                ExpiresAt = tokenResponse.RefreshTokenExpiration,
                CreatedByIp = ipAddress,
                DeviceInfo = deviceInfo
            };

            await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return tokenResponse;
        }

        public Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
