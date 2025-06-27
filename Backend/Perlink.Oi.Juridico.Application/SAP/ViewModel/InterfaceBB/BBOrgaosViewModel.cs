using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    public class BBOrgaosViewModel
    {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public string NomeBBTribunal { get; set; }
        public long CodigoBBTribunal { get; set; }
        public string NomeBBComarca { get; set; }
        public long CodigoBBComarca { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBOrgaosResultadoDTO, BBOrgaosViewModel>();
            mapper.CreateMap<BBOrgaosViewModel, BBOrgaos>();
        }
    }

    public class BBOrgaosExportarViewModel
    {
        [Name("Código")]
        public string Id { get; set; }

        [Name("Órgão BB")]
        public string Codigo { get; set; }

        [Name("Nome do Órgão BB")]
        public string Nome { get; set; }

        [Name("Tribunal BB")]
        public string NomeBBTribunal { get; set; }

        [Name("Comarca BB")]
        public string NomeBBComarca { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBOrgaosResultadoDTO, BBOrgaosExportarViewModel>()
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(orig => $"'{orig.Codigo}"));
        }
    }
}