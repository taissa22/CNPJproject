using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult
{
    public class TipoAudienciaConsumidorQueryCreator : QueryCreator
    {
        public TipoAudienciaConsumidorQueryCreator(IDatabaseContext context) : base(context)
        {
            ContextAudiencia = context;
        }

        public override IQueryable<TipoAudienciaCommandResult> GerarQuery(TipoAudienciaSort sort, bool ascending, string pesquisa = null)
        {
            var listaTipoAudienciaMigracao = ContextAudiencia.TipoAudiencias.Where(x => x.Eh_CivelEstrategico == true).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();

            Query = from a in ContextAudiencia.TipoAudiencias.AsNoTracking()
                    join ma in ContextAudiencia.MigracaoTipoAudienciaConsumidor on a.Id equals ma.CodTipoAudCivel into LeftJoinMa
                    from ma in LeftJoinMa.DefaultIfEmpty()
                    select new TipoAudienciaCommandResult(
                    a.Id,
                    a.Descricao,
                    a.Sigla,
                    a.Ativo,
                    a.Eh_CivelConsumidor,
                    a.Eh_CivelEstrategico,
                    a.Eh_Trabalhista,
                    a.Eh_TrabalhistaAdministrativo,
                    a.Eh_TributarioAdministrativo,
                    a.Eh_TributarioJudicial,
                    a.Eh_Administrativo,
                    a.Eh_CivelAdministrativo,
                    a.Eh_CriminalAdministrativo,
                    a.Eh_CriminalJudicial,
                    a.Eh_JuizadoEspecial,
                    a.Eh_Procon,
                    a.Eh_Pex,
                    (int?)ma.CodTipoAudCivelEstrat == null ? false : listaTipoAudienciaMigracao.FirstOrDefault(z => z.Id == ma.CodTipoAudCivelEstrat) != null ? listaTipoAudienciaMigracao.FirstOrDefault(z => z.Id == ma.CodTipoAudCivelEstrat).Ativo : false,
                    (int?)ma.CodTipoAudCivelEstrat,
                    (int?)ma.CodTipoAudCivelEstrat == null ? null : listaTipoAudienciaMigracao.FirstOrDefault(z => z.Id == ma.CodTipoAudCivelEstrat).Descricao,
                    a.LinkVirtual );

            return Sort(sort, ascending, pesquisa);
        }
    }
}
