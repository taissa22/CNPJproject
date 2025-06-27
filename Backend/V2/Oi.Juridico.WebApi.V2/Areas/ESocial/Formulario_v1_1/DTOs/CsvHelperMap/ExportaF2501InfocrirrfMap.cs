using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ExportaF2501InfocrirrfMap : ClassMap<EsF2501InfocrirrfDTO>
    {
        public ExportaF2501InfocrirrfMap()
        {
            Map(row => row.DescricaoTpcr).Index(0).Name("Código Receita (CR) IRRF");
            Map(row => row.InfocrcontribVrcr).Index(1).Name("Valor Correspondente (CR) IRRF");
        }
    }
}
