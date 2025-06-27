using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.GrupoDeEstados.Repository
{
    public class GrupoEstadosRepository : BaseCrudRepository<Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity.GrupoEstados, long>, IGrupoEstadosRepository
    {
        public GrupoEstadosRepository(JuridicoContext context, IAuthenticatedUser user)
            : base(context, user)
        {

        }

        public void ExcluirGrupoEstado(long id)
        {
            var grpXEmp = context.Set<GrupoEstados>()
                .Where(x => x.Id == id)
                .ToArray();

            context.RemoveRange(grpXEmp);
        }

        public void ExcluirGrupoEstado(IEnumerable<GrupoEstados> listaEntity)
        {
            var empresasRemovidasGrupo = context.Set<GrupoEstados>()
                .Where(x => listaEntity.Select(y => y.EstadoId).Contains(x.EstadoId))
                .ToArray();

            if (empresasRemovidasGrupo != null)
            {
                context.Set<GrupoEstados>().RemoveRange(empresasRemovidasGrupo);
            }
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}
