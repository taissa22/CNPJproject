using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class LancamentoProcesso : EntityCrud<LancamentoProcesso, long>
    {
        public long CodigoLancamento { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public long CodigoCatPagamento { get; set; }
        public long? CodigoBanco { get; set; }
        public decimal ValorLancamento { get; set; }
        public DateTime DataLancamento { get; set; }
        public string ComentarioSap { get; set; }
        public long? NumeroPedidoSap { get; set; }
        public DateTime? DataPagamentoPedido { get; set; }
        public long? NumeroGuia { get; set; }
        public DateTime? DataEnvioEscritorio { get; set; }
        public string Comentario { get; set; }
        public DateTime? DataRecebimentoFiscal { get; set; }
        public string CodigoAutenticacaoEletronica { get; set; }
        public DateTime? DataEfetivacaoParcelaBancoDoBrasil { get; set; }
        public long CodigoStatusPagamento { get; set; }
        public long? IdBbStatusParcela { get; set; }
        public long? IdBBModalidade { get; set; }
        public long? NumeroContaJudicial { get; set; }
        public long? NumeroParcelaContaJudicial { get; set; }
        public long? CodigoParte { get; set; }
        public long QuantidadeLancamento { get; set; }
        public bool IndicadorExluido { get; set; }
        public string CodigoUsuarioRecebedor { get; set; }
        public long? CodigoFormaPagamento { get; set; }
        public long? CodigoCentroCusto { get; set; }
        public long? CodigoFornecedor { get; set; }
        public DateTime? DataCriacaoPedido { get; set; }
        public DateTime? DataGarantiaLevantada { get; set; }
        public int? CodigoTipoParticipacao { get; set; }
        public string CodigoCentroSAP { get; set; }
        public DateTime? DataGuiaJudicial { get; internal set; }

        public override AbstractValidator<LancamentoProcesso> Validator => new LancamentoProcessoValidator();

        public CategoriaPagamento CategoriaPagamento { get; set; }
        public TipoLancamento TipoLancamento { get; set; }
        public Processo Processo { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public BBStatusParcelas BancoDoBrasilStatusParcela { get; set; }
        public BBModalidade BBModalidade { get; set; }

        public IList<LoteLancamento> LoteLancamentos { get; set; }
        public ParteProcesso ParteProcesso { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public List<CompromissoProcessoParcela> CompromissoProcessoParcelas { get; set; }
        public Banco Banco { get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorCorrecao { get; set; }
        public decimal ValorAjusteJuros { get; set; }
        public decimal ValorAjusteCorrecao { get; set; }

        public override void PreencherDados(LancamentoProcesso data)
        {
            //Id = data.Id;
            //CodigoLancamento = data.CodigoLancamento;
            //CodigoTipoLancamento = data.CodigoTipoLancamento;
            //CodigoCatPagamento = data.CodigoCatPagamento;
            //QuantidadeLancamentos = data.QuantidadeLancamentos;
            //ValorLancamento = data.ValorLancamento;
            //DataLancamento = data.DataLancamento;
            //ComentarioSap = data.ComentarioSap;
            //NumeroPedidoSap = data.NumeroPedidoSap;
            //DataCriacaoPedido = data.DataCriacaoPedido;
            //DataPagamentoPedido = data.DataPagamentoPedido;
            //CodigoUsuarioRecebedor = data.CodigoUsuarioRecebedor;
            //NumeroGuia = data.NumeroGuia;
            //DataEnvioEscritorio = data.DataEnvioEscritorio;
            //IndExcluido = data.IndExcluido;
            //Comentario = data.Comentario;
            //DataRecebimentoFiscal = data.DataRecebimentoFiscal;
            //CodigoAutenticacaoEletronica = data.CodigoAutenticacaoEletronica;
            //DataEfetivacaoParcela = data.DataEfetivacaoParcela;
            //CodigoStatusPagamento = data.CodigoStatusPagamento;
            //NumeroContaJudicial = data.NumeroContaJudicial;
            //NumeroParcelaContaJudicial = data.NumeroParcelaContaJudicial;          
            //CodigoUsuarioRecebedor = data.CodigoUsuarioRecebedor;
          
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
}

internal class LancamentoProcessoValidator : AbstractValidator<LancamentoProcesso>
{
    public LancamentoProcessoValidator()
    {
    }
}