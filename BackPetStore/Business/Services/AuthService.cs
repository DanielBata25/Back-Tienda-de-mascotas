using Business.Interfaces;
using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IClienteRepository clienteRepository, ApplicationDbContext context, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> AuthenticateAsync(LoginDto loginDto)
        {
            // 🔍 Buscar cliente por correo
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email && !c.IsDeleted);

            if (cliente == null)
                return null;

            // 🔐 Validar contraseña
            if (!VerifyPassword(loginDto.Password, cliente.Password))
                return null;

            // 🎟️ Claims personalizados del cliente
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim("nombre", $"{cliente.Nombre} {cliente.Apellido}"),
                new Claim("rol", "Cliente")
            };

            // 🔑 Clave secreta
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();

            // ⏰ Tiempo de expiración configurable desde appsettings.json
            int expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes", 30);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ============================================
        // 🔐 HASH + VERIFICACIÓN
        // ============================================
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Permite tanto contraseñas en texto plano como hash (por compatibilidad)
            var inputHash = HashPassword(inputPassword);
            return storedPassword == inputPassword || storedPassword == inputHash;
        }
    }
}
