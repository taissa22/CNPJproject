using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.Contextos.V2.ResultadoNegociacaoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s
{
    public class DownloadLogResultadoNegociacaoResponseMap : ClassMap<LogResultadoNegociacao>
    {
        public DownloadLogResultadoNegociacaoResponseMap()
        {
            Map(row => row.CodResultadoNegociacao).Name("Código da Negociação");
            Map(row => row.Operacao).Name("Operação").TypeConverter<OperacaoConverter>();
            Map(row => row.DatLog).Name("Data e Hora da Operação");
            Map(row => row.CodUsuario).Name("Código Usuário Log");
            Map(row => row.DscResultadoNegociacaoA).Name("Descrição Antes");
            Map(row => row.DscResultadoNegociacaoD).Name("Descrição Depois");
            Map(row => row.IndNegociacaoA).Name("Tipo de Negociação - Ilha de Negociação Antes");
            Map(row => row.IndNegociacaoD).Name("Tipo de Negociação - Ilha de Negociação Depois");
            Map(row => row.IndPosSentencaA).Name("Tipo de Negociação - Pós Sentença Antes");
            Map(row => row.IndPosSentencaD).Name("Tipo de Negociação - Pós Sentença Depois");
            Map(row => row.CodTipoResultadoA).Name("Tipo Resultado Antes");
            Map(row => row.CodTipoResultadoD).Name("Tipo Resultado Depois");
            Map(row => row.IndAtivoA).Name("Status Antes");
            Map(row => row.IndAtivoD).Name("Status Depois");
        }
    }
}
