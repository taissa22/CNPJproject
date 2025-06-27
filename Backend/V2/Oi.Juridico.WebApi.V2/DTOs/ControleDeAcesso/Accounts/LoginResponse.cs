namespace Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Accounts
{
    public record LoginResponse(string Access_token, string Token_type, long Expires_in, string Refresh_token);
}
