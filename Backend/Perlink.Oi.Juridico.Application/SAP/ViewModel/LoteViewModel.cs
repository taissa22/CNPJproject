using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class LoteViewModel : BaseViewModel<long>
    {
        public long CodigoTipoProcesso { get; set; }
        public long CodigoParte { get; set; }
        public long CodigoFornecedor { get; set; }
        public long CodigoCentroCusto { get; set; }
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
        public string NomeUsuario { get; set; }

        public static void Mapping(Profile mapper)
        {
            //mapper.CreateMap<LoteViewModel, Lote>();
            //mapper.CreateMap<Lote, LoteViewModel>();
            mapper.CreateMap<LoteViewModel, Lote>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.CodigoTipoProcesso, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso))
                .ForMember(dest => dest.CodigoParte, opt => opt.MapFrom(orig => orig.CodigoParte))
                .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(orig => orig.CodigoFornecedor))
                .ForMember(dest => dest.CodigoCentroCusto, opt => opt.MapFrom(orig => orig.CodigoCentroCusto))
                .ForMember(dest => dest.CodigoCentroSAP, opt => opt.MapFrom(orig => orig.CodigoCentroSAP))
                .ForMember(dest => dest.CodigoStatusPagamento, opt => opt.MapFrom(orig => orig.CodigoStatusPagamento))
                .ForMember(dest => dest.CodigoUsuario, opt => opt.MapFrom(orig => orig.CodigoUsuario))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(orig => orig.DataCriacao != null ? orig.DataCriacao : (DateTime?)null))
                .ForMember(dest => dest.DataRecebimentoFisico, opt => opt.MapFrom(orig => orig.DataRecebimentoFisico != null ? orig.DataRecebimentoFisico : (DateTime?)null))
                .ForMember(dest => dest.DataGeracaoArquivoBB, opt => opt.MapFrom(orig => orig.DataGeracaoArquivoBB != null ? orig.DataGeracaoArquivoBB : (DateTime?)null))
                .ForMember(dest => dest.DataRetornoBB, opt => opt.MapFrom(orig => orig.DataRetornoBB != null ? orig.DataRetornoBB : (DateTime?)null))
                .ForMember(dest => dest.DataErro, opt => opt.MapFrom(orig => orig.DataErro != null ? orig.DataErro : (DateTime?)null))
                .ForMember(dest => dest.DataCriacaoPedido, opt => opt.MapFrom(orig => orig.DataCriacaoPedido != null ? orig.DataCriacaoPedido : (DateTime?)null))
                .ForMember(dest => dest.DataCancelamentoLote, opt => opt.MapFrom(orig => orig.DataCancelamentoLote != null ? orig.DataCancelamentoLote : (DateTime?)null))
                .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido != null ? orig.DataPagamentoPedido : (DateTime?)null))
                .ForMember(dest => dest.NumeroLoteBB, opt => opt.MapFrom(orig => orig.NumeroLoteBB != null ? orig.NumeroLoteBB : (long?)null))
                .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
                .ForMember(dest => dest.UltimaSeqBordero, opt => opt.MapFrom(orig => orig.UltimaSeqBordero))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor));



            mapper.CreateMap<Lote, LoteViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.CodigoTipoProcesso, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso))
                .ForMember(dest => dest.CodigoParte, opt => opt.MapFrom(orig => orig.CodigoParte))
                .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(orig => orig.CodigoFornecedor))
                .ForMember(dest => dest.CodigoCentroCusto, opt => opt.MapFrom(orig => orig.CodigoCentroCusto))
                .ForMember(dest => dest.CodigoCentroSAP, opt => opt.MapFrom(orig => orig.CodigoCentroSAP))
                .ForMember(dest => dest.CodigoStatusPagamento, opt => opt.MapFrom(orig => orig.CodigoStatusPagamento))
                .ForMember(dest => dest.CodigoUsuario, opt => opt.MapFrom(orig => orig.CodigoUsuario))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(orig => orig.DataCriacao != null ? orig.DataCriacao : (DateTime?)null))
                .ForMember(dest => dest.DataRecebimentoFisico, opt => opt.MapFrom(orig => orig.DataRecebimentoFisico != null ? orig.DataRecebimentoFisico : (DateTime?)null))
                .ForMember(dest => dest.DataGeracaoArquivoBB, opt => opt.MapFrom(orig => orig.DataGeracaoArquivoBB != null ? orig.DataGeracaoArquivoBB : (DateTime?)null))
                .ForMember(dest => dest.DataRetornoBB, opt => opt.MapFrom(orig => orig.DataRetornoBB != null ? orig.DataRetornoBB : (DateTime?)null))
                .ForMember(dest => dest.DataErro, opt => opt.MapFrom(orig => orig.DataErro != null ? orig.DataErro : (DateTime?)null))
                .ForMember(dest => dest.DataCriacaoPedido, opt => opt.MapFrom(orig => orig.DataCriacaoPedido != null ? orig.DataCriacaoPedido : (DateTime?)null))
                .ForMember(dest => dest.DataCancelamentoLote, opt => opt.MapFrom(orig => orig.DataCancelamentoLote != null ? orig.DataCancelamentoLote : (DateTime?)null))
                .ForMember(dest => dest.DataPagamentoPedido, opt => opt.MapFrom(orig => orig.DataPagamentoPedido != null ? orig.DataPagamentoPedido : (DateTime?)null))
                .ForMember(dest => dest.NumeroLoteBB, opt => opt.MapFrom(orig => orig.NumeroLoteBB != null ? orig.NumeroLoteBB : (long?)null))
                .ForMember(dest => dest.NumeroPedidoSAP, opt => opt.MapFrom(orig => orig.NumeroPedidoSAP))
                .ForMember(dest => dest.UltimaSeqBordero, opt => opt.MapFrom(orig => orig.UltimaSeqBordero))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor));
         
        }
    }
}