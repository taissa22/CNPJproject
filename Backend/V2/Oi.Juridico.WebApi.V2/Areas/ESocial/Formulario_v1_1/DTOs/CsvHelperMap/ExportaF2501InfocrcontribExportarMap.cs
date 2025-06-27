using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs.CsvHelperMap
{
    public class ExportaF2501InfocrcontribExportarMap : ClassMap<EsF2501InfocrcontribExportarDTO>
    {
        public ExportaF2501InfocrcontribExportarMap()
        {
            Map(row => row.CalctribPerref).Index(1).Convert(x => $"\t{x.Value.CalctribPerref.FormatData()}").Name("Período Referência");
            Map(row => row.DescricaoTpcr).Index(2).Name("Código Receita (CR) Contrib. Sociais");
            Map(row => row.InfocrcontribVrcr).Index(3).Name("Valor Correspondente (CR) Contrib. Sociais");
        }
    }
}
