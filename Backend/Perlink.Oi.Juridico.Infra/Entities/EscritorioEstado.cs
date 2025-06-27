using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EscritorioEstado : Notifiable, IEntity, INotifiable
    {
        private EscritorioEstado()
        {
        }

        public static EscritorioEstado Criar(int profissionalId,string estadoId, int tipoProcessoId )
        {
            EscritorioEstado escritorio = new EscritorioEstado();
            escritorio.ProfissionalId = profissionalId;
            escritorio.EstadoId = estadoId;
            escritorio.TipoProcessoId = tipoProcessoId;
            return escritorio;
        }

        internal int ProfissionalId { get; private set; }

        public Profissional Profissional { get; private set; }

        internal string EstadoId { get; private set; }

        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);

     
        internal int TipoProcessoId { get; private set; }

        public TipoProcesso TipoProcesso => TipoProcesso.PorId(TipoProcessoId);
    }
}