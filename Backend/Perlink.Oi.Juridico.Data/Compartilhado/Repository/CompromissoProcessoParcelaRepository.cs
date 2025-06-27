using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class CompromissoProcessoParcelaRepository : BaseCrudRepository<CompromissoProcessoParcela, long>, ICompromissoProcessoParcelaRepository
    {
        private readonly JuridicoContext dbContext;
        public CompromissoProcessoParcelaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<DadosCompromissoEstornoDTO> ObterDadosCompromissoParaEstorno(long codigoProcesso, long codigoLancamento)
        {
            return await dbContext.Set<CompromissoProcessoParcela>()
                .Include(cp => cp.CompromissoProcesso)
                .ThenInclude(cp => cp.CompromissoProcessoCredores)
                .Where(cp => cp.CodigoProcesso == codigoProcesso && cp.CodigoLancamento == codigoLancamento)
                .Select(cp => new DadosCompromissoEstornoDTO()
                {
                    ValorCompromisso = cp.CompromissoProcesso.TotalCompromisso,
                    QuantidadeCredores = cp.CompromissoProcesso.CompromissoProcessoCredores.Count(),
                    CodigoCompromisso = cp.Id,
                    CodigoParcela = cp.CodigoParcela,
                    ValorParcela = cp.ValorParcela
                }).FirstOrDefaultAsync();
        }

        public async Task<CompromissoProcessoParcela> ObterCompromissoParcela(long codigoProcesso, long codigoCompromisso, long codigoParcela)
        {
            return await dbContext.Set<CompromissoProcessoParcela>()
                .Where(cpp => cpp.CodigoProcesso == codigoProcesso
                              && cpp.CodigoParcela == codigoParcela
                              && cpp.Id == codigoCompromisso)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<CompromissoProcessoParcela>> ObterParcelasCompromissoProcesso(long codigoProcesso, long codigoCompromisso)
        {
            return await dbContext.Set<CompromissoProcessoParcela>()
                .Where(cpp => cpp.Id == codigoCompromisso && cpp.CodigoProcesso == codigoProcesso)
                .ToListAsync();
        }

        public long ObterProximoCodigoParcela(long codigoProcesso, long codigoCompromisso)
        {
            var codigoParcela = dbContext.Set<CompromissoProcessoParcela>()
                .Where(cpp => cpp.Id == codigoCompromisso && cpp.CodigoProcesso == codigoProcesso)
                .Max(i => i.CodigoParcela);

            return codigoParcela + 1;
        }

        public List<CompromissoProcessoParcela> ObterListaParcelasCompromissoProcessoReordenacao(long codigoProcesso, long codigoCompromisso)
        {
            return dbContext.Set<CompromissoProcessoParcela>()
                .Where(cpp => cpp.Id == codigoCompromisso && cpp.CodigoProcesso == codigoProcesso)
                .OrderBy(cpp => cpp.DataVencimento)
                .ToList();
        }
        public List<CompromissoProcessoParcela> ObterListaParcelasCompromissoProcesso(long codigoProcesso, long codigoCompromisso)
        {
            return dbContext.Set<CompromissoProcessoParcela>()
                .Where(cpp => cpp.Id == codigoCompromisso && cpp.CodigoProcesso == codigoProcesso)
                .AsNoTracking()
                .ToList();
        }
    }
}
