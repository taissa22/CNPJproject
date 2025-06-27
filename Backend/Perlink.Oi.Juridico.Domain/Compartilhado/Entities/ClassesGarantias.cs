using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class ClassesGarantias : EntityCrud<ClassesGarantias, long>
    {
        

        public override AbstractValidator<ClassesGarantias> Validator => new ClassesGarantiasValidator();
        public long CodigoTipoSaldoGarantia { get; set; }
        public long ValorMultiplicador { get; set; }
        public string Descricao { get; set; }
        public long CodigoTipoLancamento { get; set; }

        public IList<CategoriaPagamento> CategoriasPagamentos { get; set; }
        public override void PreencherDados(ClassesGarantias data)
        {
            CodigoTipoSaldoGarantia = data.CodigoTipoSaldoGarantia;
            ValorMultiplicador = data.ValorMultiplicador;
            Descricao = data.Descricao;
            CodigoTipoLancamento = data.CodigoTipoLancamento;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class ClassesGarantiasValidator : AbstractValidator<ClassesGarantias>
    {
        public ClassesGarantiasValidator()
        {

        }
    }
}