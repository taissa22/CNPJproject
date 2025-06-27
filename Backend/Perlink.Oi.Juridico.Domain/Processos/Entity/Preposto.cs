using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.Processos
{
    public class Preposto : EntityCrud<Preposto, long>
    {
        public string NomePreposto { get; set; }
        //public string CodigoEstado { get; set; }
        public bool IndicaPrepostoAtivo { get; set; }

        public bool IndicaPrepostoTrabalhista { get; set; }


        public IList<AudienciaProcesso> AudienciaProcessos { get; set; }

        public override FluentValidation.AbstractValidator<Preposto> Validator => new PrepostoValidator();

        public override void PreencherDados(Preposto data)
        {
            NomePreposto = data.NomePreposto;
            //CodigoEstado = data.CodigoEstado;
            IndicaPrepostoAtivo = data.IndicaPrepostoAtivo;
            IndicaPrepostoTrabalhista = data.IndicaPrepostoTrabalhista;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class PrepostoValidator : AbstractValidator<Preposto>
    {
        public PrepostoValidator()
        {
        }
    }
}

