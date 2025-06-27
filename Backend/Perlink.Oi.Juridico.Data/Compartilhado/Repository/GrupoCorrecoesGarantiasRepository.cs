using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class GrupoCorrecoesGarantiasRepository : BaseCrudRepository<GrupoCorrecaoGarantia, long>, IGrupoCorrecoesGarantiasRepository
    {
        private readonly JuridicoContext dbContext;
        public GrupoCorrecoesGarantiasRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarComboboxGrupoCorrecao(long tipoProcesso)
        {
            return await dbContext.Set<GrupoCorrecaoGarantia>()
                          .AsNoTracking()
                          .Where(gcc => gcc.CodigoTipoProcesso == tipoProcesso)
                          .Select(gcc => new ComboboxDTO()
                          {
                              Id = gcc.Id,
                              Descricao = gcc.Descricao
                          })
                          .ToListAsync();
        }
    }
}
