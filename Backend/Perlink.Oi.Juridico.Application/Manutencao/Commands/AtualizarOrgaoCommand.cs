using Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using TelefoneVO = Perlink.Oi.Juridico.Infra.ValueObjects.Telefone;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarOrgaoCommand : Validatable, IValidatable
    {
        public int Id { get; set; } = 0;
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; }
        public IEnumerable<CompetenciaDTO> Competencias { get; set; } = Array.Empty<CompetenciaDTO>();

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

            if (!string.IsNullOrEmpty(Nome) && !Nome.HasMaxLength(400))
            {
                AddNotification(nameof(Nome), "Limite de caracteres exedido");
            }

            if (!string.IsNullOrEmpty(Telefone) && !TelefoneVO.IsValid(Telefone))
            {
                AddNotification(nameof(Telefone), "Telefone inválido");
            }

            foreach (var competencia in Competencias)
            {
                competencia.Validate();
                AddNotifications(competencia);
            }
        }
    }
}
