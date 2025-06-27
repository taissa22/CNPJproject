using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Oi.Juridico.Shared.V2.Seguranca
{
    public class JwtSettings
    {
        public string Audience { get; }
        public string Issuer { get; }
        public int ValidForMinutes { get; }
        public int RefreshTokenValidForMinutes { get; }
        public SigningCredentials SigningCredentials { get; }

        public DateTime IssuedAt => DateTime.UtcNow;
        public DateTime NotBefore => IssuedAt;
        public DateTime AccessTokenExpiration => IssuedAt.AddMinutes(ValidForMinutes);
        public DateTime RefreshTokenExpiration => IssuedAt.AddMinutes(RefreshTokenValidForMinutes);

        public JwtSettings(IConfiguration configuration)
        {
            Issuer = AddOn.Configuracoes.JwtSettings.Issuer;
            Audience = AddOn.Configuracoes.JwtSettings.Audience;
            ValidForMinutes = AddOn.Configuracoes.JwtSettings.ValidForMinutes;
            RefreshTokenValidForMinutes = AddOn.Configuracoes.JwtSettings.RefreshTokenValidForMinutes;

            var signingKey = AddOn.Configuracoes.JwtSettings.SigningKey;
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        }

    }
}
