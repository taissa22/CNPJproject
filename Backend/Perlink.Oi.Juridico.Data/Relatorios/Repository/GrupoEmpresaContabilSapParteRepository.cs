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
    public class GrupoEmpresaContabilSapParteRepository : BaseCrudRepository<GrupoEmpresaContabilSapParte, long>, IGrupoEmpresaContabilSapParteRepository
    {
        public GrupoEmpresaContabilSapParteRepository(JuridicoContext context, IAuthenticatedUser user)
            : base(context, user)
        {
        }

        /// <summary>
        /// Remove a associação por Id
        /// </summary>
        /// <param name="g">Instância do novo grupo</param>
        /// <returns></returns>
        public void ExcluirGrupoXEmpresa(long id)
        {
            var grpXEmp = context.Set<GrupoEmpresaContabilSapParte>()
                .Where(x => x.GrupoId == id)
                .ToArray();

            context.RemoveRange(grpXEmp);
        }

        /// <summary>
        /// Remove as associações por arranjo
        /// </summary>
        public void ExcluirGrupoXEmpresa(IEnumerable<GrupoEmpresaContabilSapParte> empresas)
        {
            var empresasRemovidasGrupo = context.Set<GrupoEmpresaContabilSapParte>()
                .Where(x => empresas.Select(y => y.EmpresaId).Contains(x.EmpresaId))
                .ToArray();

            if (empresasRemovidasGrupo != null)
            {
                context.Set<GrupoEmpresaContabilSapParte>().RemoveRange(empresasRemovidasGrupo);
            }
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}