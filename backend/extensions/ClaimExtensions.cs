using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetEnv;

namespace backend.extensions
{
    public static class ClaimExtensions
    {
        public static string? GetUserName(this ClaimsPrincipal user)
        {
            Env.Load();
            
            var JWT_CLAIMS_URI = Environment.GetEnvironmentVariable("JWT_CLAIMS_URI") ?? "";
            
            return user?.Claims?
                .FirstOrDefault(c => c.Type == JWT_CLAIMS_URI)
                ?.Value;
        }
    }
}