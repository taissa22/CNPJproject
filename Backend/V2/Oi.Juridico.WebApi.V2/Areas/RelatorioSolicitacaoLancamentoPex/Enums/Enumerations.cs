using System.ComponentModel;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Enums
{
    public enum TipoSolicitacaoLancamento {
        [Description("Despesa de Custas")]
        DespesaDeCustas = 1,
        [Description("Desbloqueio")]
        Desbloqueio = 2,
        [Description("Depósito Voluntário")]
        DepositoVoluntario = 3,
        [Description("Honorário Pericial")]
        HonorarioPericial = 4,
        [Description("Pagamento com Baixa de Garantia")]
        PagamentoComBaixaDeGarantia = 5,
        [Description("Pagamento sem Baixa de Garantia")]
        PagamentoSemBaixaDeGarantia = 6,
        [Description("Recuperação para Oi")]
        RecuperacaoParaOi = 7,
        [Description("Pagamento com Baixa de Garantia Acordo")]
        PagamentoComBaixaDeGarantiaAcordo = 8
    }

    public enum StatusSolicitacaoLancamento {
        [Description("Pendente de Envio")]
        PendenteEnvio = 2,
        [Description("Solicitação Enviada para a Oi")]
        EnviadoParaOi = 3,
        [Description("Devolvido pela Oi")]
        DevolvidoPelaOi = 4,
        [Description("Solicitação Cancelada")]
        Cancelado = 5,
        [Description("Solicitação Efetivada")]
        Efetivada = 6,
        [Description("Em Validação")]
        EmValidacao = 7,
        [Description("Pendente de Análise")]
        PendenteAnalise = 8,
        [Description("Solicitação Efetivada Parcialmente")]
        ParcialmenteEfetivada = 9,
        [Description("Devolvido pela Oi - Efetivado Parcialmente")]
        DevolvidoPelaOiEfetivadoParcialmente = 10,
        [Description("Solicitação Enviada para a Oi - Efetivado Parcialmente")]
        EnviadoParaOiEfetivadoParcialmente = 11
    }


 




}
