using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class DBContextService
    {
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;

        public DBContextService(ESocialDbContext eSocialDbContext, ParametroJuridicoContext parametroJuridicoDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
        }

        public async Task SalvaAlteracoesAsync(CancellationToken ct)
        {
            await _eSocialDbContext.SaveChangesAsync(ct);
        }

        public async Task SalvaAlteracoesRegistraLogAsync(string usuario, CancellationToken ct)
        {
            await _eSocialDbContext.SaveChangesAsync(usuario, true, ct);
        }

    }
}
