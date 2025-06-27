using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaIdeImpostoMap : ClassMap<EsF2501InfocrirrfRequestDTO>
    {
        public ImportarPlanilhaIdeImpostoMap()
        {
            Map(row => row.InfocrcontribTpcr).Index(0);
            Map(row => row.InfocrcontribVrcr).Index(1);
        }
    }
}
