using System;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Commands
{
    public class CriarAudienciaCommand
    {
        public int ProcessoId { get; set; }

        public DateTime DataAudiencia { get; set; }

        public DateTime HoraAudiencia { get; set; }

        public string Comentario { get; set; }

        public int TipoAudienciaId { get; set; }

        public int? EscritorioId { get; set; }

        public int? AdvogadoId { get; set; }
    }
}
