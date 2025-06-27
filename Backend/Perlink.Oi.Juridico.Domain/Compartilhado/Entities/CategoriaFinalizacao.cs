using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CategoriaFinalizacao : EntityCrud<CategoriaFinalizacao, long>
    {
        public override AbstractValidator<CategoriaFinalizacao> Validator => new CategoriaFinalizacaoValidator();

        public long CodigoFinalizacao { get; set; }

        public CategoriaPagamento CategoriaPagamento { get; set; }
        public override void PreencherDados(CategoriaFinalizacao data)
        {
            CodigoFinalizacao = data.CodigoFinalizacao;
            Id = data.Id;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class CategoriaFinalizacaoValidator : AbstractValidator<CategoriaFinalizacao>
    {
        public CategoriaFinalizacaoValidator()
        {

        }
    }
}