using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarAcaoDoCivelConsumidorCommand: Validatable, IValidatable
    {
  
        public int? IdEstrategico { get; set; }
        public string Descricao { get; set; }

        public int? NaturezaAcaoBBId { get; set; } = null;
        public bool EnviarAppPreposto { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(30))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres exedido");
            }
        }
    }
}
