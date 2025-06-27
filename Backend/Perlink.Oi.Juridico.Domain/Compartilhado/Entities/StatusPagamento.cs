using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class StatusPagamento : EntityCrud<StatusPagamento, long>
    {
        public override AbstractValidator<StatusPagamento> Validator => new StatusPagamentoValidator();

        public string Descricao { get; set; }
        public IList<Lote> Lotes { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }
        public override void PreencherDados(StatusPagamento data)
        {
            Descricao = data.Descricao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        //TODO: Implementar validaçã
    }

    internal class StatusPagamentoValidator : AbstractValidator<StatusPagamento>
    {
        public StatusPagamentoValidator()
        {
          

           
        }
    }
}
    