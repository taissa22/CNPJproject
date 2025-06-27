using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using SixLabors.ImageSharp;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501DedSuspService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501DedSuspService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoDedSusp(EsF2501DedSuspRequestDTO requestDTO, EsF2501Infovalores infoValores)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, infoValores).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, infoValores, _eSocialDbContext).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoDedSusp(EsF2501DedSuspRequestDTO requestDTO, EsF2501Infovalores infoValores, int codigoDedSusp)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, infoValores).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(requestDTO, infoValores, codigoDedSusp, _eSocialDbContext).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Dedsusp dedSusp, EsF2501Infovalores infoValores)
        {
            if (dedSusp == null || infoValores == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da deduções suspensas de benficiarios de pensão alimentícia.");
            }

            var requestDTO = new EsF2501DedSuspRequestDTO()
            {
                DedsuspIndtpdeducao = dedSusp!.DedsuspIndtpdeducao,
                DedsuspVlrdedsusp = dedSusp!.DedsuspVlrdedsusp

            };

            return ValidaErrosGlobais(requestDTO, infoValores);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501DedSuspRequestDTO requestDTO, EsF2501Infovalores infoValores)
        {
            var listaErrosGlobais = new List<string>();

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501DedSuspRequestDTO requestDTO, EsF2501Infovalores infoValores, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();

            if (dbContext.EsF2501Dedsusp.Any(x => x.DedsuspIndtpdeducao == requestDTO.DedsuspIndtpdeducao && x.IdEsF2501Infovalores == infoValores.IdEsF2501Infovalores))
            {
                listaErrosInclusao.Add("Não é possível incluir o mesmo \"Tipo de Dedução\" mais de uma vez.");
            }

            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(EsF2501DedSuspRequestDTO requestDTO, EsF2501Infovalores infoValores, int codigoDedSusp, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();

            if (dbContext.EsF2501Dedsusp.Any(x => x.DedsuspIndtpdeducao == requestDTO.DedsuspIndtpdeducao && x.IdEsF2501Infovalores == infoValores.IdEsF2501Infovalores && x.IdEsF2501Dedsusp != codigoDedSusp))
            {
                listaErrosInclusao.Add("Não é possível incluir o mesmo \"Tipo de Dedução\" mais de uma vez.");
            }


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheDedSusp(ref EsF2501Dedsusp benefPen, EsF2501DedSuspRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoInfoValores = null)
        {
            if (codigoInfoValores.HasValue)
            {
                benefPen.IdEsF2501Infovalores = codigoInfoValores.Value;
            }

            benefPen.DedsuspIndtpdeducao = requestDTO!.DedsuspIndtpdeducao;
            benefPen.DedsuspVlrdedsusp = requestDTO!.DedsuspVlrdedsusp;

            benefPen.LogCodUsuario = usuario!.Identity!.Name;
            benefPen.LogDataOperacao = DateTime.Now;
        }

        public EsF2501DedSuspDTO PreencheDedSuspDTO(ref EsF2501Dedsusp? dedSusp)
        {
            return new EsF2501DedSuspDTO()
            {
                IdEsF2501Dedsusp = dedSusp!.IdEsF2501Dedsusp,
                IdEsF2501Infovalores = dedSusp!.IdEsF2501Infovalores,
                LogCodUsuario = dedSusp!.LogCodUsuario,
                LogDataOperacao = dedSusp!.LogDataOperacao,
                DedsuspIndtpdeducao = dedSusp!.DedsuspIndtpdeducao,
                DedsuspVlrdedsusp = dedSusp!.DedsuspVlrdedsusp


            };
        }

        public IQueryable<EsF2501DedSuspDTO> RecuperaListaDedSusp(long codigoInfoValores)
        {
            return from dedSusp in _eSocialDbContext.EsF2501Dedsusp.AsNoTracking()
                   where dedSusp.IdEsF2501Infovalores == codigoInfoValores
                   select new EsF2501DedSuspDTO()
                   {
                       IdEsF2501Dedsusp = dedSusp!.IdEsF2501Dedsusp,
                       IdEsF2501Infovalores = dedSusp!.IdEsF2501Infovalores,
                       LogCodUsuario = dedSusp!.LogCodUsuario,
                       LogDataOperacao = dedSusp!.LogDataOperacao,
                       DedsuspIndtpdeducao = dedSusp!.DedsuspIndtpdeducao,
                       DedsuspVlrdedsusp = dedSusp!.DedsuspVlrdedsusp
                   };
        }

        public async Task<bool> ExisteDedSuspPorIdAsync(long codigoBenefPen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().AnyAsync(x => x.IdEsF2501Dedsusp == codigoBenefPen, ct);
        }

        public async Task<EsF2501Dedsusp?> RetornaDedSuspPorIdAsync(long codigoBenefPen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Dedsusp == codigoBenefPen, ct);
        }

        public async Task<EsF2501Dedsusp?> RetornaDedSuspnEditavelPorIdAsync(long codigoBenefPen, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Dedsusp.FirstOrDefaultAsync(x => x.IdEsF2501Dedsusp == codigoBenefPen, ct);
        }

        public async Task<bool> QuantidadeMaximaDeDedSuspExcedida(int quantidadeMaxima, long codigoDedSusp, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Dedsusp.Where(x => x.IdEsF2501Dedsusp == codigoDedSusp).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExisteDedSuspAsync(long codigoInfoValores, byte dedsuspIndtpdeducao, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().AnyAsync(x => x.IdEsF2501Infovalores == codigoInfoValores && x.DedsuspIndtpdeducao == dedsuspIndtpdeducao, ct);
        }

        public async Task<bool> ExisteDedSuspAlteraAsync(long codigoDedSusp, long codigoInfoValores, byte dedsuspIndtpdeducao, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().AnyAsync(x => x.IdEsF2501Dedsusp != codigoDedSusp && x.IdEsF2501Infovalores == codigoInfoValores && x.DedsuspIndtpdeducao == dedsuspIndtpdeducao, ct);
        }

        public void AdicionaDedSuspAoContexto(ref EsF2501Dedsusp esF2501Dedsusp)
        {
            _eSocialDbContext.Add(esF2501Dedsusp);
        }

        public void RemoveDedSuspDoContexto(ref EsF2501Dedsusp esF2501Dedsusp, long codigoDedSusp)
        {
            var benefPen = _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == codigoDedSusp);

            if (benefPen.Any())
            {
                foreach (var item2 in benefPen)
                {
                    item2.LogCodUsuario = esF2501Dedsusp.LogCodUsuario; //User!.Identity!.Name;
                    item2.LogDataOperacao = DateTime.Now;
                    _eSocialDbContext.Remove(item2);
                }
            }
            _eSocialDbContext.Remove(esF2501Dedsusp);
        }

        #endregion
    }
}
