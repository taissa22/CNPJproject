using System;

namespace Oi.Juridico.Shared.V2.Seguranca
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; } = "";
        public RefreshToken RefreshToken { get; set; }
        public string TokenType { get; set; } = "bearer";
        public long ExpiresIn { get; set; }
    }

    public class RefreshToken
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
