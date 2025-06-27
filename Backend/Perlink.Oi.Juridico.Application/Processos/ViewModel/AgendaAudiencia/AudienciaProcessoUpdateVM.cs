
using AutoMapper;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class AudienciaProcessoUpdateVM
    {
        public long CodProcesso { get; set; }

        public long SequenciaAudiencia { get; set; }

        public long? CodigoProfissional { get; set; }

        public long? CodigoAdvogado { get; set; }

        public long? CodigoProfissionalAcompanhante { get; set; }

        public long? CodigoAdvogadoAcompanhante { get; set; }

        public long? CodigoPreposto { get; set; }

        public long? CodigoPrepostoAcompanhante { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AudienciaProcessoUpdateVM, AudienciaProcesso>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.CodProcesso));
        }
    }
}
