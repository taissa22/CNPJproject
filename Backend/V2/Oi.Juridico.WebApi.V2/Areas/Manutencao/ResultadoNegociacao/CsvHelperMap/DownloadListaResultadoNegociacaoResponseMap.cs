using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.CsvHelperMap
{
    public class DownloadListaResultadoNegociacaoResponseMap : ClassMap<ListaResultadoNegociacaoResponse>
    {
        public DownloadListaResultadoNegociacaoResponseMap()
        {
            Map(row => row.CodResultadoNegociacao).Name("Código Resultado de Negociação");
            Map(row => row.DscResultadoNegociacao).Name("Descrição");
            Map(row => row.DscTipoNegociacao).Name("Tipo de Negociação");
            Map(row => row.DscTipoResultado).Name("Tipo de Resultado");
            Map(row => row.IndAtivo).Name("Status");
        }
    }
}
