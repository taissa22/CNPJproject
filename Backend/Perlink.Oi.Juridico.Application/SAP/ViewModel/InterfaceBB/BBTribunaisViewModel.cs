using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBTribunaisViewModel
    {
        [Name("Código")]
        public long Id { get; set; }

        [Name("Tribunal BB")]
        public long CodigoBB { get; set; }

        [Name("Instância Designada")]
        public string IndicadorInstancia { get; set; }

        [Name("Descrição do Tribunal")]
        public string Descricao { get; set; }

        [Name("Descrição Instância Designada")]
        public string DescricaoIndicadorInstancia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBTribunaisViewModel, BBTribunais>();

            mapper.CreateMap<BBTribunais, BBTribunaisViewModel>()
                .ForMember(dest => dest.DescricaoIndicadorInstancia, opt => opt.MapFrom(orig => orig.IndicadorInstancia == "E" ? "Estadual" : orig.IndicadorInstancia == "F" ? "Federal" : "TRT"));
        }

        public class BBTribunaisExportarViewModel
        {
            [Name("Código")]
            public long Id { get; set; }

            [Name("Tribunal BB")]
            public string CodigoBB { get; set; }

            [Name("Instância Designada")]
            public string IndicadorInstancia { get; set; }

            [Name("Descrição do Tribunal")]
            public string Descricao { get; set; }

            public static void Mapping(Profile mapper)
            {
                mapper.CreateMap<BBTribunais, BBTribunaisExportarViewModel>()
                     .ForMember(dest => dest.IndicadorInstancia, opt => opt.MapFrom(orig => orig.IndicadorInstancia == "E" ? "Estadual" : orig.IndicadorInstancia == "F" ? "Federal" : "TRT"))
                        .ForMember(dest => dest.CodigoBB, opt => opt.MapFrom(orig => $"'{orig.CodigoBB.ToString()}"));
                mapper.CreateMap<BBTribunaisExportarViewModel, BBTribunais>();
            }
        }
    }
}