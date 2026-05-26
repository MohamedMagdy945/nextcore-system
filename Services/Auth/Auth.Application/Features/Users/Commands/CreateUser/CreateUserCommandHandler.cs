
using Auth.Application.Bases;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Users.Commands.AddUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly IAuthDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        public CreateUserCommandHandler(
            IAuthDbContext context,
            IPasswordHasher passwordHasher,
            ILogger<CreateUserCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var exists = await _context.Users
                            .AnyAsync(x => x.Email == request.Email || x.UserName == request.UserName, cancellationToken);

            if (exists)
                return Result<int>.Failure("A user with the same email or username already exists.");



            User user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash("Admin@1234")
            };

            user.Email = request.Email;

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(user.Id, "User added successfully.");
        }
    }
}
