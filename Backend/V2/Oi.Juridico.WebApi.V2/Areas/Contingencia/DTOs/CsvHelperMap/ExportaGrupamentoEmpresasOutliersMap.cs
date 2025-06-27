using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.DTOs.CsvHelperMap
{
    public class ExportaGrupamentoEmpresasOutliersResponseMap : ClassMap<ExportaGrupamentoEmpresasOutliersResponse>
    {
        public ExportaGrupamentoEmpresasOutliersResponseMap()
        {
            Map(row => row.CodGrupoEmepresasOutliers).Name("Código Grupamento de Empresas Outliers");
            Map(row => row.NomGrupoEmpresasOutliers).Name("Nome Grupamento de Empresas Outliers");
            Map(row => row.CodParte).Name("Código Empresa do Grupo");
            Map(row => row.NomParte).Name("Nome Empresa do Grupo");
        }
    }
}
