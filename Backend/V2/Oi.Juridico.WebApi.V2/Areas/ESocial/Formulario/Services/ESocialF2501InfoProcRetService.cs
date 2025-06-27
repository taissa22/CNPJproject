using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Perlink.Oi.Juridico.Application.Security;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501InfoProcRetService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501InfoProcRetService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoInfoProcRet(EsF2501InfoProcRetRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, irrf).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoInfoProcRet(EsF2501InfoProcRetRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, irrf).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Infoprocret infoProcRet, EsF2501Infocrirrf irrf)
        {
            if (infoProcRet == null || irrf == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da informação de processo de retenção de tributos.");
            }

            var requestDTO = new EsF2501InfoProcRetRequestDTO()
            {
                InfoprocretTpprocret = infoProcRet.InfoprocretTpprocret,
                InfoprocretCodsusp = infoProcRet.InfoprocretCodsusp,
                InfoprocretNrprocret = infoProcRet.InfoprocretNrprocret

            };

            return ValidaErrosGlobais(requestDTO, irrf);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501InfoProcRetRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosGlobais = new List<string>();

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501InfoProcRetRequestDTO requestDTO, EsF2501Infocrirrf irrf)
        {
            var listaErrosInclusao = new List<string>();


            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheInfoProcRet(ref EsF2501Infoprocret infoProcRet, EsF2501InfoProcRetRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoInfocrirrf = null)
        {
            if (codigoInfocrirrf.HasValue)
            {
                infoProcRet.IdEsF2501Infocrirrf = codigoInfocrirrf.Value;
            }

            infoProcRet.InfoprocretTpprocret = requestDTO.InfoprocretTpprocret;
            infoProcRet.InfoprocretCodsusp = requestDTO.InfoprocretCodsusp;
            infoProcRet.InfoprocretNrprocret = requestDTO.InfoprocretNrprocret;

            infoProcRet.LogCodUsuario = usuario!.Identity!.Name;
            infoProcRet.LogDataOperacao = DateTime.Now;
        }

        public EsF2501InfoProcRetDTO PreencheInfoProcRetDTO(ref EsF2501Infoprocret? infoProcRet)
        {
            return new EsF2501InfoProcRetDTO()
            {
                IdEsF2501Infoprocret = infoProcRet!.IdEsF2501Infoprocret,
                IdEsF2501Infocrirrf = infoProcRet!.IdEsF2501Infocrirrf,
                LogCodUsuario = infoProcRet!.LogCodUsuario,
                LogDataOperacao = infoProcRet!.LogDataOperacao,
                InfoprocretTpprocret = infoProcRet.InfoprocretTpprocret,
                InfoprocretCodsusp = infoProcRet.InfoprocretCodsusp,
                InfoprocretNrprocret = infoProcRet.InfoprocretNrprocret
            };
        }

        public IQueryable<EsF2501InfoProcRetDTO> RecuperaListaInfoProcRet(long codigoInfoIRRF)
        {
            return from infoProcRet in _eSocialDbContext.EsF2501Infoprocret.AsNoTracking()
                   where infoProcRet.IdEsF2501Infocrirrf == codigoInfoIRRF
                   select new EsF2501InfoProcRetDTO()
                   {
                       IdEsF2501Infoprocret = infoProcRet!.IdEsF2501Infoprocret,
                       IdEsF2501Infocrirrf = infoProcRet!.IdEsF2501Infocrirrf,
                       LogCodUsuario = infoProcRet!.LogCodUsuario,
                       LogDataOperacao = infoProcRet!.LogDataOperacao,
                       InfoprocretTpprocret = infoProcRet.InfoprocretTpprocret,
                       InfoprocretCodsusp = infoProcRet.InfoprocretCodsusp,
                       InfoprocretNrprocret = infoProcRet.InfoprocretNrprocret
                   };
        }

        public async Task<bool> ExisteInfoProcRetPorIdAsync(long codigoInfoProcRet, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().AnyAsync(x => x.IdEsF2501Infoprocret == codigoInfoProcRet, ct);
        }

        public async Task<EsF2501Infoprocret?> RetornaInfoProcRetPorIdAsync(long codigoInfoProcRet, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infoprocret == codigoInfoProcRet, ct);
        }

        public async Task<EsF2501Infoprocret?> RetornaInfoProcRetEditavelPorIdAsync(long codigoInfoProcRet, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infoprocret.FirstOrDefaultAsync(x => x.IdEsF2501Infoprocret == codigoInfoProcRet, ct);
        }

        public async Task<bool> QuantidadeMaximaDeInfoProcRetsExcedida(int quantidadeMaxima, long codigoInfoCrIrrf, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infoprocret.Where(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf).CountAsync(ct) >= quantidadeMaxima;
        }

        public async Task<bool> ExisteInfoProcRetAsync(long codigoInfoCrIrrf, EsF2501InfoProcRetRequestDTO requestDTO, CancellationToken ct)
        {
            if (requestDTO.InfoprocretCodsusp.HasValue)
            {
                return await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.InfoprocretCodsusp == requestDTO.InfoprocretCodsusp && x.InfoprocretNrprocret == requestDTO.InfoprocretNrprocret && x.InfoprocretTpprocret == requestDTO.InfoprocretTpprocret, ct);
            }
            else
            {
                return await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().AnyAsync(x => x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.InfoprocretNrprocret == requestDTO.InfoprocretNrprocret && x.InfoprocretTpprocret == requestDTO.InfoprocretTpprocret, ct);
            }
        }

        public async Task<bool> ExisteInfoProcRetAlteraAsync(long codigoInfoCrIrrf, EsF2501InfoProcRetRequestDTO requestDTO, long codigoInfoProcRet, CancellationToken ct)
        {
            if (requestDTO.InfoprocretCodsusp.HasValue)
            {
                return await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().AnyAsync(x => x.IdEsF2501Infoprocret != codigoInfoProcRet && x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.InfoprocretCodsusp == requestDTO.InfoprocretCodsusp && x.InfoprocretNrprocret == requestDTO.InfoprocretNrprocret && x.InfoprocretTpprocret == requestDTO.InfoprocretTpprocret, ct);
            }
            else
            {
                return await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().AnyAsync(x => x.IdEsF2501Infoprocret != codigoInfoProcRet && x.IdEsF2501Infocrirrf == codigoInfoCrIrrf && x.InfoprocretNrprocret == requestDTO.InfoprocretNrprocret && x.InfoprocretTpprocret == requestDTO.InfoprocretTpprocret, ct);

            }

        }

        public void AdicionaInfoProcRetAoContexto(ref EsF2501Infoprocret esF2501Infoprocret)
        {
            _eSocialDbContext.Add(esF2501Infoprocret);
        }

        public void RemoveInfoProcRetDoContexto(ref EsF2501Infoprocret esF2501Infoprocret, long codigoInfoProcRet)
        {
            var infoValores = _eSocialDbContext.EsF2501Infovalores.AsNoTracking().Where(x => x.IdEsF2501Infoprocret == codigoInfoProcRet);

            if (infoValores.Any())
            {
                foreach (var item in infoValores)
                {
                    var dedSusp = _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().Where(x => x.IdEsF2501Infovalores == item.IdEsF2501Infovalores);
                    if (dedSusp.Any())
                    {
                        foreach (var item1 in dedSusp)
                        {
                            var benefPen = _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == item1.IdEsF2501Dedsusp);

                            if (benefPen.Any())
                            {
                                foreach (var item2 in benefPen)
                                {
                                    item2.LogCodUsuario = esF2501Infoprocret.LogCodUsuario; //User!.Identity!.Name;
                                    item2.LogDataOperacao = DateTime.Now;
                                    _eSocialDbContext.Remove(item2);
                                }
                            }

                            item1.LogCodUsuario = esF2501Infoprocret.LogCodUsuario; //User!.Identity!.Name;
                            item1.LogDataOperacao = DateTime.Now;
                            _eSocialDbContext.Remove(item1);
                        }
                    }
                    item.LogCodUsuario = esF2501Infoprocret.LogCodUsuario; //User!.Identity!.Name;
                    item.LogDataOperacao = DateTime.Now;
                    _eSocialDbContext.Remove(item);
                }
            }
            _eSocialDbContext.Remove(esF2501Infoprocret);
        }

        //public void RemoveInfoProcRetDoContexto(ref EsF2501Infoprocret esF2501Infoprocret)
        //{
        //    _eSocialDbContext.Remove(esF2501Infoprocret);
        //}

        #endregion
    }
}
