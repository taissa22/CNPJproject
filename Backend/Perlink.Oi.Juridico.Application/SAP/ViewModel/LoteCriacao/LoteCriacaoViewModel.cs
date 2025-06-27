using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao
{
    public class LoteCriacaoViewModel : BaseViewModel<long>
    {
    
        public string IdentificacaoLote { get; set; }
        public double ValorLote { get; set; }
        public DateTime DataCriacaoLote = DateTime.Now;
        public long codigoTipoProcesso { get; set; }
        public long CodigoParteEmpresa { get; set; }
        public long CodigoFornecedor { get; set; }
        public long CodigoCentroCusto { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public string CodigoCentroSAP { get; set; }
        public string CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public long CodigoStatusPagamento = Convert.ToInt64(StatusPagamentoEnum.LoteGeradoAguardandoEnvioSAP);

        //public IList<keLoteLancamento> loteLancamento { get; set; }
        public IList<BorderoDTO> Borderos { get; set; }

        public IList<DadosLoteCriacaoLancamentoDTO> DadosLancamentoDTOs { get; set; }
        //public IList<BorderoDTO> Borderos { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LoteCriacaoViewModel, Lote>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.DescricaoLote, opt => opt.MapFrom(orig => orig.IdentificacaoLote))
              .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.ValorLote))
              .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(orig => orig.DataCriacaoLote))
              .ForMember(dest => dest.CodigoTipoProcesso, opt => opt.MapFrom(orig => orig.codigoTipoProcesso))
              .ForMember(dest => dest.CodigoParte, opt => opt.MapFrom(orig => orig.CodigoParteEmpresa))
              .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(orig => orig.CodigoFornecedor))
              .ForMember(dest => dest.CodigoCentroCusto, opt => opt.MapFrom(orig => orig.CodigoCentroCusto))
              .ForMember(dest => dest.CodigoFormaPagamento, opt => opt.MapFrom(orig => orig.CodigoFormaPagamento))
              .ForMember(dest => dest.CodigoCentroSAP, opt => opt.MapFrom(orig => orig.CodigoCentroSAP))
              .ForMember(dest => dest.CodigoUsuario, opt => opt.MapFrom(orig => orig.CodigoUsuario))
              .ForMember(dest => dest.CodigoStatusPagamento, opt => opt.MapFrom(orig => orig.CodigoStatusPagamento));

            mapper.CreateMap<Lote, LoteCriacaoViewModel>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
              .ForMember(dest => dest.IdentificacaoLote, opt => opt.MapFrom(orig => orig.DescricaoLote))
              .ForMember(dest => dest.ValorLote, opt => opt.MapFrom(orig => orig.Valor))
              .ForMember(dest => dest.DataCriacaoLote, opt => opt.MapFrom(orig => orig.DataCriacao))
              .ForMember(dest => dest.codigoTipoProcesso, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso))
              .ForMember(dest => dest.CodigoParteEmpresa, opt => opt.MapFrom(orig => orig.CodigoParte))
              .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(orig => orig.CodigoFornecedor))
              .ForMember(dest => dest.CodigoCentroCusto, opt => opt.MapFrom(orig => orig.CodigoCentroCusto))
              .ForMember(dest => dest.CodigoFormaPagamento, opt => opt.MapFrom(orig => orig.CodigoFormaPagamento))
              .ForMember(dest => dest.CodigoCentroSAP, opt => opt.MapFrom(orig => orig.CodigoCentroSAP))
              .ForMember(dest => dest.CodigoUsuario, opt => opt.MapFrom(orig => orig.CodigoUsuario))
              .ForMember(dest => dest.CodigoStatusPagamento, opt => opt.MapFrom(orig => orig.CodigoStatusPagamento));

        }
    }
}
