using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ExportaF2501PlanilhaCalcTribMap : ClassMap<EsF2501CalctribDTO>
    {
        public ExportaF2501PlanilhaCalcTribMap()
        {
            Map(row => row.CalctribPerref).Index(0).Convert(x => $"\t{x.Value.CalctribPerref.FormatData()}").Name("Período Referência");
            Map(row => row.CalctribVrbccpmensal).Index(1).Name("Base Calc. Contrib. Previdenciária");
            Map(row => row.CalctribVrbccp13).Index(2).Name("Base Calc. Contrib. Previdenciária 13º");
            Map(row => row.CalctribVrrendirrf).Index(3).Name("Rendimento Tributável IR");
            Map(row => row.CalctribVrrendirrf13).Index(4).Name("Rendimento Tributável IR 13º");
        }
    }
}
