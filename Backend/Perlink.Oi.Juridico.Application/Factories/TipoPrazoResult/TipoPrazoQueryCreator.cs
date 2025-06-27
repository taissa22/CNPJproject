using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TipoPrazo;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult
{
    public class TipoPrazoQueryCreator : QueryCreator
    {
        public TipoPrazoQueryCreator(IDatabaseContext context) : base(context)
        {
            ContextPrazo = context;
        }

        public override IQueryable<TipoPrazoCommandResult> GerarQuery(TipoPrazoSort sort, bool ascending, string pesquisa = null)
        {
            Query = ContextPrazo.TiposPrazos.AsNoTracking()
                    .Select(a => new TipoPrazoCommandResult(a.Id,
                                                            a.Descricao,                                                            
                                                            a.Ativo,
                                                            (bool)a.Eh_Civel_Consumidor,
                                                            (bool)a.Eh_Trabalhista,
                                                            (bool)a.Eh_Tributario_Administrativo,
                                                            (bool)a.Eh_Tributario_Judicial,
                                                            (bool)a.Eh_Juizado_Especial,
                                                            a.Eh_Civel_Estrategico,
                                                            a.Eh_Administrativo,
                                                            a.Eh_Criminal_Judicial,
                                                            a.Eh_Procon,
                                                            a.Eh_Pex_Juizado,
                                                            a.Eh_Pex_Consumidor,
                                                            a.Eh_Criminal_Administrativo,
                                                            a.Eh_Documento,
                                                            (bool)a.Eh_Servico,
                                                            false,
                                                            null,
                                                            null));

            return Sort(sort, ascending, pesquisa);
        }
    }
}