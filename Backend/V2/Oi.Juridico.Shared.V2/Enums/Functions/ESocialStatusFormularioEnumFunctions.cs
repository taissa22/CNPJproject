using System.Collections.Generic;

namespace Oi.Juridico.Shared.V2.Enums.Functions
{
    public static class ESocialStatusFormularioEnumFunctions
    {
        public static IEnumerable<EsocialStatusFormulario> ListaStatusNaoPermitemAlteracaoStatusReclamante()
        {
            return new[]
            {
                EsocialStatusFormulario.ProntoParaEnvio,
                EsocialStatusFormulario.Processando,
                EsocialStatusFormulario.EnviadoESocial,
                EsocialStatusFormulario.RetornoESocialOk,
                EsocialStatusFormulario.ErroProcessamento,
                EsocialStatusFormulario.PendenteAcaoFPW,
                EsocialStatusFormulario.Exclusao3500Solicitada,
                EsocialStatusFormulario.Exclusao3500NaoOk,
                EsocialStatusFormulario.Exclusao3500Enviada,
                EsocialStatusFormulario.Excluido3500
            };
        }

        public static IEnumerable<EsocialStatusFormulario> ListaStatusNaoPermitemAlteracaoStatusFormulario()
        {
            return new[]
            {
                EsocialStatusFormulario.Processando,
                EsocialStatusFormulario.EnviadoESocial,
                EsocialStatusFormulario.RetornoESocialOk,
                EsocialStatusFormulario.PendenteAcaoFPW,
                EsocialStatusFormulario.Exclusao3500Solicitada,
                EsocialStatusFormulario.Exclusao3500NaoOk,
                EsocialStatusFormulario.Exclusao3500Enviada,
                EsocialStatusFormulario.Excluido3500
            };
        }

        public static IEnumerable<EsocialStatusFormulario> ListaStatusNaoPermitemExclusaoFormulario()
        {
            return new[]
            {
                EsocialStatusFormulario.Processando,
                EsocialStatusFormulario.EnviadoESocial,
                EsocialStatusFormulario.RetornoESocialOk,
                EsocialStatusFormulario.PendenteAcaoFPW,
                EsocialStatusFormulario.Exclusao3500Solicitada,
                EsocialStatusFormulario.Exclusao3500NaoOk,
                EsocialStatusFormulario.Exclusao3500Enviada,
                EsocialStatusFormulario.Excluido3500
            };
        }

        public static IEnumerable<EsocialStatusFormulario> ListaStatusNaoPermiteSolicitarExclusaoS3500()
        {
            return new[]
            {
                EsocialStatusFormulario.Processando,
                EsocialStatusFormulario.Exclusao3500Solicitada,
                EsocialStatusFormulario.Exclusao3500Enviada,
                EsocialStatusFormulario.Excluido3500
            };
        }

        public static IEnumerable<EsocialStatusFormulario> ListaStatusNaoPermiteCancelarSolicitacaoExclusaoS3500()
        {
            return new[]
            {
                EsocialStatusFormulario.Processando,
                EsocialStatusFormulario.Exclusao3500Enviada,
                EsocialStatusFormulario.Excluido3500
            };
        }
    }
}
