using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante.CsvHelperMap
{
    public class ListarResponseMap : ClassMap<ListarResponse>
    {
        public ListarResponseMap()
        {
            Map(row => row.CodSolicitante).Name("Código");
            Map(row => row.Nome).Name("Nome");
            Map(row => row.Email).Name("Email");
        }
    }
}
