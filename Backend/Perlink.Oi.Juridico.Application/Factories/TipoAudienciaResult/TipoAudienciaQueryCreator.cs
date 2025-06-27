using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult
{
    public class TipoAudienciaQueryCreator : QueryCreator
    {
        public TipoAudienciaQueryCreator(IDatabaseContext context) : base(context)
        {
            ContextAudiencia = context;
        }

        public override IQueryable<TipoAudienciaCommandResult> GerarQuery(TipoAudienciaSort sort, bool ascending, string pesquisa = null)
        {
            Query = ContextAudiencia.TipoAudiencias.AsNoTracking()
                    .Select(a => new TipoAudienciaCommandResult(a.Id,
                                                            a.Descricao,
                                                            a.Sigla,
                                                            a.Ativo,
                                                            a.Eh_CivelConsumidor,
                                                            a.Eh_CivelEstrategico,
                                                            a.Eh_Trabalhista,
                                                            a.Eh_TrabalhistaAdministrativo,
                                                            a.Eh_TributarioAdministrativo,
                                                            a.Eh_TributarioJudicial,
                                                            a.Eh_JuizadoEspecial,
                                                            a.Eh_Administrativo,
                                                            a.Eh_CivelAdministrativo,
                                                            a.Eh_CriminalJudicial,
                                                            a.Eh_CriminalAdministrativo,
                                                            a.Eh_Procon,
                                                            a.Eh_Pex,
                                                            false,
                                                            null,
                                                            null,
                                                            a.LinkVirtual));

            return Sort(sort, ascending, pesquisa);
        }
    }
}