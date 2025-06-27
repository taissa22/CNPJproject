using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501BenefPenService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501BenefPenService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoBenefPen(EsF2501BenefPenRequestDTO requestDTO, EsF2501Dedsusp dedSusp)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, dedSusp).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, dedSusp).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoBenefPen(EsF2501BenefPenRequestDTO requestDTO, EsF2501Dedsusp dedSusp)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, dedSusp).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Benefpen benefPen, EsF2501Dedsusp dedSusp)
        {
            if (benefPen == null || dedSusp == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da deduções suspensas.");
            }

            var requestDTO = new EsF2501BenefPenRequestDTO()
            {
                BenefpenCpfdep = benefPen.BenefpenCpfdep,
                BenefpenVlrdepensusp = benefPen.BenefpenVlrdepensusp

            };

            return ValidaErrosGlobais(requestDTO, dedSusp);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501BenefPenRequestDTO requestDTO, EsF2501Dedsusp dedSusp)
        {
            var listaErrosGlobais = new List<string>();

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501BenefPenRequestDTO requestDTO, EsF2501Dedsusp dedSusp)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheBenefPen(ref EsF2501Benefpen benefPen, EsF2501BenefPenRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoDedSusp = null)
        {
            if (codigoDedSusp.HasValue)
            {
                benefPen.IdEsF2501Dedsusp = codigoDedSusp.Value;
            }

            benefPen.BenefpenCpfdep = requestDTO.BenefpenCpfdep;
            benefPen.BenefpenVlrdepensusp = requestDTO.BenefpenVlrdepensusp;

            benefPen.LogCodUsuario = usuario!.Identity!.Name;
            benefPen.LogDataOperacao = DateTime.Now;
        }

        public EsF2501BenefPenDTO PreencheBenefPenDTO(ref EsF2501Benefpen? benefPen)
        {
            return new EsF2501BenefPenDTO()
            {
                IdEsF2501Dedsusp = benefPen!.IdEsF2501Dedsusp,
                IdEsF2501Benefpen = benefPen!.IdEsF2501Benefpen,
                LogCodUsuario = benefPen!.LogCodUsuario,
                LogDataOperacao = benefPen!.LogDataOperacao,
                BenefpenCpfdep = benefPen.BenefpenCpfdep,
                BenefpenVlrdepensusp = benefPen.BenefpenVlrdepensusp


            };
        }

        public IQueryable<EsF2501BenefPenDTO> RecuperaListaBenefPen(long codigoDedSusp)
        {
            return from benedPen in _eSocialDbContext.EsF2501Benefpen.AsNoTracking()
                   where benedPen.IdEsF2501Dedsusp == codigoDedSusp
                   select new EsF2501BenefPenDTO()
                   {
                       IdEsF2501Dedsusp = benedPen!.IdEsF2501Dedsusp,
                       IdEsF2501Benefpen = benedPen!.IdEsF2501Benefpen,
                       LogCodUsuario = benedPen!.LogCodUsuario,
                       LogDataOperacao = benedPen!.LogDataOperacao,
                       BenefpenCpfdep = benedPen.BenefpenCpfdep,
                       BenefpenVlrdepensusp = benedPen.BenefpenVlrdepensusp
                   };
        }

        public async Task<bool> ExisteBenefPenPorIdAsync(long codigoBenefPen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Benefpen.AsNoTracking().AnyAsync(x => x.IdEsF2501Benefpen == codigoBenefPen, ct);
        }

        public async Task<EsF2501Benefpen?> RetornaBenefPenPorIdAsync(long codigoBenefPen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Benefpen.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Benefpen == codigoBenefPen, ct);
        }

        public async Task<EsF2501Benefpen?> RetornaBenefPenEditavelPorIdAsync(long codigoBenefPen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Benefpen.FirstOrDefaultAsync(x => x.IdEsF2501Benefpen == codigoBenefPen, ct);
        }

        public async Task<bool> QuantidadeMaximaDeBenefPensExcedida(int quantidadeMaxima, long codigoDedSusp, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Benefpen.Where(x => x.IdEsF2501Benefpen == codigoDedSusp).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExisteBenefPenAsync(long codigoDedSusp, string benefpenCpfdep, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Benefpen.AsNoTracking().AnyAsync(x => x.IdEsF2501Dedsusp == codigoDedSusp && x.BenefpenCpfdep == benefpenCpfdep, ct);
        }

        public async Task<bool> ExisteBenefPenAlteraAsync(long codigoBenefPen, long codigoDedSusp, string benefpenCpfdep, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Benefpen.AsNoTracking().AnyAsync(x => x.IdEsF2501Benefpen != codigoBenefPen && x.IdEsF2501Dedsusp == codigoDedSusp && x.BenefpenCpfdep == benefpenCpfdep, ct);
        }

        public void AdicionaBenefPenAoContexto(ref EsF2501Benefpen esF2501Benefpen)
        {
            _eSocialDbContext.Add(esF2501Benefpen);
        }

        public void RemoveBenefPenDoContexto(ref EsF2501Benefpen esF2501Benefpen)
        {
            _eSocialDbContext.Remove(esF2501Benefpen);
        }

        #endregion
    }
}
