using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Pedido : EntityCrud<Pedido, long>
    {
        public override AbstractValidator<Pedido> Validator => new PedidoValidator();

        public string Descricao { get; set; }
        public bool? IndicaPedidoTrabalhista { get; set; }
        public bool? IndicaPedidoRegulatorio { get; set; }
        public bool? IndicaPedidoCivel { get; set; }
        public bool? IndicaPedidoTributarioAdm { get; set; }
        public bool? IndicaPedidoTributarioJud { get; set; }
        public bool? IndicaPedidoTrabalhistaAdm { get; set; }
        public bool? IndicaPedidoJuizado { get; set; }
        public bool IndicaEscritorioObrigatorio { get; set; }
        public bool IndicaProvavelZero { get; set; }
        public string IndicaProprioTerceiro { get; set; } //P ou T
        public bool IndicaPedidoAtivo { get; set; }
        public bool? IndicaInfluenciaContingencia { get; set; }
        public bool IndicaCivelEstrategico { get; set; }
        public bool IndicaCriminalJudicial { get; set; }
        public bool IndicaCriminalAdm { get; set; }
        public bool IndicaCivelAdm { get; set; }
        public bool IndicaProcon { get; set; }
        public bool IndicaPex { get; set; }
        public bool IndicaAtivoTributarioAdm { get; set; }
        public bool IndicaAtivoTributarioJud { get; set; }
        public bool IndicaRequerAtualizacaoDebito { get; set; }
        //public long? CodigoClassificacaoPedido { get; set; }
        //public long? CodigoGrupoPedido { get; set; }
        public decimal PercentualRaterioRisco { get; set; }
        public decimal PercentualMelhorRealizavel { get; set; }
        public decimal ValorReceitaMedia { get; set; }
        public DateTime? DataBaseReceitaMedia { get; set; }
        public string CodigoRiscoPerda { get; set; }

        public IList<PedidoProcesso> PedidosProcessos { get; set; }
        public IList<ContratoPedidoProcesso> ContratoPedidoProcessos { get; set; }

        //public GrupoPedidos GrupoPedido { get; set; }
        //public ClassificacoesPedidosPainel ClassificacoesPedidosPainel { get; set; }


        public override void PreencherDados(Pedido data)
        {
            Descricao = data.Descricao;
            IndicaPedidoTrabalhista = data.IndicaPedidoTrabalhista;
            IndicaPedidoRegulatorio = data.IndicaPedidoRegulatorio;
            IndicaPedidoCivel = data.IndicaPedidoCivel;
            IndicaPedidoTributarioAdm = data.IndicaPedidoTributarioAdm;
            IndicaPedidoTributarioJud = data.IndicaPedidoTributarioJud;
            IndicaPedidoTrabalhistaAdm = data.IndicaPedidoTrabalhistaAdm;
            IndicaPedidoJuizado = data.IndicaPedidoJuizado;
            IndicaEscritorioObrigatorio = data.IndicaEscritorioObrigatorio;
            IndicaProvavelZero = data.IndicaProvavelZero;
            IndicaProprioTerceiro = data.IndicaProprioTerceiro;
            IndicaPedidoAtivo = data.IndicaPedidoAtivo;
            IndicaInfluenciaContingencia = data.IndicaInfluenciaContingencia;
            IndicaCivelEstrategico = data.IndicaCivelEstrategico;
            IndicaCriminalJudicial = data.IndicaCriminalJudicial;
            IndicaCriminalAdm = data.IndicaCriminalAdm;
            IndicaCivelAdm = data.IndicaCivelAdm;
            IndicaProcon = data.IndicaProcon;
            IndicaPex = data.IndicaPex;
            IndicaAtivoTributarioAdm = data.IndicaAtivoTributarioAdm;
            IndicaAtivoTributarioJud = data.IndicaAtivoTributarioJud;
            IndicaRequerAtualizacaoDebito = data.IndicaRequerAtualizacaoDebito;
            //CodigoClassificacaoPedido = data.CodigoClassificacaoPedido;
            //CodigoGrupoPedido = data.CodigoGrupoPedido;
            PercentualRaterioRisco = data.PercentualRaterioRisco;
            PercentualMelhorRealizavel = data.PercentualMelhorRealizavel;
            ValorReceitaMedia = data.ValorReceitaMedia;
            DataBaseReceitaMedia = data.DataBaseReceitaMedia;
            CodigoRiscoPerda = data.CodigoRiscoPerda;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {

        }
    }
}
