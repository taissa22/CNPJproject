using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.DTO_s;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.CsvHelperMap
{
    public class DownloadLogStatusContatoResponseMap : ClassMap<DownloadLogStatusContatoResponse>
    {
        public DownloadLogStatusContatoResponseMap()
        {
            Map(row => row.CodStatusContato).Name("Código Status do Contato");
            Map(row => row.Operacao).Name("Operação").TypeConverter<OperacaoConverter>();
            Map(row => row.DatLog).Name("Data e Hora da Operação");
            Map(row => row.CodUsuario).Name("Código Usuário Log");
            Map(row => row.DscStatusContatoA).Name("Descrição do Status Antes");
            Map(row => row.DscStatusContatoD).Name("Descrição do Status Depois");
            Map(row => row.IndContatoRealizadoA).Name("Contato Realizado Antes");
            Map(row => row.IndContatoRealizadoD).Name("Contato Realizado Depois");
            Map(row => row.IndAcordoRealizadoA).Name("Acordo Realizado Antes");
            Map(row => row.IndAcordoRealizadoD).Name("Acordo Realizado Depois");
            Map(row => row.IndAtivoA).Name("Acordo Realizado Antes");
            Map(row => row.IndAtivoA).Name("Status Antes");
            Map(row => row.IndAtivoD).Name("Status Depois");
        }
    }
}
