using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class EstadoRepository : BaseCrudRepository<Estado, string>, IEstadoRepository
    {
        private readonly JuridicoContext dbContext;

        public EstadoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<IList<Estado>> ListarEstadosSemGrupo()
        {
            var resultado = await dbContext.Estados.FromSql("SELECT * FROM JUR.ESTADO E WHERE E.COD_ESTADO NOT IN (SELECT GEE.COD_ESTADO FROM JUR.GRUPO_ESTADO_ESTADO GEE)")                            
                            .OrderBy(p => p.NomeEstado)
                            .ThenBy(x => x.Id)
                            .ToListAsync();

            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EstadoDTO>> RecuperarListaEstados()
        {
            var result = await dbContext.Set<Estado>()
                .Select(e => new EstadoDTO()
                {
                    Id = e.Id,
                    Descricao = string.Format("{0} - {1}", e.Id, e.NomeEstado)
                }).OrderBy(e => e.Id).ToListAsync();

            return result;
        }
    }
}
