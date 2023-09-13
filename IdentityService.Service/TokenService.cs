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
using System.Security.Principal;
using System.Net.Http.Headers;

namespace IdentityService.Service
{
    public class TokenService : ITokenService
    {
        readonly string issuer;
        readonly string key;
        readonly string audience;      


        private IUserService _userService; 
        public TokenService(IOptions<IdentityServerConfigs> config ,
            IOptions<SettingsConfig>settingsConfig,
            IUserService userService
        ) { 
            _userService = userService;
            issuer = config.Value.Issuer.ToUpper();
            key = settingsConfig.Value.Key.ToUpper();
            audience = settingsConfig.Value.Audience.ToUpper();
        }

        public string Create(User user, AppSession session, int expirationTime = 4)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.Audience.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);


            var claims = new List<Claim>
            {
                new Claim("name", user.Name),
                new Claim("departamento", user.departamento),
                new Claim("GroupId", user.GroupId),
                new Claim("UserId", user.UserId),
                new Claim("Session", session.Session.ToString()),
                new Claim("ApiKey", session.ApiKey.ToString()),
                new Claim("WppToken", session.Token.ToString()),
                new Claim("UId", user.Id.HasValue ? user.Id.Value.ToString() : "")
            };
            claims.AddRange(user.Audience.Roles.Select(x => new Claim(ClaimTypes.Role, x.Value)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddHours(expirationTime),
                SigningCredentials = signingCredentials,
                Issuer = issuer.ToUpper(),
                Audience = user.Audience.Id.ToString().ToUpper(),
              
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        public async Task<LoginToken> LoginByToken(string accessToken)
        {
            ClaimsPrincipal user = ValidateToken(accessToken);
            var uid =    user.Claims.FirstOrDefault(x=> x.Type == "UId")?.Value.ToString();

            return new LoginToken(){
                user = await _userService.GetDetailsUser(uid),
                accessToken = accessToken, 
                tokenType = "bearer"
            };
        }


        private  ClaimsPrincipal ValidateToken(string authToken)
        {
           var tokenHandler = new JwtSecurityTokenHandler();
           var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f90702a4-a013-4a5d-bd06-ce600dd8a588"));

           var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =securityKey,
                ValidIssuer = issuer, 
                ValidAudience = audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true//Not setting ClockSkew will make token valid for more 5 minutes
            };


            SecurityToken validatedToken;
            return  tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            
        }

    }
}