using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaCalcTribMap : ClassMap<EsF2501CalctribRequestDTO>
    {
        public ImportarPlanilhaCalcTribMap()
        {
            Map(row => row.CalctribPerref).Index(0).Default("01/01/1901", true); ;
            Map(row => row.CalctribVrbccpmensal).Index(1);
            Map(row => row.CalctribVrbccp13).Index(2);
        }
    }
}
