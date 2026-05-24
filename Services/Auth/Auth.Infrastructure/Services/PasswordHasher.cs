using Auth.Application.Interfaces;

namespace Auth.Infrastructure.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {

        private const int WorkFactor = 12;

        public string Hash(string password)
         => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);


        public bool Verify(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);


    }
}
