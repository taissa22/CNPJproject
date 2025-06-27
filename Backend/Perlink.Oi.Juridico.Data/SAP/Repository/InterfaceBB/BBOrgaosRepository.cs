using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.InterfaceBB {
    public class BBOrgaosRepository : BaseCrudRepository<BBOrgaos, long>, IBBOrgaosRepository {
        private readonly JuridicoContext dbContext;

        public BBOrgaosRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user) {
            this.dbContext = dbContext;
        }

        public async Task<bool> OrgaoBBAssociadocomTribunais(long id)
        {
            var resultado = await dbContext.BBOrgaos
                .AnyAsync(l => l.CodigoBBTribunal == id);
            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }

        private IQueryable<BBOrgaos> QueryBBOrgao(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            IQueryable<BBOrgaos> query = dbContext.BBOrgaos;

            if (consultaBBOrgaosDTO.CodigoBBComarca.HasValue)
                query = query.Where(q => q.CodigoBBComarca == consultaBBOrgaosDTO.CodigoBBComarca.Value);
            if (consultaBBOrgaosDTO.CodigoBBTribunal.HasValue)
                query = query.Where(q => q.CodigoBBTribunal == consultaBBOrgaosDTO.CodigoBBTribunal.Value);
            if (!string.IsNullOrEmpty(consultaBBOrgaosDTO.NomeBBOrgao))
                query = query.Where(q => q.Nome.Contains(consultaBBOrgaosDTO.NomeBBOrgao));
            if (consultaBBOrgaosDTO.Codigo.HasValue)
                query = query.Where(q => q.Codigo == consultaBBOrgaosDTO.Codigo.Value);

            return query;
        }

        public async Task<ICollection<BBOrgaosResultadoDTO>> RecuperarPorFiltros(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            IQueryable<BBOrgaos> query = QueryBBOrgao(consultaBBOrgaosDTO);

            return await query
                .Select(q => new BBOrgaosResultadoDTO() {
                    Id = q.Id,
                    Codigo = q.Codigo,
                    Nome = q.Nome,
                    NomeBBComarca = string.Format("{0} - {1} ({2})", q.BBComarca.CodigoEstado, q.BBComarca.Descricao, q.BBComarca.Id.ToString("000000000")) ,
                    NomeBBTribunal = q.BBTribunais.Descricao,
                    CodigoBBComarca = q.BBComarca.Id,
                    CodigoBBTribunal = q.BBTribunais.Id
                })
                .OrdenarPorPropriedade(consultaBBOrgaosDTO.Ascendente, consultaBBOrgaosDTO.Ordenacao, "NomeBBComarca")
                .Paginar(consultaBBOrgaosDTO.Pagina, consultaBBOrgaosDTO.Quantidade).ToListAsync();
        }

        public async Task<int> RecuperarTotalRegistros(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            return await QueryBBOrgao(consultaBBOrgaosDTO).CountAsync();
        }

        public async Task<bool> ExisteBBComarcaAssociadoBBOrgao(long codigo) {
            return await dbContext.BBOrgaos
                .AsNoTracking()
                .AnyAsync(bbOrg => bbOrg.CodigoBBComarca == codigo);
        }

        public async Task<bool> CodgioOrgaoBBExiste(BBOrgaos entidade) {
            return await dbContext.BBOrgaos
                .AsNoTracking()
                .AnyAsync(bbOrg => bbOrg.Codigo == entidade.Codigo && bbOrg.Id != entidade.Id);
        }

        public async Task<BBOrgaos> RecuperarPorCodigoBB(long codigo)
        {
            return await dbContext.BBOrgaos.AsNoTracking()
                        .FirstOrDefaultAsync(bb => bb.Codigo == codigo);
        }
    }
}
