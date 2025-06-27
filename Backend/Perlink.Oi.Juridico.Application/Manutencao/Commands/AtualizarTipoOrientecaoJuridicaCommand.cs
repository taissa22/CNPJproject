using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarTipoOrientecaoJuridicaCommand : Validatable, IValidatable
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }




        public override void Validate()
        {

            if (Codigo <= 0)
            {
                AddNotification(nameof(Codigo), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

        }

    }
}
