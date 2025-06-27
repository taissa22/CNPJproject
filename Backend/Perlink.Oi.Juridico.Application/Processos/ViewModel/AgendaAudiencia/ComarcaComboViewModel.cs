using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.AgendaAudiencia
{
    public class ComarcaComboViewModel 
    {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ComarcaDTO, ComarcaComboViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
             .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));
        }
    }
}
