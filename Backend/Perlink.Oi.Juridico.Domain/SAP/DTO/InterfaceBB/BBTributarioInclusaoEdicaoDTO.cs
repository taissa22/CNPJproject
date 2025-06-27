using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB
{
    public class BBTributarioInclusaoEdicaoDTO
    {
        public long Id { get; set; }
        public long CodigoBB { get; set; }
        public string Descricao { get; set; }
        public string IndicadorInstancia { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<BBTributarioInclusaoEdicaoDTO, BBTribunais>();
     
            mapper.CreateMap<BBTribunais, BBTributarioInclusaoEdicaoDTO>();
        }
    }
}