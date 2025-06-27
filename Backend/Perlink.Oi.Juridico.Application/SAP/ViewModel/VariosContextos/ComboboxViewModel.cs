using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos
{
    public class ComboboxViewModel<TType>
    {
        public TType Id { get; set; }
        public string Descricao { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBComarca, ComboboxViewModel<int>>();

            mapper.CreateMap<Vara, ComboboxViewModel<long>>()
                .ForMember(e => e.Id, opt => opt.MapFrom(vara => vara.CodigoVara))
                .ForMember(e => e.Descricao, opt => opt.MapFrom(vara => vara.Id.ToString() + "a"));

            mapper.CreateMap<TipoVara, ComboboxViewModel<long>>()
                .ForMember(e => e.Id, opt => opt.MapFrom(tipoVara => tipoVara.Id))
                .ForMember(e => e.Descricao, opt => opt.MapFrom(tipoVara => tipoVara.NomeTipoVara));
        }
    }
}
