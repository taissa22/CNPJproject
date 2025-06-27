using AutoMapper;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Shared.Application.ViewModel;
using System;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.GrupoDeEstados.ViewModel
{
    public class AgregadoGrupoDeEstadosViewModel 
    {
        public List<GrupoEstadosViewModel> GruposEstados { get; set; }

        public List<EstadoViewModel> EstadosDisponiveis { get; set; }
    }
}
