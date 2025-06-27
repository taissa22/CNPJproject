using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult
{
    public class TipoDocEstrategicoQueryCreator : QueryCreator
    {
        public TipoDocEstrategicoQueryCreator(IDatabaseContext context) : base(context)
        {
            Context = context;
        }

        public override IQueryable<TipoDocumentoCommandResult> GerarQuery(TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {
            var listaTipoDocumentoMigracao = Context.TiposDocumentos.Where(x => x.CodTipoProcesso == 1).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();

              Query = from a in Context.TiposDocumentos.AsNoTracking()
                    join ma in Context.TipoDocumentoMigracaoEstrategico on a.Id equals ma.CodTipoDocCivelEstrategico into LeftJoinMa
                    from ma in LeftJoinMa.DefaultIfEmpty()
                      select new TipoDocumentoCommandResult(
                      a.Id,
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
                      (int?)ma.CodTipoDocCivelConsumidor == null ? false : listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelConsumidor) != null ? listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelConsumidor).Ativo : false,
                      (int?)ma.CodTipoDocCivelConsumidor,
                      (int?)ma.CodTipoDocCivelConsumidor == null ? null : listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelConsumidor).Descricao
  );

            return Sort(sort, ascending, pesquisa);
        }
        
    }
}
