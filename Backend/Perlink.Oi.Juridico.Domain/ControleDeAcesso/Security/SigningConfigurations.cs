﻿using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Security {
    public class SigningConfigurations {
        public SecurityKey Key { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfigurations() {
            using (var provider = new RSACryptoServiceProvider(2048)) {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}