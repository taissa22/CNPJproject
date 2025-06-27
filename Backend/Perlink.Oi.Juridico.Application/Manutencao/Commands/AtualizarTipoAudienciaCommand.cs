using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarTipoAudienciaCommand : Validatable, IValidatable
    {
        public int CodigoTipoAudiencia { get; set; } = 0;
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public bool Ativo { get; set; }
        public int? IdEstrategico { get; set; }
        public int? IdConsumidor { get; set; }
        public bool LinkVirtual { get; set; }

        public override void Validate()
        {
            if (CodigoTipoAudiencia == 0)
            {
                AddNotification(nameof(CodigoTipoAudiencia), "Campo Requerido");
            }           

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(100))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }

            if (string.IsNullOrEmpty(Sigla))
            {
                AddNotification(nameof(Sigla), "Campo Requerido");
            }
        }
    }
}
