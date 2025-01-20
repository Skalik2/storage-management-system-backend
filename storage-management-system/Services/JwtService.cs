using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using storage_management_system.Data;
using storage_management_system.Model.DataTransferObject;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace storage_management_system.Services
{
    public class JwtService
    {
        private readonly PgContext _pgContext;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        public JwtService(PgContext context, IConfiguration config, IPasswordHasher passwordHasher) 
        { 
            _configuration = config;
            _pgContext = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto?> Authenticate(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return null;
            var userAccount = await _pgContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Username == request.UserName);
            if (userAccount == null || !_passwordHasher.Verification(request.Password, userAccount.Password))
                return null;

            var userRoles = userAccount.UserRoles.Select(ur => ur.Role.Name).ToList();

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTime = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, userAccount.Id.ToString()),
            };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = tokenExpiryTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha512Signature),


            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                Username = request.UserName,
                ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.UtcNow).TotalSeconds,
            };
        }
    }
}
