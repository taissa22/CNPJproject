using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarAssuntoMigracaoConsumidorCommand : Validatable, IValidatable
    {
        public int? CodAssuntoCivel { get; set; }
        public int? CodAssuntoCivelEstrategico { get; set; }

        public override void Validate()
        {
            {
                if (CodAssuntoCivel > 0)
                {
                    AddNotification(nameof(CodAssuntoCivel), "O campo CodAssuntoCivel deve ser informado.");
                }

                if (CodAssuntoCivelEstrategico > 0)
                {
                    AddNotification(nameof(CodAssuntoCivelEstrategico), "O campo CodAssuntoCivelEstrategico deve ser informado.");
                }
            }
        }
    }

}

