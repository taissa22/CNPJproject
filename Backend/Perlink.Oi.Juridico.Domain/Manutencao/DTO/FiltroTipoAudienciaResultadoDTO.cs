
namespace Perlink.Oi.Juridico.Domain.Manutencao.DTO
{
    public class FiltroTipoAudienciaResultadoDTO
    {
        public long CodTipoAudiencia { get; set; }

        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public bool EstaAtivo { get; set; }

        public string TipoProcesso { get; set; }
    }
}
