using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DynapredictAPI.Models;
using DynapredictAPI.Data;
using DynapredictAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DynapredictAPI.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<object> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new
            {
                user = new
                {
                    id = user.Id.ToString(),
                    email = user.Email,
                    name = user.Name,
                    role = user.Role,
                    createdAt = user.CreatedAt,
                    lastLogin = user.LastLogin
                },
                token = new
                {
                    accessToken = token,
                    refreshToken = Guid.NewGuid().ToString(),
                    expiresIn = 3600
                }
            };
        }

        public async Task<object> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new InvalidOperationException("Email já cadastrado");
            }

            var user = new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new
            {
                user = new
                {
                    id = user.Id.ToString(),
                    email = user.Email,
                    name = user.Name,
                    role = user.Role,
                    createdAt = user.CreatedAt,
                    lastLogin = user.LastLogin
                },
                token = new
                {
                    accessToken = token,
                    refreshToken = Guid.NewGuid().ToString(),
                    expiresIn = 3600
                }
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}