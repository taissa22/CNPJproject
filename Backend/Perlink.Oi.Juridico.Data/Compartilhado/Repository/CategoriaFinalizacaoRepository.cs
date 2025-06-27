using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class CategoriaFinalizacaoRepository : BaseCrudRepository<CategoriaFinalizacao, long>, ICategoriaFinalizacaoRepository
    {
        private readonly JuridicoContext dbContext;

        public CategoriaFinalizacaoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExisteCategoriaFinalizacao(long codCategoriaPagamento)
        {
            var resultado = await context.Set<CategoriaFinalizacao>()
                .AnyAsync(cf => cf.Id == codCategoriaPagamento);
            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}
