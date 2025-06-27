using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EmpresaCentralizadoraAgendamentoFechAuto : Notifiable, IEntity, INotifiable
    {
        private EmpresaCentralizadoraAgendamentoFechAuto()
        {
        }

        public long Id { get; private set; }
        public long CodSolicitacaoFechamento { get; set; }
    }
}