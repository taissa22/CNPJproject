using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBNaturezasAcoesViewModel
    {
        [Name("Código")]
        public long Id { get; set; }
        [Name("Natureza Ação BB")]
        public long CodigoBB { get; set; }
        [Name("Descrição")]
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBNaturezasAcoesViewModel, BBNaturezasAcoes>();

            mapper.CreateMap<BBNaturezasAcoes, BBNaturezasAcoesViewModel>();
        }

        public class BBNaturezasAcoesExportarViewModel
        {
            [Name("Código")]
            public long Id { get; set; }
            [Name("Natureza Ação BB")]
            public string CodigoBB { get; set; }
            [Name("Descrição")]
            public string Descricao { get; set; }

            public static void Mapping(Profile mapper)
            {
                mapper.CreateMap<BBNaturezasAcoes, BBNaturezasAcoesExportarViewModel>()
               .ForMember(dest => dest.CodigoBB, opt => opt.MapFrom(orig => $"'{orig.CodigoBB.ToString()}"));
                mapper.CreateMap<BBNaturezasAcoesExportarViewModel, BBNaturezasAcoes>();
            }
        }
    }
}