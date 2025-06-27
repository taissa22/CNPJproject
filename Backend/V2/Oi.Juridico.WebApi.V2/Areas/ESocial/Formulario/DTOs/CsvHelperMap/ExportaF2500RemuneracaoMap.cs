using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ExportaF2500RemuneracaoMap : ClassMap<EsF2500RemuneracaoDTO>
    {
        public ExportaF2500RemuneracaoMap()
        {

            Map(row => row.RemuneracaoDtremun).Index(0).Convert(convertToStringFunction: x =>
            {
                var dt = (DateTime)x.Value.RemuneracaoDtremun!;
                return $"\t{dt.ToString("dd/MM/yyyy")}";
            }).Name("Data de Vigência");
            Map(row => row.RemuneracaoVrsalfx).Index(0).Name("Salário Base do Trabalhador");
            Map(row => row.DescricaoUnidadePagamento).Index(0).Name("Unidade de Pagamento");
            Map(row => row.RemuneracaoDscsalvar).Index(0).Name("Descrição Salário Variável");



        }
    }
}
