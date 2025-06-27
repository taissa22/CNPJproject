using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CompromissoProcessoCredor : EntityCrud<CompromissoProcessoCredor, long>
    {
        public long CodigoProcesso { get; set; }
        public long CodigoCredorCompromisso { get; set; }
        public override AbstractValidator<CompromissoProcessoCredor> Validator => new CompromissoProcessoCredorValidator();
        public CompromissoProcesso CompromissoProcesso { get; set; }
        //public CredorCompromisso CredorCompromisso { get; set; } //ToDo - Falta mapear

        public override void PreencherDados(CompromissoProcessoCredor data)
        {
            
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class CompromissoProcessoCredorValidator : AbstractValidator<CompromissoProcessoCredor>
        {
            public CompromissoProcessoCredorValidator()
            {
            }
        }
    }
}