using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class AndamentoDoProcesso : Notifiable, IEntity, INotifiable
    {
        private AndamentoDoProcesso()
        {
        }

        public static AndamentoDoProcesso Criar(Processo processo, Acao acao,
            DateTime dataDistribuicao, Evento evento) {
            var andamentoDoProcesso = new AndamentoDoProcesso() {
                ProcessoId = processo.Id,
                Processo = processo,
                AcaoId = acao.Id,
                Acao = acao,
                DataEvento = dataDistribuicao,
                EventoId = evento.Id,
                Evento = evento
            };
            //AndamentoDoProcesso.validate();
            return andamentoDoProcesso;
        }

        internal int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        public int Sequencial { get; private set; }

        internal int EventoId { get; private set; }
        public Evento Evento { get; private set; }

        public DateTime DataEvento { get; private set; }

        internal int? AcaoId { get; private set; }
        public Acao Acao { get; private set; }

        public int? DecisaoId { get; set; }
    }
}