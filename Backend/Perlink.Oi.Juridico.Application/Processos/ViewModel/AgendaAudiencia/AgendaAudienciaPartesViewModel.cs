using AutoMapper;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class AgendaAudienciaPartesViewModel
    {
        public IEnumerable<PartesDTO> Autores { get; set; }
        public IEnumerable<PartesDTO> Reus { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendaAudienciaPartesDTO, AgendaAudienciaPartesViewModel>();

        }
    }
}
