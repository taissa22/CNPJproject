using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarIndiceCorrecaoEsferaCommand : Validatable, IValidatable
    {
        public int EsferaId { get;  set; }
        public DateTime DataVigencia { get;  set; }
        public int IndiceId { get;  set; }


        public override void Validate()
        {

        }
    }
}