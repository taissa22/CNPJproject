using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarTipoPrazoMigracaoEstrategicoCommand : Validatable, IValidatable
    {
        public int CodTipoPrazoCivelEstrat { get; set; }
        public int CodTipoPrazoCivelCons { get; set; }

        public override void Validate()
        {
            if (CodTipoPrazoCivelEstrat > 0)
            {
                AddNotification(nameof(CodTipoPrazoCivelEstrat), "O campo CodTipoDocCivelConsumidor deve ser informado.");
            }

            if (CodTipoPrazoCivelCons > 0)
            {
                AddNotification(nameof(CodTipoPrazoCivelCons), "O campo CodTipoDocCivelEstrategico deve ser informado.");
            }
        }
    }
}
