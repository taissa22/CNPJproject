using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
#nullable enable

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarEventoCommand : Validatable, IValidatable
    {
        public string Nome { get; set; }
        public bool PossuiDecisao { get; set; }
        public bool NotificarViaEmail { get; set; }
        public bool EhPrazo { get; set; }
        public bool Ativo { get; set; }
        public bool EhTrabalhistaAdm { get; set; }
        public bool EhCivel { get; set; }
        public bool EhCivelEstrategico { get; set; }
        public bool EhRegulatorio { get; set; }
        public bool EhTrabalhista { get; set; }
        public int? InstanciaId { get; set; }
        public bool PreencheMulta { get; set; }
        public bool? ReverCalculo { get; set; }
        public bool? FinalizacaoEscritorio { get; set; }
        public bool FinalizacaoContabil { get; set; }
        public bool AlterarExcluir { get; set; }
        public int? IdEstrategico { get; set; }
        public int? IdConsumidor { get; set; }
        public bool AtualizaEscritorio { get; set; }
        public bool EhTributarioAdm { get; set; }
        public bool EhTributarioJudicial { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }
        }
    }
}