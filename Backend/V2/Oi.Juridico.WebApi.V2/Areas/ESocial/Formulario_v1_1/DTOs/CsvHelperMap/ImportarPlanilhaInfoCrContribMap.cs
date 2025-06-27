using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaInfoCrContribMap : ClassMap<EsF2501InfocrcontribImportRequestDTO>
    {
        public ImportarPlanilhaInfoCrContribMap()
        {
            Map(row => row.CalctribPerref).Index(0).Default("01/01/1901", true);
            Map(row => row.InfocrcontribTpcr).Index(1);
            Map(row => row.InfocrcontribVrcr).Index(2);
        }
    }    

}
