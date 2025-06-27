using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;
using System;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class LoteDetalhesViewModel : BaseViewModel<long>

    {
        public string NomeEmpresaGrupo { get; set; }
        public string Fornecedor { get; set; }
        public long QuantLancamento { get; set; }
        public DateTime DataEnvioEscritorio { get; set; }
        public string FormaPagamento { get; set; }
        public long? NumeroLoteBB { get; set; }
        public string CentroCusto { get; set; }
        public double Valor { get; set; }

        public bool exibirLoteBB = true;
        public string DataRetornoBB { get; set; }
        public string DataGeracaoArquivoBB { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LoteDetalhesDTO, LoteDetalhesViewModel>();
            // .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
            // .ForMember(dest => dest.NomeEmpresaGrupo, opt => opt.MapFrom(orig => orig.NomeEmpresaGrupo))
            // .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(orig => orig.Fornecedor))
            // .ForMember(dest => dest.QuantLancamento, opt => opt.MapFrom(orig => orig.QuantLancamento))
            // .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio))
            // .ForMember(dest => dest.FormaPagamento, opt => opt.MapFrom(orig => orig.FormaPagamento))
            // .ForMember(dest => dest.CentroCusto, opt => opt.MapFrom(orig => orig.CentroCusto))
            // .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor));

            mapper.CreateMap<LoteDetalhesViewModel, LoteDetalhesDTO>();
            // .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
            // .ForMember(dest => dest.NomeEmpresaGrupo, opt => opt.MapFrom(orig => orig.NomeEmpresaGrupo))
            // .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(orig => orig.Fornecedor))
            // .ForMember(dest => dest.QuantLancamento, opt => opt.MapFrom(orig => orig.QuantLancamento))
            // .ForMember(dest => dest.DataEnvioEscritorio, opt => opt.MapFrom(orig => orig.DataEnvioEscritorio))
            // .ForMember(dest => dest.FormaPagamento, opt => opt.MapFrom(orig => orig.FormaPagamento))
            // .ForMember(dest => dest.CentroCusto, opt => opt.MapFrom(orig => orig.CentroCusto))
            // .ForMember(dest => dest.Valor, opt => opt.MapFrom(orig => orig.Valor));
        }
    }
}