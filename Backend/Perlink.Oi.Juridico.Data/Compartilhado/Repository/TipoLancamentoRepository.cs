using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class TipoLancamentoRepository : BaseCrudRepository<TipoLancamento, long>, ITipoLancamentoRepository
    {
        public TipoLancamentoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user) { }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            // TODO: Implementar dropdown
            return await context.Set<TipoLancamento>()
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.Descricao);
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarTodosSAP(long codigoTipoProcesso)
        {
            // TODO: montar enum p tipo lançamento
            var query = context.Set<TipoLancamento>()
                                     .AsNoTracking();
            IQueryable<TipoLancamento> queryable = query;
            if (codigoTipoProcesso == (int)TipoProcessoEnum.CivelConsumidor ||
                codigoTipoProcesso == (int)TipoProcessoEnum.CivelEstrategico ||
                codigoTipoProcesso == (int)TipoProcessoEnum.Trabalhista ||
                codigoTipoProcesso == (int)TipoProcessoEnum.Pex ||
                codigoTipoProcesso == (int)TipoProcessoEnum.TributarioJudicial ||
                codigoTipoProcesso == (int)TipoProcessoEnum.TributarioAdministrativo)
            {
                queryable = query.Where(tl => tl.Id != 5);
            }
            else if (codigoTipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial)
            {
                queryable = query.Where(tl => tl.Id != 5 && tl.Id != 4);
            }
            else if (codigoTipoProcesso == (int)TipoProcessoEnum.Procon)
            {
                queryable = query.Where(tl => tl.Id != 5 && tl.Id != 4 && tl.Id != 1);
            }
            else if (codigoTipoProcesso == (int)TipoProcessoEnum.Administrativo)
            {
                queryable = query.Where(tl => tl.Id != 2 && tl.Id != 4 && tl.Id != 1);
            }
            else if(codigoTipoProcesso != 0)
            {
                throw new ArgumentException("Tipo de processo inválido, não é possível definir os tipos de lançamento.");
            }

            return await queryable.Select(tl => new ComboboxDTO()
            {
                Id = tl.Id,
                Descricao = tl.Descricao
            }).ToListAsync();
        }

        public async Task<IEnumerable<TipoLancamento>> RecuperarTipoLancamentoParaFiltroLote()
        {
            var tipoLancamentos = await context.Set<TipoLancamento>()
           .AsNoTracking()
          .Select(dto => new TipoLancamento()
          {
              Id = dto.Id,
              Descricao = dto.Descricao,
          }).OrderBy(x => x.Descricao).ToListAsync();
            return tipoLancamentos;
        }
    }
}