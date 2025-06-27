using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class StatusPagamentoRepository : BaseCrudRepository<StatusPagamento, long>, IStatusPagamentoRepository
    {
        private readonly JuridicoContext Context;

        public StatusPagamentoRepository(JuridicoContext Context, IAuthenticatedUser user) : base(Context, user)
        {
            this.Context = Context;
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await Context.Set<StatusPagamento>()
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                  x => x.Descricao);
        }

        public async Task<IEnumerable<StatusPagamento>> RecuperarStatusPagamentoParaFiltroLote()
        {
            var statusPagamentos = await context.Set<StatusPagamento>()
                .Where(st => st.Id != 23)
               .AsNoTracking()
              .Select(dto => new StatusPagamento()
              {
                  Id = dto.Id,
                  Descricao = dto.Descricao,
              }).OrderBy(x => x.Descricao).ToListAsync();
            return statusPagamentos;
        }
    }
}