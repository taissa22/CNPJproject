using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaIdePeriodoMap : ClassMap<EsF2500IdeperiodoRequestDTO>
    {
        public ImportarPlanilhaIdePeriodoMap()
        {
            Map(row => row.IdeperiodoPerref).Index(0).Default("01/01/1901", true);
            Map(row => row.BasecalculoVrbccpmensal).Index(1);
            Map(row => row.BasecalculoVrbccp13).Index(2);
            Map(row => row.BasemudcategVrbccprev).Index(3);
            Map(row => row.BasemudcategCodcateg).Index(4);
            Map(row => row.InfoagnocivoGrauexp).Index(5);
            Map(row => row.InfofgtsVrbcfgtsproctrab).Index(6);
            Map(row => row.InfofgtsVrbcfgtssefip).Index(7);
            Map(row => row.InfofgtsVrbcfgtsdecant).Index(8);
        }
    }
}
