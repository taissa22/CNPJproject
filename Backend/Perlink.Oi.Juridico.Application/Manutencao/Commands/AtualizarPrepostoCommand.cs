using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarPrepostoCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EstadoId { get; set; }
        public bool Ativo { get; set; } = true;
        public bool EhCivelEstrategico { get; set; } = false;
        public bool EhCivel { get; set; }
        public bool EhTrabalhista { get; set; }
        public bool EhJuizado { get; set; }
        public string? UsuarioId { get; set; }
        public bool EhProcon { get; set; }
        public bool EhPex { get; set; }
        public bool EhEscritorio { get; set; }
        public string Matricula { get; set; }

        public override void Validate()
        {
            if (Id == 0)
            {
                AddNotification(nameof(Id), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }

            if (Ativo && !EhEscritorio && !EhCivel && !EhCivelEstrategico && !EhJuizado && !EhTrabalhista && !EhProcon && !EhPex)
            {
                AddNotification("", "Para prepostos ativos, pelo menos uma área de atuação deve estar marcada");
            }
        }
    }
}