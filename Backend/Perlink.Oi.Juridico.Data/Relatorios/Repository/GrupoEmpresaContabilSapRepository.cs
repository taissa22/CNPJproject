using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Relatorios.Repository
{
    public class GrupoEmpresaContabilSapRepository : BaseCrudRepository<GrupoEmpresaContabilSap, long>, IGrupoEmpresaContabilSapRepository
    {
        private readonly IGrupoEmpresaContabilSapParteRepository repository;

        public GrupoEmpresaContabilSapRepository(JuridicoContext context, IAuthenticatedUser user, IGrupoEmpresaContabilSapParteRepository _repository)
            : base(context, user)
        {
            repository = _repository;
        }

        /// <summary>
        /// Cria o novo grupo
        /// </summary>
        /// <param name="g">Instância do novo grupo</param>
        /// <returns></returns>
        public void CriarGrupo(GrupoEmpresaContabilSap g)
        {
            context.Add(g);
        }

        /// <summary>
        /// Cria o relacionamento Grupo x Empresa
        /// </summary>
        public void CriarGrupoXEmpresa(IList<GrupoEmpresaContabilSapParte> ge)
        {
            context.AddRange(ge);
        }

        /// <summary>
        /// Remove o grupo
        /// </summary>
        /// <param name="g">Instância do novo grupo</param>
        /// <returns></returns>
        public void ExcluirGrupo(GrupoEmpresaContabilSap grupo)
        {
            repository.ExcluirGrupoXEmpresa(grupo.Id);
            context.Remove(grupo);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Recupera os grupos por nome ou pelo nome das empresas associadas
        /// </summary>
        public async Task<IList<GrupoEmpresaContabilSap>> ListarGrupoEmpresaContabilSap()
        {
            return await context.Set<GrupoEmpresaContabilSap>()
                           .AsNoTracking()
                           .Include(x => x.GrupoEmpresaContabilSapParte)
                           .ThenInclude(x => x.Empresa)
                           .OrderBy(x => x.NomeGrupo)
                           .ThenBy(x => x.Id)
                           .ToArrayAsync();
        }

        /// <summary>
        /// Valida se já existe grupo com o nome informado
        /// </summary>
        /// <param name="nome">Nome do Grupo</param>
        /// <returns></returns>
        public bool NomeGrupoJaCadastrado(string nome)
        {
            return context.Set<GrupoEmpresaContabilSap>()
                            .AsNoTracking()
                            .Any(x => x.NomeGrupo.Equals(nome));
        }

        /// <summary>
        /// Valida se uma empresa já está em uso por algum grupo
        /// </summary>
        /// <param name="id">Id grupo</param>
        public bool EmpresaEstaEmUso(long empresaId, string[] nomesGrupos)
        {
            return context.Set<GrupoEmpresaContabilSapParte>()
                            .Any(x => x.Empresa.Id == empresaId && !nomesGrupos.Contains(x.GrupoEmpresaContabilSap.NomeGrupo));
        }

        /// <summary>
        /// Recupera um grupo por nome
        /// </summary>
        /// <param name="nome">Id grupo, pois o nome é exclusivo</param>
        public GrupoEmpresaContabilSap RecuperarGrupoPorNome(string nome)
        {
            return context.Set<GrupoEmpresaContabilSap>()
                            .Include(x => x.GrupoEmpresaContabilSapParte)
                            .ThenInclude(x => x.Empresa)
                            .FirstOrDefault(x => x.NomeGrupo.Equals(nome));
        }

        /// <summary>
        /// Recupera um grupo por id
        /// </summary>
        public void AtualizarGrupo(GrupoEmpresaContabilSap grp)
        {
            context.Update(grp);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}