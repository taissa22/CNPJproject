using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.DTO_s;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.CsvHelperMap
{
    public class DownloadListaEmpresasContratadasResponseMap : ClassMap<DownloadListaEmpresasContratadasResponse>
    {
        public DownloadListaEmpresasContratadasResponseMap()
        {
            Map(row => row.CodEmpresaContratada).Name("Código Empresa Contratada");
            Map(row => row.NomEmpresaContratada).Name("Razão Social");
            Map(row => row.Matriculas).Name("Login Terceiro");
        }
    }
}
