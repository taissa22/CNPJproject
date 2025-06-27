using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
  public class CriarMigracaoAcaoCommand : Validatable, IValidatable
    {
        public int CodAcaoConsumidor { get; set; }
        public int CodAcaoEstrategico { get; set; }     

        public override void Validate()
        {
            if (CodAcaoEstrategico > 0)
            {
                AddNotification(nameof(CodAcaoEstrategico), "O campo CodAcaoEstrategico deve ser informado.");
            }

            if (CodAcaoConsumidor > 0)
            {
                AddNotification(nameof(CodAcaoEstrategico), "O campo CodAcaoConsumidor deve ser informado.");
            }
        }
    }
}
