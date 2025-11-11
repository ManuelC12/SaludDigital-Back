using SaludDigital.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SaludDigital.Helpers
{
    public class GlobalFunctions
    {
        private readonly IConfiguration _config;

        public GlobalFunctions(IConfiguration config)
        {
            _config = config;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();

            }
        }

        public static DateTime GetTimeByTimeZone()
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            DateTime timeUtc = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, localZone);
        }

        public static string ReadTokenId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var jti = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
            return jti;
        }

        public string JWT(UserDto user)
        {
            var key = _config["JWT:ClaveSecreta"];
            var issuer = _config["JWT:Issuer"];
            var audience = _config["JWT:Audience"];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Name", user.Name),
                new Claim("iUser", user.iUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
