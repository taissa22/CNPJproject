using AutoMapper;
using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Interface;
using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Service;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Shared.Application.Impl;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Impl
{
    public class GrupoEmpresaContailSapAppService : BaseCrudAppService<GrupoXEmpresaViewModel, GrupoEmpresaContabilSap, long>, IGrupoEmpresaContailSapAppService
    {
        private readonly IGrupoEmpresaContabilSapService service;
        private readonly IGrupoEmpresaContabilSapRepository repository;
        private readonly IParteRepository parteRepository;

        public GrupoEmpresaContailSapAppService(IGrupoEmpresaContabilSapService service, IGrupoEmpresaContabilSapRepository _repository, IMapper mapper, IParteRepository _parteRepository)
            : base(service, mapper)
        {
            this.service = service;
            this.repository = _repository;
            parteRepository = _parteRepository;
        }

        /// <summary>
        /// Atualiza o grupo e as empresas | Chamar validação do grupo antes
        /// </summary>
        public CommandResult Atualizar(IList<GrupoEmpresaContabilSapDTO> grupos)
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

        /// <summary>
        /// Gera o arquivo de exportação da consulta
        /// </summary>
        public async Task<IList<GrupoEmpresaContabilSap>> Exportar()
        {
            try
            {
                return await repository.ListarGrupoEmpresaContabilSap();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lista e formata os grupos com as suas empresas
        /// </summary>
        public async Task<IResultadoApplication<AgregadoGrupoContabilSapViewModel>> ListarGrupoEmpresaContabilSap()
        {
            var resultado = new ResultadoApplication<AgregadoGrupoContabilSapViewModel>();
            try
            {
                var lstGrupoXEmpresa = await repository.ListarGrupoEmpresaContabilSap();
                var lstEmpresasDisponiveis = await parteRepository.ListarEmpresasSapNaoAssociadas();

                resultado.DefinirData(this.ToViewModel(lstGrupoXEmpresa, lstEmpresasDisponiveis));
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        /// <summary>
        /// Converte para a view
        /// </summary>
        private AgregadoGrupoContabilSapViewModel ToViewModel(IList<GrupoEmpresaContabilSap> grupos, IList<Parte> empDisp)
        {
            var gruposEEmpresas = new AgregadoGrupoContabilSapViewModel
            {
                EmpresasDisponiveis = new List<EmpresaViewModel>(),
                GrupoXEmpresas = new List<GrupoXEmpresaViewModel>()
            };

            gruposEEmpresas.GrupoXEmpresas.AddRange(grupos.Select(x => new GrupoXEmpresaViewModel()
            {
                Id = x.Id,
                Nome = x.NomeGrupo,
                NomeAnterior = x.NomeGrupo,
                Persistido = true,
                EmpresasGrupo = x.GrupoEmpresaContabilSapParte.Select(y => new EmpresaViewModel()
                {
                    Id = y.Id,
                    Nome = y.Empresa.Nome,
                    Persistido = true,
                    ParteId = y.GrupoEmpresaContabilSap.GrupoEmpresaContabilSapParte.FirstOrDefault(k => k.Id == y.Id).Empresa.Id
                }),
                QtdEmpresasIniciaisControle = x.GrupoEmpresaContabilSapParte.Select(y => y.Empresa.Id).Count(),
                Recuperanda = x.Recuperanda.HasValue ? x.Recuperanda : false,
                RecuperandaAnterior = x.Recuperanda.HasValue ? x.Recuperanda : false,
            }));

            gruposEEmpresas.EmpresasDisponiveis.AddRange(empDisp.Select(x => new EmpresaViewModel()
            {
                Id = x.Id,
                Nome = x.Nome,
                ParteId = x.Id,
                Persistido = true
            }));

            return gruposEEmpresas;
        }
    }
}