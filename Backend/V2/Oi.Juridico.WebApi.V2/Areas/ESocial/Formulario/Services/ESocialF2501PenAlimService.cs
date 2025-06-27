using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501PenAlimService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501PenAlimService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoPenAlim(EsF2501PenAlimRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, irrf).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoPenAlim(EsF2501PenAlimRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Penalim penAlim, EsF2501Infocrirrf irrf)
        {
            if (penAlim == null || irrf == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da penção alimentícia.");
            }

            var requestDTO = new EsF2501PenAlimRequestDTO()
            {
                PenalimTprend = penAlim.PenalimTprend,
                PenalimCpfdep = penAlim.PenalimCpfdep,
                PenalimVlrpensao = penAlim.PenalimVlrpensao

            };

            return ValidaErrosGlobais(requestDTO, irrf);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501PenAlimRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosGlobais = new List<string>();

            if (irrf.InfocrcontribTpcr == 188951 && requestDTO.PenalimTprend != ESocialTipoRendimento.RRA.ToByte())
            {
                listaErrosGlobais.Add("Se o Código de Receita – CR relativo a Imposto de Renda Retido na Fonte for igual a \"188951 - IRRF - RRA\", obrigatoriamente e exclusivamente o campo Tipo de Rendimento (Bloco E) deverá ser de preenchimento com o valor 18 - RRA.");
            }

            if (irrf.InfocrcontribTpcr != 188951 && requestDTO.PenalimTprend == ESocialTipoRendimento.RRA.ToByte())
            {
                listaErrosGlobais.Add("Somente informe o Tipo de Rendimento igual a \"18 - RRA\" quando o campo Código Receita (CR) IRRF (Bloco E) estiver preenchido com o valor \"188951 - IRRF - RRA\".");
            }

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501PenAlimRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreenchePenAlim(ref EsF2501Penalim penAlim, EsF2501PenAlimRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoInfocrirrf = null)
        {
            if (codigoInfocrirrf.HasValue)
            {
                penAlim.IdEsF2501Infocrirrf = codigoInfocrirrf.Value;
            }

            penAlim.PenalimTprend = requestDTO.PenalimTprend;
            penAlim.PenalimCpfdep = requestDTO.PenalimCpfdep;
            penAlim.PenalimVlrpensao = requestDTO.PenalimVlrpensao;

            penAlim.LogCodUsuario = usuario!.Identity!.Name;
            penAlim.LogDataOperacao = DateTime.Now;
        }

        public EsF2501PenAlimDTO PreenchePenAlimDTO(ref EsF2501Penalim? penAlim)
        {
            return new EsF2501PenAlimDTO()
            {
                IdEsF2501Penalim = penAlim!.IdEsF2501Penalim,
                IdEsF2501Infocrirrf = penAlim!.IdEsF2501Infocrirrf,
                LogCodUsuario = penAlim!.LogCodUsuario,
                LogDataOperacao = penAlim!.LogDataOperacao,
                PenalimTprend = penAlim.PenalimTprend,
                PenalimCpfdep = penAlim.PenalimCpfdep,
                PenalimVlrpensao = penAlim.PenalimVlrpensao,
                DescricaoTipoRend = penAlim!.PenalimTprend != null ? $"{penAlim!.PenalimTprend.Value} - {penAlim!.PenalimTprend.Value.ToEnum<ESocialTipoRendimento>().Descricao()}" : string.Empty
            };
        }

        public IQueryable<EsF2501PenAlimDTO> RecuperaListaPenAlim(long codigoInfocrirrf)
        {
            return from penAlim in _eSocialDbContext.EsF2501Penalim.AsNoTracking()
                   where penAlim.IdEsF2501Infocrirrf == codigoInfocrirrf
                   select new EsF2501PenAlimDTO()
                   {
                       IdEsF2501Penalim = penAlim!.IdEsF2501Penalim,
                       IdEsF2501Infocrirrf = penAlim!.IdEsF2501Infocrirrf,
                       LogCodUsuario = penAlim!.LogCodUsuario,
                       LogDataOperacao = penAlim!.LogDataOperacao,
                       PenalimTprend = penAlim.PenalimTprend,
                       PenalimCpfdep = penAlim.PenalimCpfdep,
                       PenalimVlrpensao = penAlim.PenalimVlrpensao,
                       DescricaoTipoRend = penAlim!.PenalimTprend != null ? $"{penAlim!.PenalimTprend.Value} - {penAlim!.PenalimTprend.Value.ToEnum<ESocialTipoRendimento>().Descricao()}" : string.Empty
                   };
        }

        public async Task<bool> ExistePenAlimPorIdAsync(int codigoInfoCrIrrf, long codigoPenAlim, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Penalim.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Penalim == codigoPenAlim, ct);
        }

        public async Task<EsF2501Penalim?> RetornaPenAlimPorIdAsync(int codigoInfoCrIrrf, long codigoPenAlim, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Penalim.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Penalim == codigoPenAlim, ct);
        }

        public async Task<EsF2501Penalim?> RetornaPenAlimEditavelPorIdAsync(int codigoInfoCrIrrf, long codigoPenAlim, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Penalim.FirstOrDefaultAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Penalim == codigoPenAlim, ct);
        }

        public async Task<bool> QuantidadeMaximaDePenAlimsExcedida(int quantidadeMaxima, long codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Penalim.Where(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExistePenAlimPorTpCpfAsync(int codigoInfoCrIrrf, string penalimCpfdep, byte penalimTprend, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Penalim.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.PenalimCpfdep == penalimCpfdep && x.PenalimTprend == penalimTprend, ct);
        }

        public async Task<bool> ExistePenAlimPorTpCpfAlteraAsync(int codigoInfoCrIrrf, string penalimCpfdep, byte penalimTprend, long codigoPenAlim, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Penalim.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.IdEsF2501Penalim != codigoPenAlim && x.PenalimCpfdep == penalimCpfdep && x.PenalimTprend == penalimTprend, ct);
        }

        public void AdicionaPenAlimAoContexto(ref EsF2501Penalim esF2501Penalim)
        {
            _eSocialDbContext.Add(esF2501Penalim);
        }

        public void RemovePenAlimDoContexto(ref EsF2501Penalim esF2501Penalim)
        {
            _eSocialDbContext.Remove(esF2501Penalim);
        }

        #endregion
    }
}
