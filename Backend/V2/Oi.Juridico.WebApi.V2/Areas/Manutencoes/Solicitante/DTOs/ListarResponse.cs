using DocumentFormat.OpenXml.Office2010.Excel;
using HashidsNet;
using Newtonsoft.Json;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante.DTOs
{
    public class ListarResponse
    {
        public int? CodSolicitante { get; set; }
        public string CodSolicitanteHash { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
