using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
{
    public class ExportaF2501InfocrirrfMap : ClassMap<EsF2501InfocrirrfDTO>
    {
        public ExportaF2501InfocrirrfMap()
        {
            Map(row => row.DescricaoTpcr).Index(0).Name("Código Receita (CR) IRRF");
            Map(row => row.InfocrcontribVrcr).Index(1).Name("Valor Correspondente (CR) IRRF - Rendimento Mensal");
            Map(row => row.InfocrcontribVrcr13).Index(2).Name("Valor Correspondente (CR) IRRF - 13º");
        }
    }
}
