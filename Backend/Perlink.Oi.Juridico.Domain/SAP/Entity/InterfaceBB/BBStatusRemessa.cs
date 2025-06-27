using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class BBStatusRemessa : EntityCrud<BBStatusRemessa, long>
    {
        public override AbstractValidator<BBStatusRemessa> Validator => new BBStatusRemessaValidator();

        public long Codigo { get; set; }
        public string Descricao { get; set; }
        public IList<BBResumoProcessamento> ResumosProcessamentos { get; set; }
        public override void PreencherDados(BBStatusRemessa data)
        {
            Descricao = data.Descricao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        //TODO: Implementar validaçã
    }

    internal class BBStatusRemessaValidator : AbstractValidator<BBStatusRemessa>
    {
        public BBStatusRemessaValidator()
        {
        }
    }
}
    