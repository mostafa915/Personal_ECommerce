using ECommerce.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Authentication
{
    public class Jwt(IOptions<JwtOptions> options) : IJwt
    {
        private readonly JwtOptions _jwtOptions = options.Value;
        public (string token, int exipersIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),
                ];

            var symmertricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var signinCredinal = new SigningCredentials(symmertricKey, SecurityAlgorithms.HmacSha256);



            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresIn),
                signingCredentials: signinCredinal
                );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), _jwtOptions.ExpiresIn);
        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmertricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmertricKey,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims.First(j => j.Type == JwtRegisteredClaimNames.Sub).Value;
            }

            catch
            {
                return null;

            }
        }
    }
}
