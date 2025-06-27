using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class Lote : EntityCrud<Lote, long>
    {
       

        public override AbstractValidator<Lote> Validator => new LoteValidator();
        public long CodigoTipoProcesso { get; set; }
        public long CodigoParte { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public string CodigoCentroSAP { get; set; }
        public long CodigoStatusPagamento { get; set; }
        public string CodigoUsuario { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataRecebimentoFisico { get; set; }
        public DateTime? DataGeracaoArquivoBB { get; set; }
        public DateTime? DataRetornoBB { get; set; }
        public DateTime? DataErro { get; set; }
        public DateTime? DataCriacaoPedido { get; set; }
        public DateTime? DataCancelamentoLote { get; set; }
        public DateTime? DataPagamentoPedido { get; set; }
        public long? NumeroLoteBB { get; set; }
        public long? NumeroPedidoSAP { get; set; }
        public long UltimaSeqBordero { get; set; }
        public double Valor { get; set; }
        public string DescricaoLote { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public Parte Parte { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public Usuario Usuario { get; set; }
        public long CodigoCentroCusto { get; set; }
        public long CodigoFornecedor { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public TipoProcesso TipoProcesso { get; set; }
        public IList<Bordero> Borderos { get; set; }
        public IList<LoteLancamento> LotesLancamento { get; set; }
        public IList<BBResumoProcessamento> ResumosProcessamentos { get; set; }

        public override void PreencherDados(Lote data)
        {
            CodigoTipoProcesso = data.CodigoTipoProcesso;
            CodigoParte = data.CodigoParte;
            CodigoCentroCusto = data.CodigoCentroCusto;
            CodigoFornecedor = data.CodigoFornecedor;
            CodigoFormaPagamento = data.CodigoFormaPagamento;
            CodigoCentroSAP = data.CodigoCentroSAP;
            CodigoStatusPagamento = data.CodigoStatusPagamento;
            CodigoUsuario = data.CodigoUsuario;
            DataCriacao = data.DataCriacao;
            DataRecebimentoFisico = data.DataRecebimentoFisico;
            DataGeracaoArquivoBB = data.DataGeracaoArquivoBB;
            DataRetornoBB = data.DataRetornoBB;
            DataErro = data.DataErro;
            DataCriacaoPedido = data.DataCriacaoPedido;
            DataCancelamentoLote = data.DataCancelamentoLote;
            DataPagamentoPedido = data.DataPagamentoPedido;
            NumeroLoteBB = data.NumeroLoteBB;
            NumeroPedidoSAP = data.NumeroPedidoSAP;
            UltimaSeqBordero = data.UltimaSeqBordero;
            Valor = data.Valor;

            DescricaoLote = data.DescricaoLote;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class LoteValidator : AbstractValidator<Lote>
    {
        public LoteValidator()
        {
            RuleFor(x => x.DataCriacao)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoTipoProcesso)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoParte)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoFornecedor)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoCentroCusto)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoFormaPagamento)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoCentroSAP)
                .MaximumLength(4).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.NumeroPedidoSAP);

            RuleFor(x => x.Valor)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.CodigoUsuario)
                .MaximumLength(30).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);


            RuleFor(x => x.CodigoStatusPagamento)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
        }
    }
}