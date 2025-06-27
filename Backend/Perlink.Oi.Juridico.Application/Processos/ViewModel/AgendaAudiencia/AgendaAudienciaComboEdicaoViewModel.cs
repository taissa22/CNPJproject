using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class AgendaAudienciaComboEdicaoViewModel
    {
        public IEnumerable<PrepostoDTO> ListaPrepostos { get; set; }
        public IEnumerable<EscritorioDTO> ListaEscritorios { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendaAudienciaComboEdicaoDTO, AgendaAudienciaComboEdicaoViewModel>();
           
        }
}
}
