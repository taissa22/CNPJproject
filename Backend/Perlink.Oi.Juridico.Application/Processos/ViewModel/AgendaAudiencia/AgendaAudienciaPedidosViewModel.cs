﻿using AutoMapper;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class AgendaAudienciaPedidosViewModel
    {
        public IEnumerable<PedidoEstornoDTO> Pedidos { get; set; }
        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<AgendaAudienciaPedidoDTO, AgendaAudienciaPedidosViewModel>();

        }
    }
}
