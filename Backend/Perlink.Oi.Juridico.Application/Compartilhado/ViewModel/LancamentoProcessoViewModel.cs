using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel {
    public class LancamentoProcessoViewModel : BaseViewModel<long>
    {
       public long CodigoLancamento { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public long CodigoCatPagamento { get; set; }
        public long QuantidadeLancamentos { get; set; }
        public long? ValorLancamento { get; set; }
        public DateTime DataLancamento { get; set; }
        public string ComentarioSap { get; set; }
        public long? NumeroPedidoSap { get; set; }
        public DateTime? DataCriacaoPedido { get; set; }
        public DateTime? DataPagamentoPedido { get; set; }
        public string CodigoUsuarioRecebedor { get; set; }
        public long? NumeroGuia { get; set; }
        public DateTime? DataEnvioEscritorio { get; set; }
        public bool IndExcluido { get; set; }
        public string Comentario { get; set; }
        public DateTime? DatRecebimentoFisico { get; set; }
        public long CodigoAutenticacaoEletronica { get; set; }
        public long DataEfetivacaoParcela { get; set; }
        public long CodigoStatusPagamento { get; set; }

        public long? NumeroContaJudicial { get; set; }
        public long? NumeroParcelaContaJudicial { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LancamentoProcessoViewModel, LancamentoProcesso>();


            mapper.CreateMap<LancamentoProcesso, LancamentoProcessoViewModel>();
               
        }
    }
}
