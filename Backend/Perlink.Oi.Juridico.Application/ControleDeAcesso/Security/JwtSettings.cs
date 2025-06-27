using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Security {
    public class JwtSettings {
        public string Audience { get; }
        public string Issuer { get; }
        public int ValidForMinutes { get; }
        public int RefreshTokenValidForMinutes { get; }
        public SigningCredentials SigningCredentials { get; }

        public DateTime IssuedAt => DateTime.UtcNow;
        public DateTime NotBefore => IssuedAt;
        public DateTime AccessTokenExpiration => IssuedAt.AddMinutes(ValidForMinutes);
        public DateTime RefreshTokenExpiration => IssuedAt.AddMinutes(RefreshTokenValidForMinutes);

        public JwtSettings() { }

        public JwtSettings(IConfiguration configuration) {
            Issuer = global::Oi.Juridico.AddOn.Configuracoes.JwtSettings.Issuer;
            Audience = global::Oi.Juridico.AddOn.Configuracoes.JwtSettings.Audience;
            ValidForMinutes = global::Oi.Juridico.AddOn.Configuracoes.JwtSettings.ValidForMinutes;
            RefreshTokenValidForMinutes = global::Oi.Juridico.AddOn.Configuracoes.JwtSettings.RefreshTokenValidForMinutes;

            var signingKey = global::Oi.Juridico.AddOn.Configuracoes.JwtSettings.SigningKey;
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        }

    }
}