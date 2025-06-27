using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao {
    public class LoteCriacaoResultadoLancamentoViewModel {
        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }
        public string DescricaoEscritorio { get; set; }
        public string NomeAutor { get; set; }
        public string NumeroProcesso { get; set; }
        public decimal ValorLiquido { get; set; }
        public string DescricaoComarca { get; set; }
        public string DescricaoVara { get; set; }
        public string DescricaoTipoLancamento { get; set; }
        public string DescricaoCategoriaPagamento { get; set; }
        public string DataCriacaoLancamento { get; set; }
        public string TextoSAPIdentificacaoDoUsuario { get; set; }
        public string TextoSAP { get; set; }
        public long CodigoParte { get; set; }
        public long CodigoStatusPagamento { get; set; }
        public long? NumeroGuia { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<LoteCriacaoResultadoLancamentoDTO, LoteCriacaoResultadoLancamentoViewModel>()
                .ForMember(dest => dest.DescricaoComarca, opt => opt.MapFrom(orig => string.Format("{0} - {1}", orig.Uf, orig.NomeComarca)))
                .ForMember(dest => dest.DescricaoVara, opt => opt.MapFrom(orig => string.Format("{0}ª {1}", orig.CodigoVara, orig.NomeTipoVara)))
                .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(orig => orig.NomeParte))
                .ForMember(dest => dest.TextoSAPIdentificacaoDoUsuario, opt => opt.MapFrom(orig => orig.TextoSAP.Length > 145 ? orig.TextoSAP.Substring(145) : string.Empty));
                

            mapper.CreateMap<LoteCriacaoResultadoLancamentoViewModel, LoteCriacaoResultadoLancamentoDTO>();

        }
    }
}
