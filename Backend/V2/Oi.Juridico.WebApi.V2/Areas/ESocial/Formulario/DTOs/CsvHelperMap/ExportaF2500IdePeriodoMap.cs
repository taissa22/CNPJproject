using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ExportaF2500IdePeriodoMap : ClassMap<EsF2500IdeperiodoDTO>
    {
        public ExportaF2500IdePeriodoMap()
        {
            Map(row => row.IdeperiodoPerref).Index(0).Convert(x => $"\t{x.Value.IdeperiodoPerref.FormatData()}").Name("Período Referência");
            Map(row => row.BasecalculoVrbccpmensal).Index(1).Name("Base de Cálculo Inss Mensal");
            Map(row => row.BasecalculoVrbccp13).Index(2).Name("Base de Cálculo Inss 13º");
            Map(row => row.BasemudcategVrbccprev).Index(3).Name("Valor Base Cálc. INSS Mud. Cat.");
            Map(row => row.BasemudcategCodcategDesc).Index(4).Name("Categoria");
            Map(row => row.InfoagnocivoGrauexpDesc).Index(5).Name("Grau de Exposição");
            Map(row => row.InfofgtsVrbcfgtsproctrab).Index(6).Name("Base Cálculo ainda não declarada em SEFIP ou no eSocial");
            Map(row => row.InfofgtsVrbcfgtssefip).Index(7).Name("Base Cálculo declarada apenas em SEFIP");
            Map(row => row.InfofgtsVrbcfgtsdecant).Index(8).Name("Base Cálculo declarada no eSocial e ainda não recolhida");
        }
    }
}
