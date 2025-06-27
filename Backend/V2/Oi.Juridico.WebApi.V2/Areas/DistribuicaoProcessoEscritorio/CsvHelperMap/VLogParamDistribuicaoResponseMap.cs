using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.CsvHelperMap
{
    public class VLogParamDistribuicaoResponseMap : ClassMap<VLogParamDistribuicao>
    {
        public VLogParamDistribuicaoResponseMap()
        {
            Map(row => row.Operacao).Name("Operação").TypeConverter<OperacaoConverter>();
            Map(row => row.DatLog).Name("Data e Hora da Operação").TypeConverter<DataHoraConverter>();
            Map(row => row.CodUsuario).Name("Código Usuário Log");
            Map(row => row.NomeUsuario).Name("Nome Usuário Log");
            Map(row => row.CodParamDistribuicao).Name("Código da Parametrização");
            Map(row => row.CodEstadoA).Name("UF Antes");
            Map(row => row.CodEstadoD).Name("UF Depois");
            Map(row => row.ComarcaA).Name("Comarca Antes");
            Map(row => row.ComarcaD).Name("Comarca Depois");
            Map(row => row.VaraA).Name("Vara Antes");
            Map(row => row.VaraD).Name("Vara Depois");
            Map(row => row.NaturezaA).Name("Natureza Antes");
            Map(row => row.NaturezaD).Name("Natureza Depois");
            Map(row => row.EmpresaCentralizadoraA).Name("Empresa Centralizadora Antes");
            Map(row => row.EmpresaCentralizadoraD).Name("Empresa Centralizadora Depois");
            Map(row => row.IndAtivoA).Name("Status Antes");
            Map(row => row.IndAtivoD).Name("Status Depois");
        }
    }
}
