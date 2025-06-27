using CsvHelper;
using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaInfoCrContribColunasMap : ClassMap<EsF2501InfocrcontribImportColunaRequestDTO>
    {
        public ImportarPlanilhaInfoCrContribColunasMap()
        {
            Map(row => row.CalctribPerref).Index(0).Default("01/01/1901", true);
            Map(row => row.ContribSocialSegurado).Index(1);
            Map(row => row.ContribSocialEmpregador).Index(2);
            Map(row => row.RatSat).Index(3);
            Map(row => row.SalariaEducacao).Index(4);
            Map(row => row.INCRA).Index(5);
            Map(row => row.SENAI).Index(6);
            Map(row => row.SESI).Index(7);
            Map(row => row.SEBRAE).Index(8);
        }
    }    

}
