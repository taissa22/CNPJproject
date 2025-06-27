using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class BBStatusParcelas : EntityCrud<BBStatusParcelas, long>
    {
        public override AbstractValidator<BBStatusParcelas> Validator => new BBStatusParcelasValidator();
        public long CodigoBB { get; set; }
        public string Descricao { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }

        public override void PreencherDados(BBStatusParcelas data)
        {
            CodigoBB = data.CodigoBB;
            Descricao = data.Descricao;
            
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class BBStatusParcelasValidator : AbstractValidator<BBStatusParcelas>
    {
        public BBStatusParcelasValidator()
        {
           
        }
    }
}