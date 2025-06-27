using AutoMapper;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso
{
    public class JuroCorrecaoProcessoInputViewModel
    {
        public long? CodTipoProcesso { get; set; }

        public DateTime? DataVigencia { get; set; }

        public double? ValorJuros { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<JuroCorrecaoProcessoInputViewModel, JuroCorrecaoProcesso>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.CodTipoProcesso));

            mapper.CreateMap<JuroCorrecaoProcesso, JuroCorrecaoProcessoInputViewModel>()
                  .ForMember(dest => dest.CodTipoProcesso, opt => opt.MapFrom(orig => orig.Id));
        }
    }
}
