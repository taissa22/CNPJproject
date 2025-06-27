
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao
{
    public class AtualizarAssuntoMigracaoEstrategicoCommand : Validatable, IValidatable
    {
        public int? CodAssuntoCivelEstrat { get; set; }
        public int? CodAssuntoCivelCons { get; set; }

        public override void Validate()
        {
            //throw new NotImplementedException();
        }
    }
}
