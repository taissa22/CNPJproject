using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaCalcTribMap : ClassMap<EsF2501CalctribRequestDTO>
    {
        public ImportarPlanilhaCalcTribMap()
        {
            Map(row => row.CalctribPerref).Index(0).Default("01/01/1901", true); ;
            Map(row => row.CalctribVrbccpmensal).Index(1);
            Map(row => row.CalctribVrbccp13).Index(2);
            Map(row => row.CalctribVrrendirrf).Index(3);
            Map(row => row.CalctribVrrendirrf13).Index(4);  
        }
    }
}
