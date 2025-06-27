using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.Contextos.V2.ManutencaoContratoEscritorioContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.CsvHelperMap
{
    public class LogContratoEscritorioEstadoResponseMap : ClassMap<LogContratoEscritorioEstadoResponse>
    {
        public LogContratoEscritorioEstadoResponseMap()
        {
            Map(row => row.Operacao).Name("Operação").TypeConverter<OperacaoConverter>();
            Map(row => row.DatLog).Name("Data e Hora da Operação");
            Map(row => row.CodUsuario).Name("Código Usuário Log").TypeConverter<LongNumberConverter>();
            Map(row => row.CodContratoEscritorio).Name("Código do Contrato Escritório").TypeConverter<LongNumberConverter>();          
            Map(row => row.CodProfissionalA).Name("Código do Escritório Antes");
            Map(row => row.CodProfissionalD).Name("Código do Escritório Depois");
            Map(row => row.CodEstadoA).Name("Código do Estado Antes");
            Map(row => row.CodEstadoD).Name("Código do Estado Depois");
        }
    }
}
