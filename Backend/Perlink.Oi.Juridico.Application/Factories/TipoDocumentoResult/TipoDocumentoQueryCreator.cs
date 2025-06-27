using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult
{
    public class TipoDocumentoQueryCreator : QueryCreator
    {
        public TipoDocumentoQueryCreator(IDatabaseContext context) : base(context)
        {
            Context = context;
        }

        public override IQueryable<TipoDocumentoCommandResult> GerarQuery(TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {
            Query = Context.TiposDocumentos.AsNoTracking()
                    .Select(a => new TipoDocumentoCommandResult(a.Id,
                                                            a.Descricao,
                                                            a.MarcadoCriacaoProcesso,
                                                            a.Ativo,
                                                            a.CodTipoProcesso,
                                                            a.CodTipoPrazo,
                                                            a.TipoPrazo,
                                                            a.IndRequerDatAudiencia,
                                                            a.IndPrioritarioFila,
                                                            a.IndDocumentoProtocolo,
                                                            a.IndDocumentoApuracao,
                                                            a.IndEnviarAppPreposto,
                                                            false,
                                                            null,
                                                            null)); 

            return Sort(sort,ascending,pesquisa);
        }
    }
}
