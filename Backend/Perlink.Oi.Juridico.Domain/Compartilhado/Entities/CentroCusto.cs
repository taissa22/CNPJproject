using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CentroCusto : EntityCrud<CentroCusto, long> {
        public override AbstractValidator<CentroCusto> Validator => new CentroCustoValidator();

        public string Descricao { get; set; }
        public string CodigoCentroCustoSAP { get; set; }
        public bool IndicaAtivo { get; set; }
        public IList<Lote> Lotes { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }

        public override void PreencherDados(CentroCusto data) {
            Descricao = data.Descricao;
            CodigoCentroCustoSAP = data.CodigoCentroCustoSAP;
            IndicaAtivo = data.IndicaAtivo;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class CentroCustoValidator : AbstractValidator<CentroCusto> {
        public CentroCustoValidator() {

        }
    }
}
