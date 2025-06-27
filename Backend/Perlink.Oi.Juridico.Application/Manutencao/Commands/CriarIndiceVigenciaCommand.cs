using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarIndiceVigenciaCommand : Validatable, IValidatable
    {


        public int Tipoprocessoid { get; set; }
        public string DataVigencia { get; set; }
        public int IndiceId { get; set; }

        public override void Validate()
        {
           
        }
    }
}
