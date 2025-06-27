using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum EsocialStatusFormulario
    {        
        [Display(Description = "Não iniciado")]
        NaoIniciado = 0,
        [Display(Description = "Rascunho")]
        Rascunho = 1,
        [Display(Description = "Pronto para envio")]
        ProntoParaEnvio = 2,
        [Display(Description = "Enviado para o FPW")]
        EnviadoESocial = 3,
        [Display(Description = "Retorno eSocial Ok")]
        RetornoESocialOk = 4,
        [Display(Description = "Retorno eSocial com Críticas")]
        RetornoESocialNaoOk = 5,
        [Display(Description = "Erro Processamento")]
        ErroProcessamento = 6,
        [Display(Description = "Reconsultar no FPW")]
        RetornoOkSemRecibo = 7,
        [Display(Description = "Processando")]
        Processando = 10,
        [Display(Description = "Pendente Ação FPW")]
        PendenteAcaoFPW = 11,
        [Display(Description = "Excluído 3500")]
        Excluido3500 = 12,
        [Display(Description = "Exclusão 3500 Solicitada")]
        Exclusao3500Solicitada = 13,
        [Display(Description = "Exclusão 3500 Enviada")]
        Exclusao3500Enviada = 14,
        [Display(Description = "Exclusão 3500 não Ok")]
        Exclusao3500NaoOk = 15,
        [Display(Description = "Exclusão 3500 Erro Processamento")]
        Exclusao3500ErroProcessamento = 16,
        [Display(Description = "Reconsultar no FPW - S3500")]
        Exclusao3500ReconsultarFPW = 17

    }

}
