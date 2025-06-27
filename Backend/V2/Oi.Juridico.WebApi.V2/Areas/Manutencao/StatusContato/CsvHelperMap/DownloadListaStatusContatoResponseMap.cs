using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.DTO_s;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.CsvHelperMap
{
    public class DownloadListaStatusContatoResponseMap : ClassMap<StatusContatoResponse>
    {
        public DownloadListaStatusContatoResponseMap()
        {
            Map(row => row.CodStatusContato).Name("Código Status de Contato");
            Map(row => row.DscStatusContato).Name("Descrição do Status");
            Map(row => row.IndContatoRealizado).Name("Contato Realizado");
            Map(row => row.IndAcordoRealizado).Name("Acordo Realizado");
            Map(row => row.IndAtivo).Name("Status");
        }
    }
}
