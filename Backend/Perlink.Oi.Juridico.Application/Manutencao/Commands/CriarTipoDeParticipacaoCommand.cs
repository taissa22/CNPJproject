using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoDeParticipacaoCommand : Validatable, IValidatable
    {
        public string Descricao { get; set; } = string.Empty;

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(20))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}
