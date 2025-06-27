using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Lote : Notifiable, IEntity, INotifiable
    {
        private Lote()
        {
        }

        public int Id { get; private set; }

        internal int EmpresaDoGrupoId { get; private set; }
        public EmpresaDoGrupo EmpresaDoGrupo { get; private set; }

        private readonly HashSet<Bordero> borderosSet = new HashSet<Bordero>();
        public IReadOnlyCollection<Bordero> Borderos => borderosSet;

        public long NumeroPedidoSAP { get; private set; }

        public void AtualizaNumeroPedidoSAP(long numeroPedidoSAPNovo)
        {
            NumeroPedidoSAP = numeroPedidoSAPNovo;
        }
               
    }
}