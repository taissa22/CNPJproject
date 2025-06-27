using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class BBTribunaisRepository : BaseCrudRepository<BBTribunais, long>, IBBTribunaisRepository
    {
        private readonly JuridicoContext dbcontext;
        public BBTribunaisRepository(JuridicoContext dbcontext, IAuthenticatedUser user) : base(dbcontext, user)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<ICollection<BBTribunais>> ObterPorFiltroPaginado(FiltrosDTO filtros) {
            IQueryable<BBTribunais> query = dbcontext.BBTribunais;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.Descricao.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return await query.OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao")
            .Paginar(filtros.Pagina, filtros.Quantidade).Select(obj => new BBTribunais() {
                Id = obj.Id,
                Descricao = obj.Descricao,
                BBOrgaos = obj.BBOrgaos,
                CodigoBB = obj.CodigoBB,
                IndicadorInstancia = obj.IndicadorInstancia
            }).ToListAsync();

        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros) {
            IQueryable<BBTribunais> query = dbcontext.BBTribunais;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.Descricao.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return await query.CountAsync();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerificarDuplicidadeTribunalnalBB(BBTribunais entidade)
        {
            var ret = await dbcontext.BBTribunais
               .AsNoTracking()
               .AnyAsync(cc => cc.CodigoBB == entidade.CodigoBB &&
                                       cc.Id != entidade.Id);
            return ret;
        }
    }
}
