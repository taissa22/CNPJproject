using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    public sealed class StatusPagamento
    {
        private StatusPagamento(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        #region enum

        public static StatusPagamento PorId(int id) => Todos.Single(x => x.Id == id);

        public static StatusPagamento NOVO = new StatusPagamento(1, "Novo - Aguardando Geração de Lote");

        public static StatusPagamento LOTE_GERADO = new StatusPagamento(2, "Lote Gerado - Aguardando Envio para o SAP");

        public static StatusPagamento LOTE_CANCELADO = new StatusPagamento(3, "Lote Cancelado");

        public static StatusPagamento LOTE_ENVIADO = new StatusPagamento(4, "Lote Enviado - Aguardando Criação do Pedido SAP");

        public static StatusPagamento ERRO_NA_CRIACAO = new StatusPagamento(5, "Erro na Criação do Pedido SAP - Aguardando Criação de Lote");

        public static StatusPagamento PEDIDO_SAP_CRIADO = new StatusPagamento(6, "Pedido SAP Criado - Aguardando Recebimento Fiscal");

        public static StatusPagamento AGUARDANDO_ENVIO_PARA_CANCELAMENTO = new StatusPagamento(7, "Aguardando Envio para Cancelamento do Pedido SAP");

        public static StatusPagamento PEDIDO_SAP_ENVIADO = new StatusPagamento(8, "Pedido SAP Enviado - Aguardando Cancelamento");

        public static StatusPagamento ERRO_NO_CANCELAMENTO = new StatusPagamento(9, "Erro no Cancelamento do Pedido SAP");

        public static StatusPagamento PEDIDO_SAP_CANCELADO = new StatusPagamento(10, "Pedido SAP Cancelado - Aguardando Geração de Lote");

        public static StatusPagamento PEDIDO_SAP_PAGO = new StatusPagamento(11, "Pedido SAP Pago");

        public static StatusPagamento PEDIDO_SAP_PAGO_MANUALMENTE = new StatusPagamento(12, "Pedido SAP Pago Manualmente");

        public static StatusPagamento ESTORNO = new StatusPagamento(13, "Estorno");

        public static StatusPagamento SEM_LANCAMENTO_PARA_SAP = new StatusPagamento(14, "Sem Lançamento para o SAP");

        public static StatusPagamento PEDIDO_SAP_RECEBIDO_FISCAL = new StatusPagamento(15, "Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento");

        public static StatusPagamento SEM_LANCAMENTO_PARA_SAP_HISTORICO = new StatusPagamento(17, "Sem Lançamento para o SAP (Histórico)");

        public static StatusPagamento LANCAMENTO_DE_CONTROLE = new StatusPagamento(18, "Lançamento de Controle");

        public static StatusPagamento LOTE_AUTOMATICO_CANCELADO = new StatusPagamento(21, "Lote Automático Cancelado");

        public static StatusPagamento LANCAMENTO_AUTOMATICO_CANCELADO = new StatusPagamento(22, "Lançamento Automático Cancelado");

        public static StatusPagamento PEDIDO_SAP_RETIDO = new StatusPagamento(23, "Pedido SAP Retido - RJ");

        // TODO: Criar semelhante no front e tornar este privado.
        public static IReadOnlyCollection<StatusPagamento> Todos { get; } = new[] { NOVO, LOTE_GERADO, LOTE_CANCELADO, LOTE_ENVIADO, ERRO_NA_CRIACAO, PEDIDO_SAP_CRIADO, AGUARDANDO_ENVIO_PARA_CANCELAMENTO, PEDIDO_SAP_ENVIADO, ERRO_NO_CANCELAMENTO,
         PEDIDO_SAP_CANCELADO, PEDIDO_SAP_PAGO, PEDIDO_SAP_PAGO_MANUALMENTE, ESTORNO, SEM_LANCAMENTO_PARA_SAP, PEDIDO_SAP_RECEBIDO_FISCAL, SEM_LANCAMENTO_PARA_SAP_HISTORICO, LANCAMENTO_DE_CONTROLE, LOTE_AUTOMATICO_CANCELADO, LOTE_AUTOMATICO_CANCELADO, PEDIDO_SAP_RETIDO };

        #endregion enum

        #region converters

        public static implicit operator StatusPagamento(int value) => PorId(value);

        public static implicit operator int(StatusPagamento value) => value.Id;

        #endregion converters
    }
}