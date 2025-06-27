using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Services;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501InfoCrIrrfService
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialF2501InfoCrIrrfService(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }



        #region Funções Auxiliares

        public async Task<bool> ExisteInfoCrIrrfPorIdAsync(long codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf, ct);
        }

        public async Task<EsF2501Infocrirrf?> RetornaInfoCrIrrfPorIdAsync(int codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf, ct);
        }

        public async Task<EsF2501Infocrirrf?> RetornaInfoCrIrrfEditavelPorIdAsync(int codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infocrirrf.FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf, ct);
        }

        #endregion
    }
}
