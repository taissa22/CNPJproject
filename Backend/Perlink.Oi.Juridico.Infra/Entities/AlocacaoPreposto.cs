using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class AlocacaoPreposto : Notifiable, IEntity, INotifiable
    {
        private AlocacaoPreposto()
        {
        }

        internal int EmpresaDoGrupoId { get; private set; }
        public EmpresaDoGrupo EmpresaDoGrupo { get; private set; }

        public int ComarcaId { get; private set; }

        public int VaraId { get; private set; }

        public int TipoVaraId { get; private set; }

        public int PrepostoId { get; private set; }

        public DateTime DataAlocacao { get; private set; }
    }
}