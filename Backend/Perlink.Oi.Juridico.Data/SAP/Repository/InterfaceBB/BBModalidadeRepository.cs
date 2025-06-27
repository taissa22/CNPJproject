using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.InterfaceBB
{
    public class BBModalidadeRepository : BaseCrudRepository<BBModalidade, long>, IBBModalidadeRepository
    {
        private readonly JuridicoContext dbContext;

        public BBModalidadeRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }


        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
        public Task<bool> CodigoBBJaExiste(BBModalidade obj)
        {
            return dbContext.BBModalidades.AsNoTracking()
                        .AnyAsync(bb => bb.CodigoBB == obj.CodigoBB && bb.Id != obj.Id);
        }

        public async Task<IEnumerable<BBModalidade>> recuperarModalidadeBB(BBModalidadeFiltroDTO filtro) {
            IQueryable<BBModalidade> query = Where(filtro);
            var result = await query
                .Select(obj => new BBModalidade() {
                    Id = obj.Id,
                    CodigoBB = obj.CodigoBB,
                    Descricao = obj.Descricao
                }).OrdenarPorPropriedade(filtro.Ascendente, filtro.Ordenacao, "Descricao")
              .Paginar(filtro.Pagina, filtro.Quantidade)
              .ToListAsync();

            return result;
        }

        public async Task<ICollection<BBModalidade>> exportarModalidadeBB(BBModalidadeFiltroDTO filtro)
        {
            IQueryable<BBModalidade> query = Where(filtro);
            var result = await query
                .Select(obj => new BBModalidade()
                {
                    Id = obj.Id,
                    CodigoBB = obj.CodigoBB,
                    Descricao = obj.Descricao
                }).ToListAsync();
          

            return result;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(BBModalidadeFiltroDTO filtroDTO) {
            IQueryable<BBModalidade> query = Where(filtroDTO);
            var resultado = await query.CountAsync();

            return resultado;

        }

        private IQueryable<BBModalidade> Where(BBModalidadeFiltroDTO filtros) {
    IQueryable<BBModalidade> query = dbContext.BBModalidades;

    if (!string.IsNullOrEmpty(filtros.Descricao))
        query = query.Where(q => q.Descricao.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

    return query;
}
    }
}

