using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ExportaF2500IdePeriodoMap : ClassMap<EsF2500IdeperiodoDTO>
    {
        public ExportaF2500IdePeriodoMap()
        {
            Map(row => row.IdeperiodoPerref).Index(0).Convert(x => $"\t{x.Value.IdeperiodoPerref.FormatData()}").Name("Período Referência");
            Map(row => row.BasecalculoVrbccpmensal).Index(0).Name("Base de Cálculo Inss Mensal");
            Map(row => row.BasecalculoVrbccp13).Index(0).Name("Base de Cálculo Inss 13º");
            Map(row => row.BasecalculoVrbcfgts).Index(0).Name("Base de Cálculo FGTS");
            Map(row => row.BasecalculoVrbcfgts13).Index(0).Name("Base de Cálculo FGTS 13º");
            Map(row => row.InfofgtsVrbcfgtsguia).Index(0).Name("Base de Cálculo FGTS GUIA");
            Map(row => row.InfofgtsVrbcfgts13guia).Index(0).Name("Base de Cálculo FGTS 13º GUIA");
            Map(row => row.BasemudcategVrbccprev).Index(0).Name("Valor Base Cálc. INSS Mud. Cat.");
            Map(row => row.InfofgtsPagdireto).Index(0).Convert(x => x.Value.InfofgtsPagdireto == "N" ? "Não" : x.Value.InfofgtsPagdireto == "S" ? "Sim" : string.Empty).Name("FGTS Pago Direto Trabalhador");
            Map(row => row.BasemudcategCodcategDesc).Index(0).Name("Categoria");
            Map(row => row.InfoagnocivoGrauexpDesc).Index(0).Name("Grau de Exposição");
        }
    }
}
