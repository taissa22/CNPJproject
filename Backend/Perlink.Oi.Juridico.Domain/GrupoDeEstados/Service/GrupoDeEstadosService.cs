using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Service;
using Shared.Domain.Impl.Service;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Service
{
    public class GrupoDeEstadosService : BaseCrudService<Entity.GrupoDeEstados, long>, IGrupoDeEstadosService
    {
        private readonly IGrupoDeEstadosRepository grupoDeEstadosRepository;
        private readonly IGrupoEstadosRepository grupoEstadoRepository;
        private const string _dadosAlteradosReload = "Os dados foram alterados por outro usuário. Será feita uma recarga para sincronizar as informações.";
        private const string _inclusaoNaoPermitida = "Inclusão não Permitida. Já existe um grupo cadastrado com esse nome.";
        private const string _existeGruposSemEstadoAssociado = "Existem grupos sem estado associado.";

        public GrupoDeEstadosService(IGrupoDeEstadosRepository _grupoDeEstadosRepository, IGrupoEstadosRepository _grupoEstadosRepository)
            : base(_grupoDeEstadosRepository)
        {
            this.grupoDeEstadosRepository = _grupoDeEstadosRepository;
            this.grupoEstadoRepository = _grupoEstadosRepository;
        }

        public void Atualizar(IList<GrupoDeEstadosDTO> grupos)
        {
            var existeGrupoSemEstado = grupos.Any(x => x.EstadosGrupo.Count() < 1);

            if (existeGrupoSemEstado)
                throw new Exception(_existeGruposSemEstadoAssociado);

            ValidarConsistenciaDosGrupos(grupos);

            foreach (var grupo in grupos)
            {
                GrupoDeEstados.Entity.GrupoDeEstados grpBD = grupoDeEstadosRepository.RecuperarGrupoPorNome(grupo.NomeAnterior);

                if (grpBD != null && !grupo.Nome.Equals(grpBD.NomeGrupo))
                {
                    if (grupoDeEstadosRepository.NomeGrupoJaCadastrado(grupo.Nome))
                    {
                        throw new Exception(_inclusaoNaoPermitida);
                    }

                    grpBD.AtualizarNome(grupo.Nome);
                    grupoDeEstadosRepository.AtualizarGrupo(grpBD);
                }

                if (grupo.Excluido)
                {
                    if (grpBD is null)
                        throw new Exception(_dadosAlteradosReload);

                    grupoEstadoRepository.ExcluirGrupoEstado(grpBD.GrupoEstados);
                    grupoDeEstadosRepository.ExcluirGrupo(grpBD);
                    grupoDeEstadosRepository.SaveChanges();
                    continue;
                }

                #region Remove as empresas que saíram

                if (grpBD != null)
                {
                    var removidos = grpBD.GrupoEstados.Where(x => !grupo.EstadosGrupo.Select(y => y.Id).Contains(x.EstadoId));
                    if (removidos != null && removidos.Count() > 0)
                    {
                        grupoEstadoRepository.ExcluirGrupoEstado(removidos);
                    }
                }

                #endregion Remove as empresas que saíram

                #region Insere as empresas novas

                IList<GrupoEstados> lstGxE = new List<GrupoEstados>();
                bool novoRegistro = false;

                if (grpBD is null)
                {
                    novoRegistro = true;
                    grpBD = new GrupoDeEstados.Entity.GrupoDeEstados()
                    {
                        NomeGrupo = grupo.Nome,
                        GrupoEstados = new List<GrupoEstados>()
                    };
                }

                foreach (var estado in grupo.EstadosGrupo)
                {
                    if (!grpBD.GrupoEstados.Select(x => x.EstadoId).Contains(estado.Id))
                    {
                        lstGxE.Add(new GrupoEstados()
                        {
                            EstadoId = estado.Id,
                            GrupoId = grpBD.Id
                        });
                    }
                }

                if (lstGxE.Count() > 0)
                {
                    if (novoRegistro)
                    {
                        grpBD.GrupoEstados = lstGxE;
                        grupoDeEstadosRepository.CriarGrupo(grpBD);
                    }
                    else
                    {
                        grupoDeEstadosRepository.CriarGrupoEstado(lstGxE);
                    }
                }

                #endregion Insere as empresas novas

                grupoDeEstadosRepository.SaveChanges();
            }
        }

        private void ValidarConsistenciaDosGrupos(IList<GrupoDeEstadosDTO> grupos)
        {
            Entity.GrupoDeEstados grupoPersistido = null;
            foreach (var grupo in grupos)
            {
                // Valida se excluíram o grupo
                if (grupo.Persistido)
                {
                    grupoPersistido = grupoDeEstadosRepository.RecuperarGrupoPorNome(grupo.NomeAnterior);

                    if (grupoPersistido is null)
                        throw new Exception(_dadosAlteradosReload);

                    // Valida se algum grupo foi alterado por outro usuário
                    var EstadosSalvas = grupoPersistido.GrupoEstados.Select(x => x.EstadoId).ToArray();

                    //if (EstadosSalvas.Count() != grupo.EstadosIniciais)
                    //{
                    //    throw new Exception(_dadosAlteradosReload);
                    //}
                }

                foreach (var estado in grupo.EstadosGrupo)
                {
                    // Valida se o estado adicionada já está em uso
                    if (!estado.Persistido)
                    {
                        if (grupoDeEstadosRepository.EstadoEstaEmUso(estado.Id, grupos.Select(t => t.NomeAnterior).ToArray()))
                        {
                            throw new Exception(_dadosAlteradosReload);
                        }
                    }
                }
            }
        }
    }
}
