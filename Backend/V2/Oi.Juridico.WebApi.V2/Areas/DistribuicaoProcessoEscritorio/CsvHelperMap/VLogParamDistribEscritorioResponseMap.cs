using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.CsvHelperMap
{
    public class VLogParamDistribEscritorioResponseMap : ClassMap<VLogParamDistribEscritorio>
    {
        public VLogParamDistribEscritorioResponseMap()
        {
            Map(row => row.Operacao).Name("Operação").TypeConverter<OperacaoConverter>(); ;
            Map(row => row.DatLog).Name("Data e Hora da Operação").TypeConverter<DataHoraConverter>(); 
            Map(row => row.CodUsuario).Name("Código Usuário Log");
            Map(row => row.NomeUsuario).Name("Nome Usuário Log");
            Map(row => row.CodParamDistribuicao).Name("Código da Parametrização");
            Map(row => row.CodProfissionalA).Name("Código do Escritório Antes");
            Map(row => row.CodProfissionalD).Name("Código do Escritório Depois");
            Map(row => row.NomProfissionalA).Name("Nome do Escritório Antes");
            Map(row => row.NomProfissionalD).Name("Nome do Escritório Depois");
            Map(row => row.SolicitanteA).Name("Solicitante Antes");
            Map(row => row.SolicitanteD).Name("Solicitante Depois");
            Map(row => row.DatVigenciaInicialA).Name("Vigência Inicial Antes");
            Map(row => row.DatVigenciaInicialD).Name("Vigência Inicial Depois");
            Map(row => row.DatVigenciaFinalA).Name("Vigência Final Antes");
            Map(row => row.DatVigenciaFinalD).Name("Vigência Final Depois");
            Map(row => row.PorcentagemProcessosA).Name("Percentual Processos Antes");
            Map(row => row.PorcentagemProcessosD).Name("Percentual Processos Depois");
            Map(row => row.PrioridadeA).Name("Prioridade Antes");
            Map(row => row.PrioridadeD).Name("Prioridade Depois");
        }
    }
}
