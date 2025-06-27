using FluentValidation;
using Perlink.Oi.Juridico.Domain.Logs.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Processos.Entity
{
    public class TipoContratoEscritorio : EntityCrud<TipoContratoEscritorio, long>
    {
        public long CodTpContraEscritorio { get; set; }
        public string DscTipoContrato { get; set; }
        public string IndAtivo { get; set; }

        public override AbstractValidator<TipoContratoEscritorio> Validator => new TipoContratoEscritorioValidator();

        public override void PreencherDados(TipoContratoEscritorio data)
        {
            CodTpContraEscritorio = data.CodTpContraEscritorio;
            DscTipoContrato = data.DscTipoContrato;
            IndAtivo = data.IndAtivo;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class TipoContratoEscritorioValidator : AbstractValidator<TipoContratoEscritorio>
    {
        public TipoContratoEscritorioValidator()
        {

        }
    }
}
