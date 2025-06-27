using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class GruposLotesJuizadosRepository : BaseCrudRepository<GruposLotesJuizados, long>, IGruposLotesJuizadosRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly SequencialRepository sequencialRepository;

        public GruposLotesJuizadosRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            sequencialRepository = new SequencialRepository(dbContext, user);
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros)
        {
            IQueryable<GruposLotesJuizados> query = dbContext.GruposLotesJuizados;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.Descricao.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return await query.Select(obj => new GruposLotesJuizados()).CountAsync();
        }

        public async Task<long> ObterUltimoId()
        {
            await sequencialRepository.AtualizarSequence("GRUPOS_LOTES_JUIZADOS");
            return await sequencialRepository.RecuperarSequencialAsync("GRUPOS_LOTES_JUIZADOS");
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GruposLotesJuizados>> RecuperarGrupoLoteJuizadoPorFiltro(FiltrosDTO filtros)
        {
            IQueryable<GruposLotesJuizados> query = dbContext.GruposLotesJuizados;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.Descricao.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return await query.Select(obj => new GruposLotesJuizados() { Descricao = obj.Descricao, Id = obj.Id })
                              .OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao")
                              .Paginar(filtros.Pagina, filtros.Quantidade).ToListAsync();
        }
    }
}
