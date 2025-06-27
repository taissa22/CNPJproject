namespace Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Accounts
{
    public class LoginRequest
    {
        public string Username { get; }
        public string? Password { get; }
        public string GrantType { get; }
        public string RefreshToken { get; }

        public LoginRequest(string username, string password, string refreshtoken, string granttype)
        {
            GrantType = granttype ?? "password";
            Username = username.ToUpper();
            Password = password ?? null;
            RefreshToken = refreshtoken ?? "60";
        }

    }
}
