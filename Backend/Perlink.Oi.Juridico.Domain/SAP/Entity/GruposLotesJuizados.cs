using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class GruposLotesJuizados : EntityCrud<GruposLotesJuizados, long>
    {
        public override AbstractValidator<GruposLotesJuizados> Validator => new GruposLotesJuizadosValidator();

        public string Descricao { get; set; }


        public override void PreencherDados(GruposLotesJuizados data)
        {
            Descricao = data.Descricao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class GruposLotesJuizadosValidator : AbstractValidator<GruposLotesJuizados>
        {
            public GruposLotesJuizadosValidator()
            {
            }
        }
    }
}
