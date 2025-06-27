using System;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Perlink.Oi.Juridico.Application.Security {
    public class JwtService : IJwtService {
        private readonly JwtSettings _settings;
        private IDistributedCache _cache;

        public JwtService(JwtSettings settings, IDistributedCache cache)
        {
            _settings = settings;
            _cache = cache;
        }

        public JsonWebToken CreateJsonWebToken(User user) {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new GenericIdentity(user.Login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Login)
                    }),                   
                Expires = _settings.AccessTokenExpiration,                 
                SigningCredentials = _settings.SigningCredentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);


            var accessToken = tokenHandler.WriteToken(token);

            var refreshToken = CreateRefreshToken(user.Login);

            TimeSpan finalExpiration =
                TimeSpan.FromMinutes(_settings.RefreshTokenValidForMinutes);

            DistributedCacheEntryOptions opcoesCache =
                new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(finalExpiration);
            _cache.SetString(refreshToken.Token,
                JsonSerializer.Serialize(refreshToken),
                opcoesCache);

            return new JsonWebToken {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = (long)TimeSpan.FromMinutes(_settings.ValidForMinutes).TotalSeconds
            };
        }

        private RefreshToken CreateRefreshToken(string username)
        {
            var refreshToken = new RefreshToken
            {
                Username = username,
                ExpirationDate = _settings.RefreshTokenExpiration
            };

            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.Token = token.Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);


            return refreshToken;
        }

        private static ClaimsIdentity GetClaimsIdentity(User user) {
            var identity = new ClaimsIdentity
            (
                new GenericIdentity(user.Login),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name)
                }
            );

            foreach (var role in user.Roles) {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            foreach (var policy in user.Permissions) {
                identity.AddClaim(new Claim("permissions", policy));
            }

            return identity;
        }
    }
}