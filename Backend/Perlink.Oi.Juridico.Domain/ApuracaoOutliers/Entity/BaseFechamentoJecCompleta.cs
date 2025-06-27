using FluentValidation;
using Perlink.Oi.Juridico.Domain.Fechamento.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity {
    public class BaseFechamentoJecCompleta : EntityCrud<BaseFechamentoJecCompleta, long> {
        public override AbstractValidator<BaseFechamentoJecCompleta> Validator => throw new NotImplementedException();

        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public long CodigoProcesso { get; set; }
        public string NumeroProcesso { get; set; }
        public string CodigoEstado { get; set; }
        public long CodigoComarca { get; set; }
        public long CodigoVara { get; set; }
        public long CodigoTipoVara { get; set; }
        public long CodigoEmpresaDoGrupo { get; set; }
        public DateTime DataCadastroProcesso { get; set; }
        public FechamentosProcessosJEC FechamentosProcessosJEC { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }
        public string PrePos { get; set; }
        public string NomeComarca { get; set; }
        public string NomeTipoVara { get; set; }
        public string NomeEmpresaGrupo { get; set; }
        public DateTime DataFinalizacaoContabil { get; set; }
        public bool ProcInfluenciaContingencia { get; set; }
        public long CodigoLancamento { get; set; }
        public decimal? ValorLancamento { get; set; }
        public DateTime? DataRecebimentoFiscal { get; set; }
        public DateTime? DataPagamento { get; set; }
        public long? CodigoCategoriaPagamento { get; set; }
        public string DescricaoCategoriaPagamento { get; set; }
        public bool CatPagInfluenciaContingencia { get; set; }
        public string ParametroMediaMovel { get; set; }

        public override void PreencherDados(BaseFechamentoJecCompleta data) {
            MesAnoFechamento = data.MesAnoFechamento;
            DataFechamento = data.DataFechamento;
            CodigoProcesso = data.CodigoProcesso;
            NumeroProcesso = data.NumeroProcesso;
            CodigoEstado = data.CodigoEstado;
            CodigoComarca = data.CodigoComarca;
            CodigoVara = data.CodigoVara;
            CodigoTipoVara = data.CodigoTipoVara;
            CodigoEmpresaDoGrupo = data.CodigoEmpresaDoGrupo;
            DataCadastroProcesso = data.DataCadastroProcesso;
            PrePos = data.PrePos;
            DataFinalizacaoContabil = data.DataFinalizacaoContabil;
            ProcInfluenciaContingencia = data.ProcInfluenciaContingencia;
            CodigoLancamento = data.CodigoLancamento;
            ValorLancamento = data.ValorLancamento;
            DataRecebimentoFiscal = data.DataRecebimentoFiscal;
            DataPagamento = data.DataPagamento;
            CodigoCategoriaPagamento = data.CodigoCategoriaPagamento;
            DescricaoCategoriaPagamento = data.DescricaoCategoriaPagamento;
            CatPagInfluenciaContingencia = data.CatPagInfluenciaContingencia;
            ParametroMediaMovel = data.ParametroMediaMovel;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class BaseFechamentoJecCompletaValidator : AbstractValidator<BaseFechamentoJecCompleta> {
        public BaseFechamentoJecCompletaValidator() {

        }
    }
}
