using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501IdeAdvService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501IdeAdvService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoIdeAdv(EsF2501IdeAdvRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, irrf).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoIdeAdv(EsF2501IdeAdvRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Ideadv ideAdv, EsF2501Infocrirrf irrf)
        {
            if (ideAdv == null || irrf == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da remuneração.");
            }

            var requestDTO = new EsF2501IdeAdvRequestDTO()
            {
                IdeadvTpinsc = ideAdv.IdeadvTpinsc,
                IdeadvNrinsc = ideAdv.IdeadvNrinsc,
                IdeadvVlradv = ideAdv.IdeadvVlradv

            };

            return ValidaErrosGlobais(requestDTO, irrf);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501IdeAdvRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosGlobais = new List<string>();

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501IdeAdvRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheIdeAdv(ref EsF2501Ideadv ideAdv, EsF2501IdeAdvRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoInfocrirrf = null)
        {
            if (codigoInfocrirrf.HasValue)
            {
                ideAdv.IdEsF2501Infocrirrf = codigoInfocrirrf.Value;
            }
            var apenasDigitos = new Regex(@"[^\d]");

            ideAdv.IdeadvTpinsc = requestDTO.IdeadvTpinsc;
            ideAdv.IdeadvNrinsc = apenasDigitos.Replace(requestDTO.IdeadvNrinsc!, "");
            ideAdv.IdeadvVlradv = requestDTO.IdeadvVlradv;

            ideAdv.LogCodUsuario = usuario!.Identity!.Name;
            ideAdv.LogDataOperacao = DateTime.Now;
        }

        public EsF2501IdeAdvDTO PreencheIdeAdvDTO(ref EsF2501Ideadv? ideAdv)
        {
            return new EsF2501IdeAdvDTO()
            {
                IdEsF2501Ideadv = ideAdv!.IdEsF2501Ideadv,
                IdEsF2501Infocrirrf = ideAdv!.IdEsF2501Infocrirrf,
                LogCodUsuario = ideAdv!.LogCodUsuario,
                LogDataOperacao = ideAdv!.LogDataOperacao,
                IdeadvTpinsc = ideAdv!.IdeadvTpinsc,
                IdeadvNrinsc = ideAdv!.IdeadvNrinsc,
                IdeadvVlradv = ideAdv!.IdeadvVlradv
            };
        }

        public IQueryable<EsF2501IdeAdvDTO> RecuperaListaIdeAdv(long codigoInfocrirrf)
        {
            return from ideAdv in _eSocialDbContext.EsF2501Ideadv.AsNoTracking()
                   where ideAdv.IdEsF2501Infocrirrf == codigoInfocrirrf
                   select new EsF2501IdeAdvDTO()
                   {
                       IdEsF2501Ideadv = ideAdv!.IdEsF2501Ideadv,
                       IdEsF2501Infocrirrf = ideAdv!.IdEsF2501Infocrirrf,
                       LogCodUsuario = ideAdv!.LogCodUsuario,
                       LogDataOperacao = ideAdv!.LogDataOperacao,
                       IdeadvTpinsc = ideAdv!.IdeadvTpinsc,
                       IdeadvNrinsc = ideAdv!.IdeadvNrinsc,
                       IdeadvVlradv = ideAdv!.IdeadvVlradv
                   };
        }

        public async Task<bool> ExisteIdeAdvPorIdAsync(int codigoInfoCrIrrf, long codigoIdeAdv, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Ideadv.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Ideadv == codigoIdeAdv, ct);
        }

        public async Task<EsF2501Ideadv?> RetornaIdeAdvPorIdAsync(int codigoInfoCrIrrf, long codigoIdeAdv, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Ideadv.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Ideadv == codigoIdeAdv, ct);
        }

        public async Task<EsF2501Ideadv?> RetornaIdeAdvEditavelPorIdAsync(int codigoInfoCrIrrf, long codigoIdeAdv, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Ideadv.FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Ideadv == codigoIdeAdv, ct);
        }

        public async Task<bool> QuantidadeMaximaDeIdeAdvsExcedida(int quantidadeMaxima, long codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Ideadv.Where(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExisteIdeAdvPorTpInscAsync(int codigoInfoCrIrrf, string ideadvNrinsc, byte ideadvTpinsc, CancellationToken ct)
        {
            var apenasDigitos = new Regex(@"[^\d]");

            return await _eSocialDbContext.EsF2501Ideadv.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdeadvNrinsc == apenasDigitos.Replace(ideadvNrinsc, "") && x.IdeadvTpinsc == ideadvTpinsc, ct);
        }

        public async Task<bool> ExisteIdeAdvPorTpInscAlteraAsync(int codigoInfoCrIrrf, string ideadvNrinsc, byte ideadvTpinsc, long codigoIdeAdv, CancellationToken ct)
        {
            var apenasDigitos = new Regex(@"[^\d]");

            return await _eSocialDbContext.EsF2501Ideadv.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Ideadv != codigoIdeAdv && x.IdeadvNrinsc == apenasDigitos.Replace(ideadvNrinsc, "") && x.IdeadvTpinsc == ideadvTpinsc, ct);
        }

        public void AdicionaIdeAdvAoContexto(ref EsF2501Ideadv esF2501Ideadv)
        {
            _eSocialDbContext.Add(esF2501Ideadv);
        }

        public void RemoveIdeAdvAoContexto(ref EsF2501Ideadv esF2501Ideadv)
        {
            _eSocialDbContext.Remove(esF2501Ideadv);
        }

        #endregion
    }
}
