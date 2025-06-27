using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ExportaF2501InfocrcontribMap : ClassMap<EsF2501InfocrcontribDTO>
    {
        public ExportaF2501InfocrcontribMap()
        {
            Map(row => row.DescricaoTpcr).Index(1).Name("Código Receita (CR) Contrib. Sociais");
            Map(row => row.InfocrcontribVrcr).Index(2).Name("Valor Correspondente (CR) Contrib. Sociais");
        }
    }
}
