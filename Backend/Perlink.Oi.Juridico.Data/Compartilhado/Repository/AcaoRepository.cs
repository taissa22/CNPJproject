using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository {
    public class AcaoRepository : BaseCrudRepository<Acao, long>, IAcaoRepository
    {
        private readonly JuridicoContext dbContext;

        public AcaoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            this.dbContext = context;
        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteBBNaturezaAcaoAssociadoAcao(long id)
        {
            return await dbContext.Acoes.AsNoTracking()
                        .AnyAsync(a => a.IdBBNaturezasAcoes == id);
        }

    }
}
