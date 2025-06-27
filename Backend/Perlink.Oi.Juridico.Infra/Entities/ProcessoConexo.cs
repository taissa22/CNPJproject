using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ProcessoConexo : Notifiable, IEntity, INotifiable
    {
        private ProcessoConexo()
        {
        }

        public int Id { get; private set; }

        internal int? EmpresaDoGrupoId { get; private set; }
        public EmpresaDoGrupo EmpresaDoGrupo { get; private set; }
        public int? OrgaoId { get; private set; }
        public int? ComarcaId { get; private set; }
    }
}