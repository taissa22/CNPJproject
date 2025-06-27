using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Entities {
    public sealed class Lancamento : Notifiable, IEntity, INotifiable {
        private Lancamento() {
        }

        public int Sequencial { get; private set; }

        internal int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        public int StatusPagamentoId { get; private set; }

        public int? FormaPagamentoId { get; private set; }

        public int? FornecedorId { get; set; }

        public decimal Valor { get; private set; }

        public DateTime? DataPagamento { get; private set; }

        public int? ParteId { get; private set; }
        public Parte Parte { get; private set; }

        public string ComentarioSap { get; private set; }       

        public StatusPagamento StatusPagamento { get; private set; }

        internal int? FielDepositarioId { get; private set; }
        public FielDepositario FielDepositario { get; private set; }

        internal Despesa Despesa { get; private set; }

        public long NumeroPedidoSAP { get; private set; }

        public string Comentario { get; private set; }

        private static readonly List<int> STATUS_COM_COMENTARIO_SAP =
            new List<int>() {
                StatusPagamento.NOVO,
                StatusPagamento.ERRO_NA_CRIACAO,
                StatusPagamento.PEDIDO_SAP_CANCELADO
            };

        internal void AtualizaComentarioSAP(string comentarioSap)
        {            
            if (STATUS_COM_COMENTARIO_SAP.Contains(StatusPagamento))
            {
                ComentarioSap = comentarioSap;
            }
        }

        public void AtualizaNumeroPedidoSAP(long numeroPedidoSAPNovo)
        {
            NumeroPedidoSAP = numeroPedidoSAPNovo;
        }

        public void AtualizaComentarioMigracaoPedidosSAP(string comentarioMigracao)
        {
            string novoComentarioMigracao = $"{Comentario} {comentarioMigracao}";

            if (novoComentarioMigracao.Length > 4000) {
                novoComentarioMigracao = novoComentarioMigracao.Substring(0, 4000);
            }

            Comentario = novoComentarioMigracao;

        }
    }
}
