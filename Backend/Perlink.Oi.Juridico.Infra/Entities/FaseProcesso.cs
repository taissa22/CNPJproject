using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FaseProcesso : Notifiable, IEntity, INotifiable
    {
        private FaseProcesso()
        {
        }

        public FaseProcesso(int seqFase, int eventoId, int decisaoId, int processoId)
        {
            SeqFase = seqFase;
            EventoId = eventoId;
            ProcessoId = processoId;
            DecisaoId = decisaoId;

        }

        public int SeqFase { get; set; }
        public int? EventoId { get; set; }
        public int ProcessoId { get; set; }
        public int? DecisaoId { get; set; }
    }
}