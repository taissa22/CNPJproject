using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class ImportarCotacaoCommand : Validatable, IValidatable
    {
        public DateTime DataAnoMes { get; set; }
        public string Arquivo { get; set; }

        public override void Validate()
        {
            
        }
    }
}
