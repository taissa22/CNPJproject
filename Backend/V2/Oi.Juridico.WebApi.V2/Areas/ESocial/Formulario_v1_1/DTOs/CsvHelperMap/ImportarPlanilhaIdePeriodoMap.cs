using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaIdePeriodoMap : ClassMap<EsF2500IdeperiodoRequestDTO>
    {
        public ImportarPlanilhaIdePeriodoMap()
        {
            Map(row => row.IdeperiodoPerref).Index(0).Default("01/01/1901",true);
            Map(row => row.BasecalculoVrbccpmensal).Index(1);
            Map(row => row.BasecalculoVrbccp13).Index(2);
            Map(row => row.BasecalculoVrbcfgts).Index(3);
            Map(row => row.BasecalculoVrbcfgts13).Index(4);
            Map(row => row.InfofgtsVrbcfgtsguia).Index(5);
            Map(row => row.InfofgtsVrbcfgts13guia).Index(6);
            Map(row => row.InfofgtsPagdireto).Index(7);
            Map(row => row.BasemudcategVrbccprev).Index(8);
            Map(row => row.BasemudcategCodcateg).Index(9);
            Map(row => row.InfoagnocivoGrauexp).Index(10);
        }
    }
}
