namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class ListarAnexosResponse
    {
        public int CodParamDistribuicao { get; internal set; }
        public int IdAnexoDistEscritorio { get; internal set; }
        public string NomeArquivo { get; internal set; } = "";
        public string Comentario { get; internal set; } = "";
        public DateTime DataUpload { get; internal set; }
        public string NomeUsuario { get; internal set; } = "";
    }
}
