using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Oi.Juridico.WebApi.V2.Services;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2500InfoIntermService
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;
        private readonly ESocialF2500Service _esocialF2500Service;

        public ESocialF2500InfoIntermService(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService, ESocialF2500Service esocialF2500Service)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
            _esocialF2500Service = esocialF2500Service;
        }

        public IEnumerable<string> ValidaInclusaoInfoInterm(EsF2500InfoIntermRequestDTO requestDTO)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, _eSocialDbContext).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, _eSocialDbContext).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoInfoInterm(EsF2500InfoIntermRequestDTO requestDTO, long codigoInfoInterm)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, _eSocialDbContext).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(requestDTO, codigoInfoInterm, _eSocialDbContext).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Infointerm infoInterm, ESocialDbContext dbContext)
        {
            if (infoInterm == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação do trabalho intermitente.");
            }

            var requestDTO = new EsF2500InfoIntermRequestDTO()
            {
                InfointermDia = infoInterm.InfointermDia,
                InfointermHrstrab = infoInterm.InfointermHrstrab,
            };

            return ValidaErrosGlobais(requestDTO, dbContext);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500InfoIntermRequestDTO requestDTO, ESocialDbContext dbContext)
        {
            var listaErrosGlobais = new List<string>();


            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2500InfoIntermRequestDTO requestDTO, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(EsF2500InfoIntermRequestDTO requestDTO, long codigoInfoInterm, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheInfoInterm(ref EsF2500Infointerm infoInterm, EsF2500InfoIntermRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoPeriodo = null)
        {
            if (codigoPeriodo.HasValue)
            {
                infoInterm.IdEsF2500Ideperiodo = codigoPeriodo.Value;
            }
            if (!requestDTO.IdEsF2500Infointerm.HasValue)
            {
                infoInterm.InfointermDia = requestDTO.InfointermDia.HasValue ? requestDTO.InfointermDia : 0;                
            }

            infoInterm.InfointermHrstrab = requestDTO.InfointermHrstrab;

            infoInterm.LogCodUsuario = usuario!.Identity!.Name;
            infoInterm.LogDataOperacao = DateTime.Now;
        }

        public void InsereInfoIntermZerado(ClaimsPrincipal? usuario, CancellationToken ct, long? codigoPeriodo = null)
        {
            var infoInterm = new EsF2500Infointerm { 
                IdEsF2500Ideperiodo = codigoPeriodo!.Value,
                InfointermDia = 0,
                //InfointermHrstrab = "0000",
                LogCodUsuario = usuario!.Identity!.Name,
                LogDataOperacao = DateTime.Now,
        };
            _eSocialDbContext.Add(infoInterm);

            _eSocialDbContext.SaveChangesAsync(ct);
        }

        public EsF2500InfoIntermDTO PreencheInfoIntermDTO(ref EsF2500Infointerm? infointerm)
        {
            return new EsF2500InfoIntermDTO()
            {
                IdEsF2500Ideperiodo = infointerm!.IdEsF2500Ideperiodo,
                IdEsF2500Infointerm = infointerm!.IdEsF2500Infointerm,
                InfointermDia = infointerm!.InfointermDia,
                InfointermHrstrab = !string.IsNullOrEmpty(infointerm!.InfointermHrstrab) ? infointerm!.InfointermHrstrab.Insert(2, ":") : string.Empty,
                LogCodUsuario = infointerm!.LogCodUsuario,
                LogDataOperacao = infointerm!.LogDataOperacao,
            };
        }

        public IQueryable<EsF2500InfoIntermDTO> RecuperaListaInfoInterm(int codigoInfoInterm)
        {
            var retorno = from infointerm in _eSocialDbContext.EsF2500Infointerm.AsNoTracking()
                          where infointerm.IdEsF2500Ideperiodo == codigoInfoInterm
                          select new EsF2500InfoIntermDTO()
                          {
                              IdEsF2500Ideperiodo = infointerm!.IdEsF2500Ideperiodo,
                              IdEsF2500Infointerm = infointerm!.IdEsF2500Infointerm,
                              InfointermDia = infointerm!.InfointermDia,
                              InfointermHrstrab = !string.IsNullOrEmpty(infointerm!.InfointermHrstrab) ? infointerm!.InfointermHrstrab.Insert(2, ":") : string.Empty,
                              LogCodUsuario = infointerm!.LogCodUsuario,
                              LogDataOperacao = infointerm!.LogDataOperacao
                          };

            return retorno;
        }

        public async Task<bool> ExisteInfoIntermPorIdAsync(long codigoPeriodo, long codigoInfoInterm, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.AsNoTracking().AnyAsync(x => x.IdEsF2500Ideperiodo == codigoPeriodo && x.IdEsF2500Infointerm == codigoInfoInterm, ct);
        }

        public async Task<bool> ExisteInfoIntermPorPeriodoAsync(long codigoPeriodo, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.AsNoTracking().AnyAsync(x => x.IdEsF2500Ideperiodo == codigoPeriodo, ct);
        }

        public async Task<EsF2500Infointerm?> RetornaInfoIntermPorIdAsync(long codigoPeriodo, long codigoInfoInterm, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Ideperiodo == codigoPeriodo && x.IdEsF2500Infointerm == codigoInfoInterm, ct);
        }

        public async Task<EsF2500Infointerm?> RetornaInfoIntermUpdatPorIdAsync(long codigoPeriodo, long codigoInfoInterm, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.FirstOrDefaultAsync(x => x.IdEsF2500Ideperiodo == codigoPeriodo && x.IdEsF2500Infointerm == codigoInfoInterm, ct);
        }

        public async Task<bool> QuantidadeMaximaDeInfoIntermExcedida(int quantidadeMaxima, long codigoPeriodo, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.Where(x => x.IdEsF2500Ideperiodo == codigoPeriodo).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> VerificaDia(int? dia, long codigoPeriodo, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.AnyAsync(x => x.IdEsF2500Ideperiodo == codigoPeriodo && x.InfointermDia == dia,ct);
        }

        public async Task<bool> VerificaDiaUpdate(int? dia, long codigoPeriodo, long? codigoInfoInterm, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Infointerm.AnyAsync(x => x.IdEsF2500Ideperiodo == codigoPeriodo && x.InfointermDia == dia && x.IdEsF2500Infointerm != codigoInfoInterm, ct);
        }

        public void AdicionaInfoIntermAoContexto(ref EsF2500Infointerm esF2500InfoInterm)
        {
            _eSocialDbContext.Add(esF2500InfoInterm);
        }

        public void RemoveInfoIntermAoContexto(ref EsF2500Infointerm esF2500InfoInterm)
        {
            _eSocialDbContext.Remove(esF2500InfoInterm);
        }


        #endregion
    }
}
