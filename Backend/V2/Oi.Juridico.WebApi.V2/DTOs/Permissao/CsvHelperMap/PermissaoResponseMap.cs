using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.DTOs.Permissao.CsvHelperMap
{
    public class PermissaoResponseMap : ClassMap<PermissaoResponse>
    {
        public PermissaoResponseMap()
        {
            Map(row => row.PermissaoId).Name("Código");
            Map(row => row.Descricao).Name("Descrição");
            Map(row => row.Caminho).Name("Caminho");
            Map(row => row.Modulos).Name("Módulos");
        }

    }
}
