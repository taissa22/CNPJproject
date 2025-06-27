using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao
{
    public class LoteCriacaoResultadoEmpresaGrupoViewModel
    {
        public long CodigoEmpresaGrupo { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public string Uf { get; set; }
        public string DescricaoFornecedor { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string DescricaoCentroCusto { get; set; }
        public string CentroSAP { get; set; }
        public int TotalLancamneto { get; set; }
        public long CodigoCentroCusto { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public long CodigoFornecedor { get; set; }
        public bool FormaPagamentoUsaBordero { get; set; }
        public bool LoteValido { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LoteCriacaoResultadoEmpresaGrupoViewModel, LoteCriacaoResultadoEmpresaGrupoDTO>();

            mapper.CreateMap<LoteCriacaoResultadoEmpresaGrupoDTO, LoteCriacaoResultadoEmpresaGrupoViewModel>()
                .ForMember(dest => dest.FormaPagamentoUsaBordero, opt => opt.MapFrom(orig => orig.IndicaBordero == "S"))
                .ForMember(dest => dest.LoteValido, opt => opt.MapFrom(orig => !string.IsNullOrEmpty(orig.FornecedorSAP) && !string.IsNullOrEmpty(orig.CentroSAP)));
        }
    }
}