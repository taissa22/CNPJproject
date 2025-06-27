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
    public class BancoRepository : BaseCrudRepository<Banco, long>, IBancoRepository
    {
        private readonly JuridicoContext dbContext;

        public BancoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Banco>> RecuperarNomeBanco()
        {
            return await context.Set<Banco>()
                .OrderBy(p => p.NomeBanco)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BancoListaDTO>> RecuperarListaBancos()
        {
            return await context.Set<Banco>()
                                .Select(b => new BancoListaDTO()
                                {
                                    Id = b.Id,
                                    Descricao = b.NomeBanco
                                })
                                .OrderBy(p => p.Descricao)
                                .AsNoTracking()
                                .ToListAsync();
        }
    }
}
