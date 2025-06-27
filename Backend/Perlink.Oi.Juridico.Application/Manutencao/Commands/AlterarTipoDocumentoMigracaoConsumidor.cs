using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
   public class AlterarTipoDocumentoMigracaoConsumidor : Validatable, IValidatable
    {
        public int CodTipoDocCivelConsumidor { get; set; }
        public int CodTipoDocCivelEstrategico { get; set; }

        public override void Validate()
        {
            if (CodTipoDocCivelConsumidor > 0)
            {
                AddNotification(nameof(CodTipoDocCivelConsumidor), "O campo CodTipoDocCivelConsumidor deve ser informado.");
            }

            if (CodTipoDocCivelEstrategico > 0)
            {
                AddNotification(nameof(CodTipoDocCivelEstrategico), "O campo CodTipoDocCivelEstrategico deve ser informado.");
            }
        }
    }
}

