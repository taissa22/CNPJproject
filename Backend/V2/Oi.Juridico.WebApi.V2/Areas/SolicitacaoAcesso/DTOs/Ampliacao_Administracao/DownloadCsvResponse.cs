using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using System.Text.Json.Serialization;

namespace Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.DTOs.Ampliacao_Administracao
{
    public class DownloadCsvResponse
    {
        public string UsuarioSolicitante { get; set; } = "";
        public string Login { get; set; } = "";
        public string Email { get; set; } = "";
        public string Escritorio { get; set; } = "";
        public string Origem { get; set; } = "";
        public string SituacaoOrigem { get; set; } = "";
        public string DataHoraSolicitacao { get; set; } = "";
        public string AdministradorAprovador { get; set; } = "";
        public string DataHoraReativacao { get; set; } = "";
        public string DataValidadeSenha { get; set; } = "";
        public string Perfil { get; set; } = "";
        [JsonIgnore]
        public string CodPerfil { get; set; } = "";
        public string GestorResponsavel { get; set; } = "";
        public string Aprovado { get; set; } = "";
        public string DataHora { get; set; } = "";
        public string Observacao { get; set; } = "";
        [JsonIgnore]
        public string DescricaoRejeicao { get; set; } = "";
        [JsonIgnore]
        public DateTime? DatAcaoGestor { get; set; }
        [JsonIgnore]
        public DateTime? DatAcaoAdministrador { get; set; }
        [JsonIgnore]
        public List<int> CodProfissional { get; internal set; } = new();
        public string SituacaoOrigemDescricao
        {
            get
            {
                return string.IsNullOrEmpty(SituacaoOrigem)
                ? "Sem Situação Origem"
                : EnumHelper.GetEnumText((SituacaoUsuarioArquivoDeImportacaoEnum)Enum.Parse(typeof(SituacaoUsuarioArquivoDeImportacaoEnum), SituacaoOrigem));
            }
        }

        public string StatusSolicitacao
        {
            get
            {
                return string.IsNullOrEmpty(StatusRenovacaoAcesso)
                ? EnumHelper.GetEnumText((StatusSolicitacaoDeAcessoEnum)Enum.Parse(typeof(StatusSolicitacaoDeAcessoEnum), StatusDaSolicitacaoDeAcessoEnum))
                : StatusRenovacaoAcesso;
            }
        }

        public string StatusRenovacaoAcesso { get; set; } = "";
        public string StatusDaSolicitacaoDeAcessoEnum { get; set; } = "";
        public string UsuarioSolicitado { get; internal set; } = "";
        [JsonIgnore]
        public string IndAprovacaoGestor { get; internal set; } = "";
        [JsonIgnore]
        public string IndAprovacaoAdministrador { get; internal set; } = "";
    }
}
