using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class CentroCustoRepository : BaseCrudRepository<CentroCusto, long>, ICentroCustoRepository
    {
        private readonly JuridicoContext dbContext;

        public CentroCustoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        private int OrderByNumberFirst(string x)
        {
            int i = 0;
            return int.TryParse(x, out i) ? i : int.MaxValue;
        }

        public async Task<IEnumerable<CentroCusto>> RecuperarCentroCustoParaFiltroLote()
        {
            var fornecedor =  dbContext.CentroCusto
                    .AsNoTracking()
                    .OrderBy(s => OrderByNumberFirst(s.Descricao))
                    .ThenBy(s => s.Descricao)
                    .Select(dto => new CentroCusto()
                    {
                        Id = dto.Id,
                        Descricao = dto.IndicaAtivo ? dto.Descricao : string.Format("{0} (Inativo)",dto.Descricao),
                        IndicaAtivo = dto.IndicaAtivo
                    }
                    ).OrderBy(x => x.Descricao);
            var teste = fornecedor.ToSql();
            return await fornecedor.ToListAsync();
        }

        #region Exibir pesquisa Grid

        public async Task<IEnumerable<CentroCustoResultadoDTO>> ConsultarCentrosCustos(CentroCustoFiltroDTO filtros)
        {

            IQueryable<CentroCusto> query = WhereCentroCusto(filtros);
            var result = await query

                .Select(obj => new CentroCustoResultadoDTO() 
                {
                    Codigo = obj.Id,
                    DescricaoCentroCusto = obj.Descricao,
                    CentroCustoSAP = obj.CodigoCentroCustoSAP,
                    IndicaAtivo = obj.IndicaAtivo
                }).ToListAsync();

            return result.OrderBy(ct => ct.DescricaoCentroCusto).AsQueryable()
                         .OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao")
                         .Paginar(filtros.Pagina, filtros.Quantidade).ToList();
        }
        public async Task<ICollection<CentroCustoResultadoDTO>> ExportarCentrosCustos(CentroCustoFiltroDTO filtros)
        {
            IQueryable<CentroCusto> query = WhereCentroCusto(filtros);
            var result = await query

                               .Select(obj => new CentroCustoResultadoDTO() {
                                   Codigo = obj.Id,
                                   DescricaoCentroCusto = obj.Descricao,
                                   CentroCustoSAP = obj.CodigoCentroCustoSAP,
                                   IndicaAtivo = obj.IndicaAtivo
                               }).OrderBy(or => or.DescricaoCentroCusto).ToListAsync();
            return result;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO filtroDTO)
        {
            return await WhereCentroCusto(filtroDTO).CountAsync();
        }

        private IQueryable<CentroCusto> WhereCentroCusto(CentroCustoFiltroDTO filtros)
        {
            IQueryable<CentroCusto> query = dbContext.CentroCusto;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.Descricao.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return query;
        }

        #endregion Exibir pesquisa Grid

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public Task CadastrarCentroCusto(CentroCusto entidade) => base.Inserir(entidade);

        public async Task<bool> VerificarDuplicidadeDescricaoCentroCusto(CentroCusto centroCusto)
        {
            var ret = await dbContext.CentroCusto
                .AsNoTracking()
                .AnyAsync(cc => cc.Descricao == centroCusto.Descricao &&
                                        cc.Id != centroCusto.Id);
            return ret;
        }

        public async Task<bool> VerificarDuplicidadeCentroCustoSAP(CentroCusto centroCusto)
        {
            var ret = await dbContext.CentroCusto
                .AsNoTracking()
                .AnyAsync(cc => cc.CodigoCentroCustoSAP == centroCusto.CodigoCentroCustoSAP &&
                                        cc.Id != centroCusto.Id);
            return ret;
        }
    }
}
