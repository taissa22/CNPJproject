using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoOrientecaoJuridicaCommand : Validatable, IValidatable
    {

        public string Descricao { get; set; }

        public override void Validate()
        {

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }
        }

    }
}
