using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaIdeImpostoMap : ClassMap<EsF2501InfocrirrfRequestDTO>
    {
        public ImportarPlanilhaIdeImpostoMap()
        {
            Map(row => row.InfocrcontribTpcr).Index(0);
            Map(row => row.InfocrcontribVrcr).Index(1);
            Map(row => row.InfocrcontribVrcr13).Index(2);
        }
    }
}
