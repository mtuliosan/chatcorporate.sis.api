using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Service
{
    public class TokenService : ITokenService
    {
        readonly string issuer;

        public TokenService(IOptions<IdentityServerConfigs> config) => issuer = config.Value.Issuer;

        public string Create(User user, AppSession session, int expirationTime = 4)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.Audience.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);


            var claims = new List<Claim>
            {
                new Claim("name", user.Name),
                new Claim("GroupId", user.GroupId),
                new Claim("UserId", user.UserId),
                new Claim("Session", session.Session.ToString()),
                new Claim("ApiKey", session.ApiKey.ToString()),
                new Claim("WppToken", session.Token.ToString()),
                new Claim("UId", user.Id.HasValue ? user.Id.Value.ToString() : "")
            };
            claims.AddRange(user.Audience.Roles.Select(x => new Claim("role", x.Value)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddHours(expirationTime),
                SigningCredentials = signingCredentials,
                Issuer = issuer.ToUpper(),
                Audience = user.Audience.Id.ToString().ToUpper()
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}