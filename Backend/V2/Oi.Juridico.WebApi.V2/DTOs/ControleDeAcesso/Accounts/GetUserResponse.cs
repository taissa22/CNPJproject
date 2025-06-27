namespace Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Accounts
{
    public class GetUserResponse
    {
        public string Username { get; set; } = "";
        public string Nome { get; set; } = "";
        public string[] Permissoes { get; set; } = Array.Empty<string>();
        public string Ambiente { get; set; } = "";
        public bool EhEscritorio { get; set; } = false;
    }
}
