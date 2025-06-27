using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class LoteResultadoViewModel : BaseViewModel<long>
    {
        public long Id { get; set; }
        public string DescricaoLote { get; set; }
        public string FormaPagamento { get; set; }
        public string NomeEmpresaGrupo { get; set; }
        public string NomeUsuario { get; set; }
        public string DataCriacao { get; set; }
        public string StatusPagamento { get; set; }
        public string DataCriacaoPedido { get; set; }
        public long NumeroPedidoSAP { get; set; }
        public long? NumeroLoteBB { get; set; }
        public bool ExisteBordero { get; set; } // TODO: Aplicar existeBordero
        public long CodigoStatusPagamento { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<LoteResultadoDTO, LoteResultadoViewModel>();
        }
    }
}