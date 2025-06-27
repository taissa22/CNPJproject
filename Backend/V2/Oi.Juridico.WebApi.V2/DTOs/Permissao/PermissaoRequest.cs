namespace Oi.Juridico.WebApi.V2.DTOs.Permissao
{
    public class PermissaoRequest
    {
        public string PermissaoId { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Caminho { get; set; } = string.Empty;
        public int[] ListaModulos { get; set; } = Array.Empty<int>();
    }
}
