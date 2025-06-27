using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoPendenciaCommand : Validatable, IValidatable
    {

        public string Descricao { get; set; } = string.Empty;

        public override void Validate()
        {           
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && Descricao.Length > 50)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            }
        }
    }
}
