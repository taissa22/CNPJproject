using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBModalidadeViewModel
    {
        [Name("Código")]
        public long Id { get; set; }
        [Name("Modalidade BB")]
        public long CodigoBB { get; set; }
        [Name("Descrição")]
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        { 
        //{
        //    mapper.CreateMap<BBModalidadeViewModel, BBModalidadeDTO>();

        //    mapper.CreateMap<BBModalidadeDTO, BBModalidadeViewModel>();

            mapper.CreateMap<BBModalidadeViewModel, BBModalidade>();

            mapper.CreateMap<BBModalidade, BBModalidadeViewModel>();
        }

        public class BBModalidadeExportarViewModel
        {
            [Name("Código")]
            public long Id { get; set; }
            [Name("Modalidade BB")]
            public string CodigoBB { get; set; }
            [Name("Descrição")]
            public string Descricao { get; set; }

            public static void Mapping(Profile mapper)
            {
                mapper.CreateMap<BBModalidade, BBModalidadeExportarViewModel>()
                .ForMember(dest => dest.CodigoBB, opt => opt.MapFrom(orig => $"'{orig.CodigoBB.ToString()}"));

            }
        }
    }
}
