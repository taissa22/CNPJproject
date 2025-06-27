using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AlterarMigracaoAcaoConsumidor : Validatable, IValidatable
    {
        public int CodAcaoCivel { get; set; }
        public int CodAcaoCivelEstrategico { get; set; }
        public override void Validate()
        {
            {
                AddNotification(nameof(CodAcaoCivelEstrategico), "O campo CodAcaoEstrategico deve ser informado.");
            }

            if (CodAcaoCivel > 0)
            {
                AddNotification(nameof(CodAcaoCivel), "O campo CodAcaoConsumidor deve ser informado.");
            }
        }
    }
}
