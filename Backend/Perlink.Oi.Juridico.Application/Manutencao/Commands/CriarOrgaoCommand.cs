using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using TelefoneVO = Perlink.Oi.Juridico.Infra.ValueObjects.Telefone;
using TipoOrgaoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoOrgao;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarOrgaoCommand : Validatable, IValidatable
    {   
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; }
        public string TipoOrgao { get; set; } = string.Empty;
        public IEnumerable<string> Competencias { get; set; } = Array.Empty<string>();

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Nome) && !Nome.HasMaxLength(400))
            {
                AddNotification(nameof(Nome), "Limite de caracteres excedido");
            }

            if (!string.IsNullOrEmpty(Telefone) && !TelefoneVO.IsValid(Telefone))
            {
                AddNotification(nameof(Telefone), "Telefone inválido");
            }

            if (!TipoOrgaoEnum.IsValid(TipoOrgao))
            {
                AddNotification(nameof(TipoOrgao), "Tipo Orgão inválido");
            }
        }
    }
}
