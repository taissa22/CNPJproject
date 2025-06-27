using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TipoPrazo;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult
{
    public class TipoPrazoEstrategicoQueryCreator : QueryCreator
    {
        public TipoPrazoEstrategicoQueryCreator(IDatabaseContext context) : base(context)
        {
            ContextPrazo = context;
        }

        public override IQueryable<TipoPrazoCommandResult> GerarQuery(TipoPrazoSort sort, bool ascending, string pesquisa = null)
        {
            var listaTipoPrazoMigracao = ContextPrazo.TiposPrazos.Where(x => x.Eh_Civel_Consumidor == true).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();

            Query = from a in ContextPrazo.TiposPrazos.AsNoTracking()
                    join ma in ContextPrazo.MigracaoTipoPrazoEstrategico on a.Id equals ma.CodTipoPrazoCivelEstrat into LeftJoinMa
                    from ma in LeftJoinMa.DefaultIfEmpty()
                    select new TipoPrazoCommandResult(
                    a.Id,
                    a.Descricao,
                    a.Ativo,
                    (bool?)a.Eh_Civel_Consumidor,
                    (bool?)a.Eh_Trabalhista,
                    (bool?)a.Eh_Tributario_Administrativo,
                    (bool?)a.Eh_Tributario_Judicial,
                    (bool?)a.Eh_Juizado_Especial,
                    a.Eh_Civel_Estrategico,
                    a.Eh_Administrativo,
                    a.Eh_Criminal_Judicial,
                    a.Eh_Procon,
                    a.Eh_Pex_Juizado,
                    a.Eh_Pex_Consumidor,
                    a.Eh_Criminal_Administrativo,
                    a.Eh_Documento,
                    (bool?)a.Eh_Servico,
                    (int?)ma.CodTipoPrazoCivelCons == null ? false : listaTipoPrazoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoPrazoCivelCons) != null ? listaTipoPrazoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoPrazoCivelCons).Ativo : false,
                    (int?)ma.CodTipoPrazoCivelCons,
                    (int?)ma.CodTipoPrazoCivelCons == null ? null : listaTipoPrazoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoPrazoCivelCons).Descricao
);

            return Sort(sort, ascending, pesquisa);
        }
    }
}
