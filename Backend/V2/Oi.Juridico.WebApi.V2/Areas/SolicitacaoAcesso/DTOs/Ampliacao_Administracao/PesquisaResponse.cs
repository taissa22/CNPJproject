using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using System.Text.Json.Serialization;

namespace Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.DTOs.Ampliacao_Administracao
{
    public class PesquisaResponse
    {
        public int Id { get; set; }
        public string UsuarioSolicitante { get; set; } = "";
        public DateTime DataSolicitacao { get; set; }
        public string Escritorios { get; set; } = "";
        public string UsuarioAdministrador { get; set; } = "";
        public string Gestores { get; set; } = "";
        [JsonIgnore]
        public List<int> CodProfissional { get; internal set; } = new();

        public byte Status { get; set; }
        public string UsuarioSolicitado { get; set; } = "";
    }
}
