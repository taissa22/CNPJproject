using Oi.Juridico.Shared.V2.Enums;
using System.ComponentModel;

namespace Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.DTOs.Ampliacao_Administracao
{
    public class FiltroAdministracaoSolicitacaoAmpliacao
    {
        public string UsuarioSolicitante { get; set; } = "";
        public string UsuarioSolicitado { get; set; } = "";
        public string Login { get; set; } = "";
        public DateTime? DataSolicitacaoIni { get; set; }
        public DateTime? DataSolicitacaoFim { get; set; }
        public long CodigoEscritorio { get; set; }
        public List<int> ListaDeStatus { get; set; } = new();
        public TipoPesquisaEnum TiposDePesquisaEmNomeSolicitante { get; set; }
        public TipoPesquisaEnum TiposDePesquisaEmNomeSolicitado { get; set; }
        public TipoPesquisaEnum TiposDePesquisaEmLogin { get; set; }
        public int Pagina { get; set; }
        public string FiltroDataDe { get; set; } = "";
        public string FiltroDataAte { get; set; } = "";

    }
}
