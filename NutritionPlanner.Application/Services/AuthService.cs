using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _userRepository;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IUsersRepository usersRepository)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _userRepository = usersRepository;
        }

        public async Task<Guid> RegisterUserAsync(RegisterRequest request)
        {
            var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Пользователь с таким email уже существует.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserEntity
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                Name = request.Name,
                Age = request.Age,
                Gender = request.Gender,
                Height = request.Height,
                Weight = request.Weight,
                ActivityLevelId = request.ActivityLevelId,
                CreatedAt = DateTime.UtcNow,
                Role = Role.User
            };

            return await _authRepository.CreateUserAsync(user);
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный email или пароль.");

            if (user.IsBlocked)
            {
                if (user.BlockedUntil.HasValue && user.BlockedUntil <= DateTime.UtcNow)
                {
                    user.IsBlocked = false;
                    user.BlockedUntil = null;
                    user.BlockReason = null;
                    await _userRepository.UpdateAsync(user);
                }
                else
                {
                    var message = "Ваш аккаунт заблокирован";
                    bool hasDate = user.BlockedUntil.HasValue;
                    bool hasReason = !string.IsNullOrEmpty(user.BlockReason);

                    if (hasDate)
                    {
                        message += $" до {user.BlockedUntil.Value:dd.MM.yyyy HH:mm}";
                    }

                    if (hasReason)
                    {
                        message += hasDate ? ". " : " ";
                        message += $"Причина: {user.BlockReason}";
                    }

                    throw new UnauthorizedAccessException(message);
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _authRepository.GetUserByEmailAsync(email);
        }
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        [Required]
        public int ActivityLevelId { get; set; }

        [Required]
        public int GoalTypeId { get; set; }

    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
