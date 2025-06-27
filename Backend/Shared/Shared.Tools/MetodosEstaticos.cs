using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Tools
{
    public static class MetodosEstaticos
    {
        public static string GerarSenha()
        {
            var opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
            };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public static string CriptografarSenha(string palavraChave, string senha)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();

            return pw.HashPassword(palavraChave, senha);
        }

        public static bool CompararSenha(string senhaCriptografada, string senhaInformada, string palavraChave)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();

            var resultado = pw.VerifyHashedPassword(palavraChave, senhaCriptografada, senhaInformada);

            return (resultado == PasswordVerificationResult.Success);
        }
    }
}
