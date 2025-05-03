using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.interfaces;
using backend.models;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;

namespace backend.repositories
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            Env.Load();
            var signingKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY") ?? "";
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey ?? throw new InvalidOperationException("JWT Signing Key is not set")));
        }

        public string CreateToken(AppUser user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentException("User must have an email and username to create a token.");
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "";
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "";

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);  
        }
    }
}