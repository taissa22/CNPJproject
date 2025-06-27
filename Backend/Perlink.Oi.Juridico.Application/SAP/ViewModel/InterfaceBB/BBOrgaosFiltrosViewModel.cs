using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB {
    public class BBOrgaosFiltrosViewModel {
        public IEnumerable<BBTribunaisComboBoxViewModel> BBTribunais { get; set; }
        public IEnumerable<BBComarcaComboBoxViewModel> BBComarcas { get; set; }
    }

    public class BBComarcaComboBoxViewModel {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<BBComarca, BBComarcaComboBoxViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => string.Format("{0} - {1} ({2})", orig.CodigoEstado, orig.Descricao, orig.Id.ToString("000000000"))));            
        }
    }

    public class BBTribunaisComboBoxViewModel {
        public long Id { get; set; }
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper) {
            mapper.CreateMap<BBTribunais, BBTribunaisComboBoxViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(orig => orig.Descricao));
        }
    }
}
