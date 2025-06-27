using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Commands {

    public class AtualizarAudienciaCommand : Validatable, IValidatable {
        public int Id { get => Sequencial; set => Sequencial = value; }

        public int Sequencial { get; set; }

        public int ProcessoId { get; set; }

        public DateTime DataAudiencia { get; set; }

        public DateTime HoraAudiencia { get; set; }

        public string Comentario { get; set; }

        public int TipoAudienciaId { get; set; }

        public int? EscritorioId { get; set; }

        public int? AdvogadoId { get; set; }

        public int? PrepostoId { get; set; }

        public override void Validate() {
            if (ProcessoId <= 0) {
                AddNotification(nameof(ProcessoId), "O campo ID do Processo é obrigatório.");
            }

            if (Sequencial <= 0) {
                AddNotification(nameof(Sequencial), "O campo Sequencial é obrigatório.");
            }
        }
    }
}