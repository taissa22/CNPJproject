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
    public class TipoDocConsumidorQueryCreator : QueryCreator
    {
        public TipoDocConsumidorQueryCreator(IDatabaseContext context) : base(context)
        {
            Context = context;
        }

        public override IQueryable<TipoDocumentoCommandResult> GerarQuery(TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {
            var listaTipoDocumentoMigracao = Context.TiposDocumentos.Where(x => x.CodTipoProcesso == 9).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();

              Query = from a in Context.TiposDocumentos.AsNoTracking()
                    join ma in Context.TipoDocumentoMigracao on a.Id equals ma.CodTipoDocCivelConsumidor into LeftJoinMa
                    from ma in LeftJoinMa.DefaultIfEmpty()
                        //where a.CodTipoProcesso == 1
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
                        (int?)ma.CodTipoDocCivelEstrategico == null ? false : listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelEstrategico) != null ? listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelEstrategico).Ativo : false,
                        (int?)ma.CodTipoDocCivelEstrategico,
                        (int?)ma.CodTipoDocCivelEstrategico == null ? null : listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelEstrategico).Descricao
                    );

            return Sort(sort, ascending, pesquisa);
        }
        
    }
}
