using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarMigracaoAssuntoEstrategicoCommand : Validatable, IValidatable
    {
        public int CodAssuntoCivelEstrat { get; set; }
        public int CodAssuntoCivelCons { get; set; }

        public override void Validate()
        {
            if (CodAssuntoCivelEstrat > 0)
            {
                AddNotification(nameof(CodAssuntoCivelEstrat), "O campo CodAssuntoCivelEstrat deve ser informado.");
            }

            if (CodAssuntoCivelCons > 0)
            {
                AddNotification(nameof(CodAssuntoCivelCons), "O campo CodAssuntoCivelCons deve ser informado.");
            }
        }
    }
}