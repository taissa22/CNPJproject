using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs
{
    public class ListarPautaComposicaoAudienciaResponse
    {
        public string Hora { get; set; }
        public string Tipo { get; set; }
        public string Juizado { get; set; }
        public string ProcessoEmpresaGrupo { get; set; }
        public string Assunto { get; set; }
        public string Pedido { get; set; }
        public string ValorCausa { get; set; }
        public string Terceirizado { get; set; }
        public string CodEstado { get; set; }
        public short CodVara { get; set; }
        public string NomComarca { get; set; }
        public string NomTipoVara { get; set; }
        public string NroProcessoCartorio { get; set; }
        public string NomParte { get; set; }
        public int CodProcesso { get; set; }
        public int CodParte { get; set; }
        public int SeqAudiencia { get; set; }
        public int? AlocacaoTipo { get; set; }
    }
}
