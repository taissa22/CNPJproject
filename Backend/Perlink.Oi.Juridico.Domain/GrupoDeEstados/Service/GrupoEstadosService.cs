using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Service;
using Shared.Domain.Impl.Service;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Service
{
    public class GrupoEstadosService : BaseCrudService<Entity.GrupoEstados, long>, IGrupoEstadosService
    {
        public readonly IGrupoEstadosRepository repository;
        public readonly IUsuarioService usuario;
        public readonly IMapper mapper;

        private readonly INasService nasService;

        public GrupoEstadosService(IGrupoEstadosRepository repository, IUsuarioService usuario, INasService nasService, IMapper mapper) : base(repository)
        {
            this.repository = repository;
            this.usuario = usuario;
            this.nasService = nasService;
            this.mapper = mapper;
        }

    }
}
