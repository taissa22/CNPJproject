namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusNegociacoes.DTO
{
    public class DeparaStatusNegociacoesResponse
    {
        public decimal Id { get; set; }

        public short? IdStatusApp { get; set; }

        public short? IdSubStatusApp { get; set; }

        public short? IdStatusSisjur { get; set; }

        public string DescricaoStatusApp { get; set; } = "";

        public string DescricaoSubStatusApp { get; set; } = "";

        public string DescricaoStatusSisjur { get; set; } = "";

        //public string CriaNegociacoes { get; set; } = "";

        public byte? IdTipoProcesso { get; set; }

        public string DescricaoTipoProcesso { get; set; } = "";
    }
}
