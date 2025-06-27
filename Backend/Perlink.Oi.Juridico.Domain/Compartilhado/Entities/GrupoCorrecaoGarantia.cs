using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class GrupoCorrecaoGarantia : EntityCrud<GrupoCorrecaoGarantia, long>
    {
    

        public override AbstractValidator<GrupoCorrecaoGarantia> Validator => new GrupoCorrecaoGarantiaValidator();


        public long CodigoTipoProcesso { get; set; }
        public string Descricao { get; set; }
        public TipoProcesso TipoProcesso { get; set; }
        public IList<CategoriaPagamento> categoriaPagamento { get; set; }
        public override void PreencherDados(GrupoCorrecaoGarantia data)
        {
            CodigoTipoProcesso = data.CodigoTipoProcesso;
            Descricao = data.Descricao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class GrupoCorrecaoGarantiaValidator : AbstractValidator<GrupoCorrecaoGarantia>
    {
        public GrupoCorrecaoGarantiaValidator()
        {

        }
    }
}