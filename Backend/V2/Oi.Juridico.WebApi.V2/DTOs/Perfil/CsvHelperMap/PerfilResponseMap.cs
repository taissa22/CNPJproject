using CsvHelper.Configuration;

namespace Oi.Juridico.WebApi.V2.DTOs.Perfil.CsvHelperMap
{
    public class PerfilResponseMap : ClassMap<PerfilResponse>
    {
        public PerfilResponseMap()
        {
            Map(row => row.Nome).Name("Nome");
            Map(row => row.Restrito).Name("Restrito");
            Map(row => row.Descricao).Name("Descrição");
            Map(row => row.GestorDefault).Name("Gestor Default");
            Map(row => row.Modulo).Name("Módulo");
            Map(row => row.NomePermissao).Name("Nome da Permissão");
        }
    }
}
