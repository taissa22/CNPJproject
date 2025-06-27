namespace Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Parametros
{
    public class GetByIdResponse
    {
        public string CodParametro { get; set; } = "";
        public string CodTipoParametro { get; set; } = "";
        public string DscParametro { get; set; } = "";
        public string DscConteudoParametro { get; set; } = "";
        public string IndUsuarioAtualiza { get; set; } = "";
        public string DscDonoParametro { get; set; } = "";
        public DateTime? DatUltAtualizacao { get; set; }
    }
}
