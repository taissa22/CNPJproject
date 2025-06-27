using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
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
    public class BBComarcaRepository : BaseCrudRepository<BBComarca, long>, IBBComarcaRepository
    {
        private readonly JuridicoContext dbContext;

        public BBComarcaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UFIsValid(string uf)
        {
            return await dbContext.Estados
                         .AsNoTracking()
                         .AnyAsync(e => e.Id.ToUpper().Equals(uf.ToUpper()));
        }

        public async Task<bool> CodigoBBJaExiste(BBComarca bbComarca)
        {
            return await dbContext.BBComarcas
              .AsNoTracking()
              .AnyAsync(bb => bb.CodigoBB == bbComarca.CodigoBB && bb.Id != bbComarca.Id);
        }

        public async Task<BBComarca> RecuperarPorCodigoBB(long codigo)
        {
            return await dbContext.BBComarcas.AsNoTracking()
                        .FirstOrDefaultAsync(bb => bb.CodigoBB == codigo);
        }

        public async Task<ICollection<BBComarca>> ConsultarBBComarca(FiltrosDTO filtroDTO)
        {
            IQueryable<BBComarca> query = whereBBComarca(filtroDTO);

            var resultado = await query.OrdenarPorPropriedade(filtroDTO.Ascendente, filtroDTO.Ordenacao, "Descricao")
                        .Paginar(filtroDTO.Pagina, filtroDTO.Quantidade)
                        .ToListAsync();

            return resultado;
        }
        public async Task<ICollection<BBComarca>> ExportarBBComarca(FiltrosDTO filtroDTO)
        {
            IQueryable<BBComarca> query = whereBBComarca(filtroDTO);

            return await query.ToListAsync();
        }

        public async Task<int> TotalBBComarca(FiltrosDTO filtroDTO)
        {
            IQueryable<BBComarca> query = whereBBComarca(filtroDTO);

            return await query.CountAsync();
        }

        private IQueryable<BBComarca> whereBBComarca(FiltrosDTO filtroDTO)
        {
            IQueryable<BBComarca> query = dbContext.BBComarcas;
            if (!string.IsNullOrEmpty(filtroDTO.Descricao))
                query = query.Where(bb => bb.Descricao.Trim().ToUpper().Contains(filtroDTO.Descricao.Trim().ToUpper()));

            return query;
        }

    }
}