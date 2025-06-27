namespace Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.DTOs.Perfil
{
    public class ObterUsuariosResponse
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Associado { get; set; }
        public bool Ativo { get; set; }
    }
}
