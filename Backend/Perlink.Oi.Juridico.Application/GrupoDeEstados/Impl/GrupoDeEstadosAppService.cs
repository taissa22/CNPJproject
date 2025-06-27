using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.Interface;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.ViewModel;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.GrupoDeEstados.Impl
{
    public class GrupoDeEstadosAppService : BaseCrudAppService<GrupoEstadosViewModel, Domain.GrupoDeEstados.Entity.GrupoDeEstados, long>, IGrupoDeEstadosAppService
    {
        public readonly IGrupoDeEstadosService service;
        private readonly IGrupoDeEstadosRepository repository;
        private readonly IEstadoRepository estadoRepository;


        public GrupoDeEstadosAppService(IGrupoDeEstadosService _service, IGrupoDeEstadosRepository _repository, IEstadoRepository _estadoRepository, IMapper _mapper)
            : base(_service, _mapper)
        {
            this.service = _service;
            this.repository = _repository;
            this.estadoRepository = _estadoRepository;
        }

        public CommandResult Atualizar(IList<GrupoDeEstadosDTO> grupos)
        {
            try
            {
                service.Atualizar(grupos);
            }
            catch (Exception e)
            {
                return CommandResult.Invalid(e.Message);
            }

            return CommandResult.Valid();
        }

        public async Task<IList<Domain.GrupoDeEstados.Entity.GrupoDeEstados>> Exportar()
        {
            try
            {
                return await repository.ListarGrupoDeEstados();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResultadoApplication<AgregadoGrupoDeEstadosViewModel>> ListarGrupoDeEstados()
        {
            var resultado = new ResultadoApplication<AgregadoGrupoDeEstadosViewModel>();

            try
            {
                var ListaGrupoEstado = await repository.ListarGrupoDeEstados();
                var ListaEstadoDisponiveis = await estadoRepository.ListarEstadosSemGrupo();

                resultado.DefinirData(this.ToViewModel(ListaGrupoEstado, ListaEstadoDisponiveis));
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        private AgregadoGrupoDeEstadosViewModel ToViewModel(IList<Domain.GrupoDeEstados.Entity.GrupoDeEstados> grupos, IList<Estado> estadosDisponiveis)
        {
            var gruposEstados = new AgregadoGrupoDeEstadosViewModel
            {
                EstadosDisponiveis = new List<EstadoViewModel>(),
                GruposEstados = new List<GrupoEstadosViewModel>()
            };

            gruposEstados.GruposEstados.AddRange(grupos.Select(x => new GrupoEstadosViewModel()
            {
                Id = x.Id,
                Nome = x.NomeGrupo,
                NomeAnterior = x.NomeGrupo,
                Persistido = true,
                EstadosGrupo = x.GrupoEstados.Select(y => new EstadoViewModel()
                {
                    Id = y.EstadoId,
                    Descricao = y.Estado.NomeEstado,
                    Persistido = true,

                }),

                EstadosIniciais = x.GrupoEstados.Select(y => y.Estado.Id).Count()
            }));

            gruposEstados.EstadosDisponiveis.AddRange(estadosDisponiveis.Select(x => new EstadoViewModel()
            {
                Id = x.Id,
                Descricao = x.NomeEstado,
                Persistido = true
            }));

            return gruposEstados;
        }
    }
}
