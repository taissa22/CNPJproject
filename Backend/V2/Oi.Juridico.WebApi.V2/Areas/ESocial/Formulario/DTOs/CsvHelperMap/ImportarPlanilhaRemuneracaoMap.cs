using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ImportarPlanilhaRemuneracaoMap : ClassMap<EsF2500RemuneracaoRequestDTO>
    {
        public ImportarPlanilhaRemuneracaoMap()
        {
            Map(row => row.RemuneracaoDtremun).Index(0);
            Map(row => row.RemuneracaoVrsalfx).Index(1);
            Map(row => row.RemuneracaoUndsalfixo).Index(2);
            Map(row => row.RemuneracaoDscsalvar).Index(3);
        }
    }
}
