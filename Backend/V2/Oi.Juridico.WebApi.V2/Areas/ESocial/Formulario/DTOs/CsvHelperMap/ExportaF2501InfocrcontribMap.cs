using CsvHelper.Configuration;
using Oi.Juridico.Shared.V2.Extensions;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs.CsvHelperMap
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
