using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using TelefoneVO = Perlink.Oi.Juridico.Infra.ValueObjects.Telefone;
using TipoOrgaoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoOrgao;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarOrientacaoJuridicaCommand : Validatable, IValidatable
    {       
        public int CodTipoOrientacaoJuridica { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string PalavraChave { get; set; }
        public bool EhTrabalhista { get; set; }
        public bool Ativo { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O campo deve ser preenchido");
            }
        }
    }
}
