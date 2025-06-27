using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoPrazoCommand : Validatable, IValidatable
    {
        public string Descricao { get; set; }
        public double TipoProcesso { get; set; }
        public bool Ativo { get; set; }
        public bool? EhServico { get; set; }
        public bool? EhDocumento { get; set; }
        public int? IdConsumidor { get; set; }
        public int? IdEstrategico { get; set; }


        public override void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres exedido");
            }
        }
    }
}
