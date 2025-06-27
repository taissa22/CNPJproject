using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBStatusParcelasViewModel 
    {
        [Name("Código")]
        public long Id {get; set;}
        [Name("Status Parcela BB")]
        public long CodigoBB { get; set; }
        [Name("Descrição")]
        public string Descricao { get; set; }


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBStatusParcelasViewModel, BBStatusParcelas>();

            mapper.CreateMap<BBStatusParcelas, BBStatusParcelasViewModel>();
           
        }

        public class BBStatusParcelasExportarViewModel
        {
            [Name("Código")]
            public long Id { get; set; }
            [Name("Status Parcela BB")]
            public string CodigoBB { get; set; }
            [Name("Descrição")]
            public string Descricao { get; set; }

            public static void Mapping(Profile mapper)
            {
                mapper.CreateMap<BBStatusParcelas, BBStatusParcelasExportarViewModel>()
               .ForMember(dest => dest.CodigoBB, opt => opt.MapFrom(orig => $"'{orig.CodigoBB.ToString()}"));
                mapper.CreateMap<BBStatusParcelasExportarViewModel, BBStatusParcelas>();
            }
        }
    }
}
