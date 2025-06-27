using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Service;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Domain.Relatorios.Service
{
    public class GrupoEmpresaContabilSapService : BaseCrudService<GrupoEmpresaContabilSap, long>, IGrupoEmpresaContabilSapService
    {
        private readonly IGrupoEmpresaContabilSapRepository grupoRepository;
        private readonly IGrupoEmpresaContabilSapParteRepository repository;
        private const string _dadosAlteradosReload = "Os dados foram alterados por outro usuário. Será feita uma recarga para sincronizar as informações.";
        private const string _inclusaoNaoPermitida = "Já existe um grupo cadastrado com nome '{0}'. \n Os dados foram alterados por outro usuário. \nSerá feita uma recarga.";
        private const string _existeGruposSemEmpresaAssociada = "Existem grupos sem empresa associada.";

        public GrupoEmpresaContabilSapService(IGrupoEmpresaContabilSapRepository _grupoRepository, IGrupoEmpresaContabilSapParteRepository _repository)
            : base(_grupoRepository)
        {
            grupoRepository = _grupoRepository;
            repository = _repository;
        }

        /// <summary>
        /// Atualiza os grupos
        /// </summary>
        /// <param name="grp">Grupo</param>
        public void Atualizar(IList<GrupoEmpresaContabilSapDTO> grupos)
        {
            var existeGrupoSemEmpresa = grupos.Any(x => x.EmpresasGrupo.Count() < 1);

            if (existeGrupoSemEmpresa)
                throw new Exception(_existeGruposSemEmpresaAssociada);

            ValidarConsistenciaDosGrupos(grupos);

            foreach (var grupo in grupos)
            {
                GrupoEmpresaContabilSap grpBD = grupoRepository.RecuperarGrupoPorNome(grupo.NomeAnterior);

                if (grpBD != null)
                {
                    if (!grupo.Nome.Equals(grpBD.NomeGrupo))
                        grpBD.AtualizarNome(grupo.Nome);

                    grpBD.AtualizarFlagRecuperanda(grupo.Recuperanda);
                    grupoRepository.AtualizarGrupo(grpBD);
                }

                if (grupo.Excluido)
                {
                    if (grpBD is null)
                        throw new Exception(_dadosAlteradosReload);

                    repository.ExcluirGrupoXEmpresa(grpBD.GrupoEmpresaContabilSapParte);
                    grupoRepository.ExcluirGrupo(grpBD);
                    grupoRepository.SaveChanges();
                    continue;
                }

                #region Remove as empresas que saíram

                if (grpBD != null)
                {
                    var removidos = grpBD.GrupoEmpresaContabilSapParte.Where(x => !grupo.EmpresasGrupo.Select(y => y.Id).Contains(x.Id));
                    if (removidos != null && removidos.Count() > 0)
                    {
                        repository.ExcluirGrupoXEmpresa(removidos);
                    }
                }

                #endregion Remove as empresas que saíram

                #region Insere as empresas novas

                IList<GrupoEmpresaContabilSapParte> lstGxE = new List<GrupoEmpresaContabilSapParte>();
                bool novoRegistro = false;

                if (grpBD is null)
                {
                    novoRegistro = true;
                    grpBD = new GrupoEmpresaContabilSap()
                    {
                        NomeGrupo = grupo.Nome,
                        Recuperanda = grupo.Recuperanda,
                        GrupoEmpresaContabilSapParte = new List<GrupoEmpresaContabilSapParte>()
                    };
                }

                foreach (var empresa in grupo.EmpresasGrupo)
                {
                    if (!grpBD.GrupoEmpresaContabilSapParte.Select(x => x.Id).Contains(empresa.Id))
                    {
                        lstGxE.Add(new GrupoEmpresaContabilSapParte()
                        {
                            EmpresaId = empresa.ParteId,
                            GrupoId = grpBD.Id
                        });
                    }
                }

                if (lstGxE.Count() > 0)
                {
                    if (novoRegistro)
                    {
                        grpBD.GrupoEmpresaContabilSapParte = lstGxE;
                        grupoRepository.CriarGrupo(grpBD);
                    }
                    else
                    {
                        grupoRepository.CriarGrupoXEmpresa(lstGxE);
                    }
                }

                #endregion Insere as empresas novas

                grupoRepository.SaveChanges();
            }
        }

        /// <summary>
        /// Valida se existe alguma empresa sendo associada que já está associada a um grupo. Realiza o reload para atualizar os dados da tela
        /// </summary>
        private void ValidarConsistenciaDosGrupos(IList<GrupoEmpresaContabilSapDTO> grupos)
        {
            GrupoEmpresaContabilSap grupoPersistido = null;
            foreach (var grupo in grupos)
            {
                // Valida se excluíram o grupo
                if (grupo.Persistido)
                {
                    grupoPersistido = grupoRepository.RecuperarGrupoPorNome(grupo.NomeAnterior);

                    if (grupoPersistido is null)
                        throw new Exception(_dadosAlteradosReload);

                    // Valida se algum grupo foi alterado por outro usuário
                    if (grupoPersistido.GrupoEmpresaContabilSapParte.Count() != grupo.QtdEmpresasIniciaisControle)
                    {
                        throw new Exception(_dadosAlteradosReload);
                    }
                }
                else if (grupoRepository.NomeGrupoJaCadastrado(grupo.Nome))
                {
                    throw new Exception(string.Format(_inclusaoNaoPermitida, grupo.Nome));
                }

                foreach (var empresa in grupo.EmpresasGrupo)
                {
                    // Valida se a empresa adicionada já está em uso
                    if (!empresa.Persistido)
                    {
                        if (grupoRepository.EmpresaEstaEmUso(empresa.ParteId, grupos.Select(t => t.NomeAnterior).ToArray()))
                        {
                            throw new Exception(_dadosAlteradosReload);
                        }
                    }
                }
            }
        }
    }
}