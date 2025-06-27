using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501InfoValoresService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501InfoValoresService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoInfoValores(EsF2501InfoValoresRequestDTO requestDTO, EsF2501Infoprocret infoProcRet)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, infoProcRet).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, infoProcRet).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoInfoValores(EsF2501InfoValoresRequestDTO requestDTO, EsF2501Infoprocret infoProcRet)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, infoProcRet).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Infovalores infoValores, EsF2501Infoprocret infoProcRet)
        {
            if (infoValores == null || infoProcRet == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da informação de valores de retenção de tributos.");
            }

            var requestDTO = new EsF2501InfoValoresRequestDTO()
            {
                InfovaloresIndapuracao = infoValores.InfovaloresIndapuracao,
                InfovaloresVlrcmpanoant = infoValores.InfovaloresVlrcmpanoant,
                InfovaloresVlrcmpanocal = infoValores.InfovaloresVlrcmpanocal,
                InfovaloresVlrdepjud = infoValores.InfovaloresVlrdepjud,
                InfovaloresVlrnretido = infoValores.InfovaloresVlrnretido,
                InfovaloresVlrrendsusp = infoValores.InfovaloresVlrrendsusp

            };

            return ValidaErrosGlobais(requestDTO, infoProcRet);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501InfoValoresRequestDTO requestDTO, EsF2501Infoprocret infoProcRet)
        {
            var listaErrosGlobais = new List<string>();

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501InfoValoresRequestDTO requestDTO, EsF2501Infoprocret infoProcRet)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheInfoValores(ref EsF2501Infovalores infoValores, EsF2501InfoValoresRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoInfoProcRet = null)
        {
            if (codigoInfoProcRet.HasValue)
            {
                infoValores.IdEsF2501Infoprocret = codigoInfoProcRet.Value;
            }

            infoValores.InfovaloresIndapuracao = requestDTO.InfovaloresIndapuracao;
            infoValores.InfovaloresVlrcmpanoant = requestDTO.InfovaloresVlrcmpanoant;
            infoValores.InfovaloresVlrcmpanocal = requestDTO.InfovaloresVlrcmpanocal;
            infoValores.InfovaloresVlrdepjud = requestDTO.InfovaloresVlrdepjud;
            infoValores.InfovaloresVlrnretido = requestDTO.InfovaloresVlrnretido;
            infoValores.InfovaloresVlrrendsusp = requestDTO.InfovaloresVlrrendsusp;

            infoValores.LogCodUsuario = usuario!.Identity!.Name;
            infoValores.LogDataOperacao = DateTime.Now;
        }

        public EsF2501InfoValoresDTO PreencheInfoValoresDTO(ref EsF2501Infovalores? infoValores)
        {
            return new EsF2501InfoValoresDTO()
            {
                IdEsF2501Infovalores = infoValores!.IdEsF2501Infovalores,
                IdEsF2501Infoprocret = infoValores!.IdEsF2501Infoprocret,
                LogCodUsuario = infoValores!.LogCodUsuario,
                LogDataOperacao = infoValores!.LogDataOperacao,
                InfovaloresIndapuracao = infoValores.InfovaloresIndapuracao,
                InfovaloresVlrcmpanoant = infoValores.InfovaloresVlrcmpanoant,
                InfovaloresVlrcmpanocal = infoValores.InfovaloresVlrcmpanocal,
                InfovaloresVlrdepjud = infoValores.InfovaloresVlrdepjud,
                InfovaloresVlrnretido = infoValores.InfovaloresVlrnretido,
                InfovaloresVlrrendsusp = infoValores.InfovaloresVlrrendsusp

            };
        }

        public IQueryable<EsF2501InfoValoresDTO> RecuperaListaInfoValores(long codigoInfoProcRet)
        {
            return from infoValores in _eSocialDbContext.EsF2501Infovalores.AsNoTracking()
                   where infoValores.IdEsF2501Infoprocret == codigoInfoProcRet
                   select new EsF2501InfoValoresDTO()
                   {
                       IdEsF2501Infovalores = infoValores!.IdEsF2501Infovalores,
                       IdEsF2501Infoprocret = infoValores!.IdEsF2501Infoprocret,
                       LogCodUsuario = infoValores!.LogCodUsuario,
                       LogDataOperacao = infoValores!.LogDataOperacao,
                       InfovaloresIndapuracao = infoValores.InfovaloresIndapuracao,
                       InfovaloresVlrcmpanoant = infoValores.InfovaloresVlrcmpanoant,
                       InfovaloresVlrcmpanocal = infoValores.InfovaloresVlrcmpanocal,
                       InfovaloresVlrdepjud = infoValores.InfovaloresVlrdepjud,
                       InfovaloresVlrnretido = infoValores.InfovaloresVlrnretido,
                       InfovaloresVlrrendsusp = infoValores.InfovaloresVlrrendsusp
                   };
        }

        public async Task<bool> ExisteInfoValoresPorIdAsync(long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infovalores.AsNoTracking().AnyAsync(x => x.IdEsF2501Infovalores == codigoInfoValores, ct);
        }

        public async Task<EsF2501Infovalores?> RetornaInfoValoresPorIdAsync(long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infovalores.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infovalores == codigoInfoValores, ct);
        }

        public async Task<EsF2501Infovalores?> RetornaInfoValoresEditavelPorIdAsync(long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infovalores.FirstOrDefaultAsync(x => x.IdEsF2501Infovalores == codigoInfoValores, ct);
        }

        public async Task<bool> QuantidadeMaximaDeInfoValoressExcedida(int quantidadeMaxima, long CodigoInfoProcRet, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infovalores.Where(x => x.IdEsF2501Infoprocret == CodigoInfoProcRet).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExisteInfoValoresAsync(long CodigoInfoProcRet, int infovaloresIndapuracao, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infovalores.AsNoTracking().AnyAsync(x => x.IdEsF2501Infoprocret == CodigoInfoProcRet && x.InfovaloresIndapuracao == infovaloresIndapuracao, ct);
        }

        public async Task<bool> ExisteInfoValoresAlteraAsync(long CodigoInfoProcRet, int infovaloresIndapuracao, long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infovalores.AsNoTracking().AnyAsync(x => x.IdEsF2501Infovalores != codigoInfoValores && x.IdEsF2501Infoprocret == CodigoInfoProcRet && x.InfovaloresIndapuracao == infovaloresIndapuracao, ct);
        }

        public void AdicionaInfoValoresAoContexto(ref EsF2501Infovalores esF2501Infovalores)
        {
            _eSocialDbContext.Add(esF2501Infovalores);
        }

        public void RemoveInfoValoresDoContexto(ref EsF2501Infovalores esF2501Infovalores, long codigoInfoValores)
        {
            var dedSusp = _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().Where(x => x.IdEsF2501Infovalores == codigoInfoValores);

            if (dedSusp.Any())
            {
                foreach (var item1 in dedSusp)
                {
                    var benefPen = _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == item1.IdEsF2501Dedsusp);

                    if (benefPen.Any())
                    {
                        foreach (var item2 in benefPen)
                        {
                            item2.LogCodUsuario = esF2501Infovalores.LogCodUsuario; //User!.Identity!.Name;
                            item2.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(item2);
                        }
                    }

                    item1.LogCodUsuario = esF2501Infovalores.LogCodUsuario; //User!.Identity!.Name;
                    item1.LogDataOperacao = DateTime.Now;
                    _eSocialDbContext.Remove(item1);
                }
            }


            _eSocialDbContext.Remove(esF2501Infovalores);
        }


        //public void RemoveInfoValoresDoContexto(ref EsF2501Infovalores esF2501Infovalores)
        //{
        //    _eSocialDbContext.Remove(esF2501Infovalores);
        //}

        #endregion
    }
}
