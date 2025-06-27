using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class ContratoPedidoProcesso : EntityCrud<ContratoPedidoProcesso, long>
    {
        public long CodigoProcesso { get; set; }
        public long CodigoPedido { get; set; }
        public string CodigoRiscoPerda { get; set; }
        public long? IdTeseAutor { get; set; }
        public DateTime? DataTeseAutor { get; set; }
        public double ValorPrincipalAutor { get; set; }
        public double ValorJurosPrincipalAutor { get; set; }
        public double ValorJurosRendimentoAutor { get; set; }
        public double ValorRetencaoIRAutor { get; set; }
     
        public long? IdTeseOtimista { get; set; }
        public DateTime? DataTeseOtimista { get; set; }
        public double ValorPrincipalOtimista { get; set; }
        public double ValorJurosPrincipalOtimista { get; set; }
        public double ValorJurosRendimentoOtimista { get; set; }
        public double ValorRetencaoIROtimista { get; set; }
   
        public long? IdTesePessimista { get; set; }
        public DateTime? DataTesePessimista { get; set; }
        public double ValorPrincipalPessimista { get; set; }
        public double ValorJurosPrincipalPessimista { get; set; }
        public double ValorJurosRendimentoPessimista { get; set; }
        public double ValorRetencaoIRPessimista { get; set; }

        public long? IdTesePerito { get; set; }
        public DateTime? DataTesePerito { get; set; }
        public double ValorPrincipalPerito { get; set; }
        public double ValorJurosPrincipalPerito { get; set; }
        public double ValorJurosRendimentoPerito { get; set; }
        public double ValorRetencaoIRPerito { get; set; }

        public long? IdTeseAtual { get; set; }
        public DateTime? DataTeseAtual { get; set; }
        public double ValorPrincipalAtual { get; set; }
        public double ValorJurosPrincipalAtual { get; set; }
        public double ValorJurosRendimentoAtual { get; set; }
        public double ValorRetencaoIRAtual { get; set; }
        public override AbstractValidator<ContratoPedidoProcesso> Validator => new ContratoPedidoProcessoValidator();

        public ContratoProcesso ContratoProcesso { get; set; }
        public Pedido Pedido { get; set; }

        public override void PreencherDados(ContratoPedidoProcesso data)
        {
            CodigoProcesso = data.CodigoProcesso;
            CodigoPedido = data.CodigoPedido;
            CodigoRiscoPerda = data.CodigoRiscoPerda;
            IdTeseAutor = data.IdTeseAutor;
            DataTeseAutor = data.DataTeseAutor;
            ValorPrincipalAutor = data.ValorPrincipalAutor;
            ValorJurosPrincipalAutor = data.ValorJurosPrincipalAutor;
            ValorJurosRendimentoAutor = data.ValorJurosRendimentoAutor;
            ValorRetencaoIRAutor = data.ValorRetencaoIRAutor;
            IdTeseOtimista = data.IdTeseOtimista;
            DataTeseOtimista = data.DataTeseOtimista;
            ValorPrincipalOtimista = data.ValorPrincipalOtimista;
            ValorJurosPrincipalOtimista = data.ValorJurosPrincipalOtimista;
            ValorJurosRendimentoOtimista = data.ValorJurosRendimentoOtimista;
            ValorRetencaoIROtimista = data.ValorRetencaoIROtimista;

            IdTesePessimista = data.IdTesePessimista;
            DataTesePessimista = data.DataTesePessimista;
            ValorPrincipalPessimista = data.ValorPrincipalPessimista;
            ValorJurosPrincipalPessimista = data.ValorJurosPrincipalPessimista;
            ValorJurosRendimentoPessimista = data.ValorJurosRendimentoPessimista;
            ValorRetencaoIRPessimista = data.ValorRetencaoIRPessimista;


            IdTesePerito = data.IdTesePerito;
            DataTesePerito = data.DataTesePerito;
            ValorPrincipalPerito = data.ValorPrincipalPerito;
            ValorJurosPrincipalPerito = data.ValorJurosPrincipalPerito;
            ValorJurosRendimentoPerito = data.ValorJurosRendimentoPerito;
            ValorRetencaoIRPerito = data.ValorRetencaoIRPerito;

            IdTeseAtual = data.IdTeseAtual;
            DataTeseAtual = data.DataTeseAtual;
            ValorPrincipalAtual = data.ValorPrincipalAtual;
            ValorJurosPrincipalAtual = data.ValorJurosPrincipalAtual;
            ValorJurosRendimentoAtual = data.ValorJurosRendimentoAtual;
            ValorRetencaoIRAtual = data.ValorRetencaoIRAtual;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class ContratoPedidoProcessoValidator : AbstractValidator<ContratoPedidoProcesso>
        {
            public ContratoPedidoProcessoValidator()
            {
            }
        }
    }
}