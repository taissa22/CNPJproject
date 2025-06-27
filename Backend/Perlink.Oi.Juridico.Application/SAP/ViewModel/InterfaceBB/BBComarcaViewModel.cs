using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBComarcaViewModel 
    {
        public long Id { get; set; }
        public long CodigoBB { get; set; }
        public string Descricao { get; set; }
        public string CodigoEstado { get; set; }
        public bool ConfirmaCardastro { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBComarcaViewModel, BBComarca>();

            mapper.CreateMap<BBComarca, BBComarcaViewModel>();
        }
    }
    public class BBComarcaExportarViewModel
    {
        [Name("Código")]
        public long Id { get; set; }
        [Name("Comarca BB")]
        public string CodigoBB { get; set; }
        [Name("UF")]
        public string CodigoEstado { get; set; }
        [Name("Descrição")]
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBComarcaExportarViewModel, BBComarca>();

            mapper.CreateMap<BBComarca, BBComarcaExportarViewModel>()
            .ForMember(dest => dest.CodigoBB, opt => opt.MapFrom(orig => $"'{orig.CodigoBB.ToString()}"));
        }
    }
}
