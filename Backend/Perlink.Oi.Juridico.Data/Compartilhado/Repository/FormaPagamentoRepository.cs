using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class FormaPagamentoRepository : BaseCrudRepository<FormaPagamento, long>, IFormaPagamentoRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly ISequencialRepository sequencialRepository;

        public FormaPagamentoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            this.dbContext = context;
            sequencialRepository = new SequencialRepository(dbContext, user);
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await dbContext.FormaPagamentos
                    .AsNoTracking()
                    .OrderBy(x => x.DescricaoFormaPagamento)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.Id.ToString());
        }

        public async Task<IEnumerable<FormaPagamentoGridDTO>> GetFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros)
        {

            IQueryable<FormaPagamento> query = WhereFormaPagamento(filtros);
            var result = await query
             .Select(e => new FormaPagamentoGridDTO()
             {
                 Codigo = e.Id,
                 DescricaoFormaPagamento = e.DescricaoFormaPagamento,
                 Restrita = e.IndicaRestrita,
                 RequerBordero = e.IndicaBordero
             }).ToListAsync();


            var resultado = result.OrderBy(ct => ct.DescricaoFormaPagamento)
                                  .AsQueryable()
                                  .OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "DescricaoFormaPagamento")
                                  .Paginar(filtros.Pagina, filtros.Quantidade)
                                  .ToList();
            return resultado;
        }

        public async Task<int> GetTotalFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros)
        {
            IQueryable<FormaPagamento> query = WhereFormaPagamento(filtros);
            var resultado = await query.CountAsync();

            return resultado;
        }

        public async Task<IEnumerable<FormaPagamentoGridExportarDTO>> GetFormaPagamentoGridExportarManutencao(FormaPagamentoFiltroDTO filtros)
        {

            IQueryable<FormaPagamento> query = WhereFormaPagamento(filtros);
            var result = await query
                             .Select(e => new FormaPagamentoGridExportarDTO()
                             {
                                 Codigo = e.Id,
                                 DescricaoFormaPagamento = e.DescricaoFormaPagamento,
                                 Restrita = e.IndicaRestrita,
                                 RequerBordero = e.IndicaBordero
                             }).OrderBy(e => e.DescricaoFormaPagamento).ToListAsync();

            return result;
        }

        private IQueryable<FormaPagamento> WhereFormaPagamento(FormaPagamentoFiltroDTO filtros)
        {
            IQueryable<FormaPagamento> query = dbContext.FormaPagamentos;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.DescricaoFormaPagamento.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return query;
        }



        public async Task<FormaPagamento> CadastrarFormaPagamento(FormaPagamento formaPagamento)
        {
            await sequencialRepository.AtualizarSequence("FORMA_PAGAMENTO");
            formaPagamento.Id = await sequencialRepository.RecuperarSequencialAsync("FORMA_PAGAMENTO");

            await base.Inserir(formaPagamento);
            return formaPagamento;
        }
    }
}
