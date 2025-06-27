using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public partial class VEsAcompanhamentoDTO
    {
        public string TipoFormulario { get; set; } = string.Empty;
        public long? IdFormulario { get; set; }
        public int? CodProcesso { get; set; }
        public long? CodParte { get; set; }
        public byte? StatusFormulario { get; set; }
        public string? DescStatusFormulario
        {
            get
            {
                var desc = StatusFormulario.HasValue ? EnumExtension.ToEnum<EsocialStatusFormulario>(StatusFormulario.Value).Descricao() : string.Empty;

                return desc;
            }
        }

       


        public string LogCodUsuario { get; set; } = string.Empty;
        public DateTime? LogDataOperacao { get; set; }
        public string InfoprocessoNrproctrab { get; set; } = string.Empty;
        public string NomParte { get; set; } = string.Empty;
        public string CpfParte { get; set; } = string.Empty;
        public int? IdEsEmpresaAgrupadora { get; set; }
        public short? CodComarca { get; set; }
        public short? CodVara { get; set; }
        public short? CodTipoVara { get; set; }
        public string NomComarca { get; set; } = string.Empty;
        public string NomTipoVara { get; set; } = string.Empty;
        public long? CodParteEmpresa { get; set; }
        public string CgcParteEmpresa { get; set; } = string.Empty;
        public string NomParteEmpresa { get; set; } = string.Empty;
        public string IndProcessoAtivo { get; set; } = string.Empty;
        public string IndProprioTerceiro { get; set; } = string.Empty;
        public string CodEstado { get; set; } = string.Empty;
        public DateTime? InfoprocjudDtsent { get; set; }

        public byte? TipoFormularioTipo { get; set; }
        public string FinalizadoEscritorio { get; set; } = string.Empty;
        public string FinalizadoContador { get; set; } = string.Empty;

        public string NrRecibo { get; set; } = string.Empty;

        public string NomEscritorio { get; set; } = string.Empty;
        public string NomContador { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public string PeriodoApuracao {  get; set; } = string.Empty;
        public bool ExibirHistorico { get; set; } = false;
        public string ExclusaoNrrecibo { get; set; } = string.Empty;
        public bool ExibirRetorno { get; set; }
        public string NomeArquivoEnviado { get; set; } = string.Empty;
        public string? VersaoEsocial { get; set; }
        public string NomeArquivoRetornado { get; set; } = string.Empty;
        public bool ehDataFutura { get; set; } = false;
    }
}