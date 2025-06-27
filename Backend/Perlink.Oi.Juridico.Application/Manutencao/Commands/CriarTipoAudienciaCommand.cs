using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoAudienciaCommand : Validatable, IValidatable
    {
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public int TipoProcesso { get; set; }
        public bool Ativo { get; set; }
        public int? IdConsumidor { get; set; }
        public int? IdEstrategico { get; set; }
        public bool LinkVirtual { get; set; }


        public override void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(100))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres exedido");
            }

            if (string.IsNullOrEmpty(Sigla))
            {
                AddNotification(nameof(Sigla), "O campo deve ser preenchido");
            }
        }
    }
}
