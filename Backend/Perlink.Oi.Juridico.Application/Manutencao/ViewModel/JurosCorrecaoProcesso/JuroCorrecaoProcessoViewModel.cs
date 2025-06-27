using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.DTO;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso
{
    public class JuroCorrecaoProcessoViewModel
    {
        [Name("Código")]
        public long CodTipoProcesso { get; set; }

        [Name("Tipo Processo")]
        public string NomTipoProcesso { get; set; }

        [Name("Data de Vigência")]
        public DateTime DataVigencia { get; set; }

        [Name("Valor do Juros")]
        public double? ValorJuros { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<JuroCorrecaoProcessoViewModel, JuroCorrecaoProcessoDTO>()
                  .ForMember(dest => dest.CodTipoProcesso, opt => opt.MapFrom(orig => orig.CodTipoProcesso))
                  .ForMember(dest => dest.NomTipoProcesso, opt => opt.MapFrom(orig => orig.NomTipoProcesso));

            mapper.CreateMap<JuroCorrecaoProcessoDTO, JuroCorrecaoProcessoViewModel>()
                  .ForMember(dest => dest.CodTipoProcesso, opt => opt.MapFrom(orig => orig.CodTipoProcesso))
                  .ForMember(dest => dest.NomTipoProcesso, opt => opt.MapFrom(orig => orig.NomTipoProcesso));
        }
    }
}
