using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarPrepostoCommand : Validatable, IValidatable
    {
        public string Nome { get; set; }
        public string EstadoId { get; set; }
        public bool Ativo { get; set; } = true;
        public bool EhCivelEstrategico { get; set; } = false;
        public bool EhCivel { get; set; }
        public bool EhTrabalhista { get; set; }
        public bool EhJuizado { get; set; }
        public string? UsuarioId { get; set; } = null;
        public bool EhProcon { get; set; }
        public bool EhPex { get; set; }
        public bool EhEscritorio { get; set; }
        public string Matricula { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }

            if (Ativo && !EhEscritorio && !EhCivel && !EhCivelEstrategico && !EhJuizado && !EhTrabalhista && !EhProcon && !EhPex)
            {
                AddNotification("", "Para prepostos ativos, pelo menos uma área de atuação deve estar marcada");
            }
        }
    }
}