using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501DedDepenService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501DedDepenService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoDedDepen(EsF2501DedDepenRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, irrf).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoDedDepen(EsF2501DedDepenRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Deddepen dedDepen, EsF2501Infocrirrf irrf)
        {
            if (dedDepen == null || irrf == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da dededução de dependente.");
            }

            var requestDTO = new EsF2501DedDepenRequestDTO()
            {
                DeddepenTprend = dedDepen!.DeddepenTprend,
                DeddepenCpfdep = dedDepen!.DeddepenCpfdep,
                DeddepenVlrdeducao = dedDepen!.DeddepenVlrdeducao

            };

            return ValidaErrosGlobais(requestDTO, irrf);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501DedDepenRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosGlobais = new List<string>();

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501DedDepenRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheDedDepen(ref EsF2501Deddepen dedDepen, EsF2501DedDepenRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoInfocrirrf = null)
        {
            if (codigoInfocrirrf.HasValue)
            {
                dedDepen.IdEsF2501Infocrirrf = codigoInfocrirrf!.Value;
            }

            dedDepen.DeddepenTprend = requestDTO!.DeddepenTprend;
            dedDepen.DeddepenCpfdep = requestDTO!.DeddepenCpfdep;
            dedDepen.DeddepenVlrdeducao = requestDTO!.DeddepenVlrdeducao;

            dedDepen.LogCodUsuario = usuario!.Identity!.Name;
            dedDepen.LogDataOperacao = DateTime.Now;
        }

        public EsF2501DedDepenDTO PreencheDedDepenDTO(ref EsF2501Deddepen? dedDepen)
        {
            return new EsF2501DedDepenDTO()
            {
                IdEsF2501Deddepen = dedDepen!.IdEsF2501Deddepen,
                IdEsF2501Infocrirrf = dedDepen!.IdEsF2501Infocrirrf,
                LogCodUsuario = dedDepen!.LogCodUsuario,
                LogDataOperacao = dedDepen!.LogDataOperacao,
                DeddepenTprend = dedDepen!.DeddepenTprend,
                DeddepenCpfdep = dedDepen!.DeddepenCpfdep,
                DeddepenVlrdeducao = dedDepen!.DeddepenVlrdeducao,
                DescricaoTipoRend = dedDepen!.DeddepenTprend != null ? $"{dedDepen!.DeddepenTprend.Value} - {dedDepen!.DeddepenTprend.Value.ToEnum<ESocialTipoRendimento>().Descricao()}" : string.Empty

            };
        }

        public IQueryable<EsF2501DedDepenDTO> RecuperaListaDedDepen(long codigoInfocrirrf)
        {
            return from dedDepen in _eSocialDbContext.EsF2501Deddepen.AsNoTracking()
                   where dedDepen.IdEsF2501Infocrirrf == codigoInfocrirrf
                   select new EsF2501DedDepenDTO()
                   {
                       IdEsF2501Deddepen = dedDepen!.IdEsF2501Deddepen,
                       IdEsF2501Infocrirrf = dedDepen!.IdEsF2501Infocrirrf,
                       LogCodUsuario = dedDepen!.LogCodUsuario,
                       LogDataOperacao = dedDepen!.LogDataOperacao,
                       DeddepenTprend = dedDepen!.DeddepenTprend,
                       DeddepenCpfdep = dedDepen!.DeddepenCpfdep,
                       DeddepenVlrdeducao = dedDepen!.DeddepenVlrdeducao,
                       DescricaoTipoRend = dedDepen!.DeddepenTprend != null ? $"{dedDepen!.DeddepenTprend.Value} - {dedDepen!.DeddepenTprend.Value.ToEnum<ESocialTipoRendimento>().Descricao()}" : string.Empty
                   };
        }

        public async Task<bool> ExisteDedDepenPorIdAsync(int codigoInfoCrIrrf, long codigoDedDepen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Deddepen.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Deddepen == codigoDedDepen, ct);
        }

        public async Task<EsF2501Deddepen?> RetornaDedDepenPorIdAsync(int codigoInfoCrIrrf, long codigoDedDepen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Deddepen.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Deddepen == codigoDedDepen, ct);
        }

        public async Task<EsF2501Deddepen?> RetornaDedDepenEditavelPorIdAsync(int codigoInfoCrIrrf, long codigoDedDepen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Deddepen.FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Deddepen == codigoDedDepen, ct);
        }

        public async Task<bool> QuantidadeMaximaDeDedDepensExcedida(int quantidadeMaxima, long codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Deddepen.Where(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExisteDedDepenPorTipoCpfAsync(int codigoInfoCrIrrf, string deddepenCpfdep, byte deddepenTprend, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Deddepen.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.DeddepenCpfdep == deddepenCpfdep && x.DeddepenTprend == deddepenTprend, ct);
        }

        public async Task<bool> ExisteDedDepenPorTipoCpfAlteraAsync(int codigoInfoCrIrrf, string deddepenCpfdep, byte deddepenTprend, long codigoDedDepen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Deddepen.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Deddepen != codigoDedDepen && x.DeddepenCpfdep == deddepenCpfdep && x.DeddepenTprend == deddepenTprend, ct);
        }

        public void AdicionaDedDepenAoContexto(ref EsF2501Deddepen esF2501Deddepen)
        {
            _eSocialDbContext.Add(esF2501Deddepen);
        }

        public void RemoveDedDepenAoContexto(ref EsF2501Deddepen esF2501Deddepen)
        {
            _eSocialDbContext.Remove(esF2501Deddepen);
        }

        #endregion
    }
}
