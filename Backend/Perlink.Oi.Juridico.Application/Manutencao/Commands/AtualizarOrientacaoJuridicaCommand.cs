using Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System;
using System.Collections.Generic;
using TelefoneVO = Perlink.Oi.Juridico.Infra.ValueObjects.Telefone;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarOrientacaoJuridicaCommand : Validatable, IValidatable
    {
        public int CodOrientacaoJuridica { get; set; }
        public int CodTipoOrientacaoJuridica { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string PalavraChave { get; set; }
        public bool EhTrabalhista { get; set; }
        public bool  Ativo { get; set; }

        public override void Validate()
        {
            if (CodOrientacaoJuridica == 0)
            {
                AddNotification(nameof(CodOrientacaoJuridica), "Campo Requerido");
            }
        }
    }
}
