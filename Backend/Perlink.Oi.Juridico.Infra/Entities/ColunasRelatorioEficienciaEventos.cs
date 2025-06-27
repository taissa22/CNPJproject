using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ColunasRelatorioEficienciaEvento : Notifiable, IEntity, INotifiable
    {
        private ColunasRelatorioEficienciaEvento()
        {
        }
       

        public ColunasRelatorioEficienciaEvento(int tipoProcesso, int eventoId, int relatorioId, int colunaId, int decisaoId, int sequencial)
        {
            TipoProcesso = tipoProcesso;
            RelatorioId = relatorioId;
            EventoId = eventoId;
            ColunaId = colunaId;
            DecisaoId = decisaoId;
            Sequencial = sequencial;

        }

        public int TipoProcesso { get; set; }
        public int RelatorioId { get; set; }
        public int EventoId { get; set; }
        public int ColunaId { get; set; }
        public int? DecisaoId { get; set; }
        public int Sequencial { get; set; }        
    }
}