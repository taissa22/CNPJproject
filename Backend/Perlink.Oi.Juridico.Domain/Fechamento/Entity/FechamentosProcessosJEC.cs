using FluentValidation;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Fechamento.Entity {
    public class FechamentosProcessosJEC : EntityCrud<FechamentosProcessosJEC, long> {
        public override AbstractValidator<FechamentosProcessosJEC> Validator => throw new NotImplementedException();

        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public int NumeroMeses { get; set; }
        public bool IndicaFechamentoMes { get; set; }
        public DateTime DataGeracao { get; set; }
        public string CodigoUsuario { get; set; }
        public long CodigoEmpresaCentralizadoraAssociada { get; set; }
        public DateTime MesAnoFechamentoAssociado { get; set; }
        public DateTime DataFechamentoAssociado { get; set; }
        public decimal ValorCorte { get; set; }
        public string TipoDataMediaMovel { get; set; }


        public IList<BaseFechamentoJecCompleta> BaseFechamentoJecCompleta { get; set; }
        public IList<AgendarApuracaoOutliers> AgendarApuracaoOutliers { get; set; }

        public override void PreencherDados(FechamentosProcessosJEC data) {
            MesAnoFechamento = data.MesAnoFechamento;
            DataFechamento = data.DataFechamento;
            NumeroMeses = data.NumeroMeses;
            IndicaFechamentoMes = data.IndicaFechamentoMes;
            DataGeracao = data.DataGeracao;
            CodigoUsuario = data.CodigoUsuario;
            CodigoEmpresaCentralizadoraAssociada = data.CodigoEmpresaCentralizadoraAssociada;
            MesAnoFechamentoAssociado = data.MesAnoFechamentoAssociado;
            DataFechamentoAssociado = data.DataFechamentoAssociado;
            ValorCorte = data.ValorCorte;
            TipoDataMediaMovel = data.TipoDataMediaMovel;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class FechamentosProcessosJECValidator: AbstractValidator<FechamentosProcessosJEC> {
        public FechamentosProcessosJECValidator() { }
    }
}
