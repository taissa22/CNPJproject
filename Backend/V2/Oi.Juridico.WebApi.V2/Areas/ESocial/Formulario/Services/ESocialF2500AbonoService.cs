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
    public class ESocialF2500AbonoService
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;
        private readonly ESocialF2500Service _esocialF2500Service;

        public ESocialF2500AbonoService(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        public IEnumerable<string> ValidaInclusaoAbono(EsF2500AbonoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato, _eSocialDbContext).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, contrato, _eSocialDbContext).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoAbono(EsF2500AbonoRequestDTO requestDTO, int codigoAbono, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato, _eSocialDbContext).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(requestDTO, codigoAbono, contrato, _eSocialDbContext).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Abono abono, EsF2500Infocontrato contrato, ESocialDbContext dbContext)
        {
            if (abono == null || contrato == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da remuneração.");
            }

            var requestDTO = new EsF2500AbonoRequestDTO()
            {
                AbonoAnobase = abono.AbonoAnobase
            };

            return ValidaErrosGlobais(requestDTO, contrato, dbContext);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500AbonoRequestDTO requestDTO, EsF2500Infocontrato contrato, ESocialDbContext dbContext)
        {
            var listaErrosGlobais = new List<string>();

            if (string.IsNullOrEmpty(contrato.InfovlrIdenabono) || !string.IsNullOrEmpty(contrato.InfovlrIdenabono) && contrato.InfovlrIdenabono == "N")
            {
                listaErrosGlobais.Add($"Não deve ser informado o grupo \"Abono\" (Bloco J), quando o campo \"Indenização Abono Salarial\" estiver desmarcado.");
            }

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2500AbonoRequestDTO requestDTO, EsF2500Infocontrato contrato, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();

            if (dbContext.EsF2500Abono.Any(x => x.AbonoAnobase == requestDTO.AbonoAnobase && x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato))
            {
                listaErrosInclusao.Add("Este Ano Base já foi informado.");
            }

            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(EsF2500AbonoRequestDTO requestDTO, int codigoAbono, EsF2500Infocontrato contrato, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();

            if (dbContext.EsF2500Abono.Any(x => x.AbonoAnobase == requestDTO.AbonoAnobase && x.IdEsF2500Abono != codigoAbono && x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato))
            {
                listaErrosInclusao.Add("Este Ano Base já foi informado.");
            }

            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheAbono(ref EsF2500Abono abono, EsF2500AbonoRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoContrato = null)
        {
            if (codigoContrato.HasValue)
            {
                abono.IdEsF2500Infocontrato = codigoContrato.Value;
            }

            abono.AbonoAnobase = requestDTO.AbonoAnobase;

            abono.LogCodUsuario = usuario!.Identity!.Name;
            abono.LogDataOperacao = DateTime.Now;
        }

        public EsF2500AbonoDTO PreencheAbonoDTO(ref EsF2500Abono? abono)
        {
            return new EsF2500AbonoDTO()
            {
                IdEsF2500Abono = abono!.IdEsF2500Abono,
                IdEsF2500Infocontrato = abono!.IdEsF2500Infocontrato,
                LogCodUsuario = abono!.LogCodUsuario,
                LogDataOperacao = abono!.LogDataOperacao,
                AbonoAnobase = abono!.AbonoAnobase
            };
        }

        public IQueryable<EsF2500AbonoDTO> RecuperaListaAbono(long codigoContrato)
        {
            return from ab in _eSocialDbContext.EsF2500Abono.AsNoTracking()
                   where ab.IdEsF2500Infocontrato == codigoContrato
                   select new EsF2500AbonoDTO()
                   {
                       IdEsF2500Infocontrato = ab.IdEsF2500Infocontrato,
                       IdEsF2500Abono = ab.IdEsF2500Abono,
                       LogCodUsuario = ab.LogCodUsuario,
                       LogDataOperacao = ab.LogDataOperacao,
                       AbonoAnobase = ab.AbonoAnobase
                   };
        }

        public async Task<bool> ExisteAbonoPorIdAsync(long codigoContrato, long codigoAbono, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Abono.AsNoTracking().AnyAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Abono == codigoAbono, ct);
        }

        public async Task<EsF2500Abono?> RetornaAbonoPorIdAsync(long codigoContrato, long codigoAbono, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Abono.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Abono == codigoAbono, ct);
        }

        public async Task<EsF2500Abono?> RetornaAbonoUpdatPorIdAsync(long codigoContrato, long codigoAbono, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Abono.FirstOrDefaultAsync(x => x.IdEsF2500Infocontrato == codigoContrato && x.IdEsF2500Abono == codigoAbono, ct);
        }

        public async Task<bool> QuantidadeMaximaDeAbonosExcedida(int quantidadeMaxima, long codigoContrato, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500Abono.Where(x => x.IdEsF2500Infocontrato == codigoContrato).CountAsync(ct) >= quantidadeMaxima;
        }

        public void AdicionaAbonoAoContexto(ref EsF2500Abono esF2500Abono)
        {
            _eSocialDbContext.Add(esF2500Abono);
        }

        public void RemoveAbonoAoContexto(ref EsF2500Abono esF2500Abono)
        {
            _eSocialDbContext.Remove(esF2500Abono);
        }


        #endregion
    }
}
