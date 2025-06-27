using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Shared.Tools;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2501InfoDepService
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialF2501InfoDepService(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public IEnumerable<string> ValidaInclusaoInfoDep(EsF2501InfoDepRequestDTO requestDTO, EsF2501 esf2501)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, esf2501).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, esf2501, _eSocialDbContext).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoInfoDep(int codigoInfoDep, EsF2501InfoDepRequestDTO requestDTO, EsF2501 esf2501)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, esf2501).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(codigoInfoDep, requestDTO, esf2501, _eSocialDbContext).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2501Infodep infoDep, EsF2501 esf2501)
        {
            if (infoDep == null || esf2501 == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da informação de dependente.");
            }

            var requestDTO = new EsF2501InfoDepRequestDTO()
            {
                InfodepCpfdep = infoDep!.InfodepCpfdep,
                InfodepDtnascto = infoDep!.InfodepDtnascto,
                InfodepNome = infoDep!.InfodepNome,
                InfodepDepirrf = infoDep!.InfodepDepirrf,
                InfodepTpdep = infoDep!.InfodepTpdep,
                InfodepDescrdep = infoDep!.InfodepDescrdep

            };

            return ValidaErrosGlobais(requestDTO, esf2501);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2501InfoDepRequestDTO requestDTO, EsF2501 esf2501)
        {
            var listaErrosGlobais = new List<string>();

            if (requestDTO.InfodepCpfdep.RemoverCaracteres() == esf2501.IdetrabCpftrab)
            {
                listaErrosGlobais.Add("O CPF do dependente deve ser diferente do CPF do  trabalhador");
            }

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2501InfoDepRequestDTO requestDTO, EsF2501 esf2501, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();

            var jaExiste = dbContext.EsF2501Infodep.Any(x => x.IdEsF2501 == esf2501.IdF2501 && x.InfodepCpfdep == requestDTO.InfodepCpfdep.RemoverCaracteres());

            if (jaExiste)
            {
                listaErrosInclusao.Add("Não é permitido CPFs duplicados.");
            }

            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(int codigoInfoDep, EsF2501InfoDepRequestDTO requestDTO, EsF2501 esf2501, ESocialDbContext dbContext)
        {
            var listaErrosInclusao = new List<string>();

            var jaExiste = dbContext.EsF2501Infodep.Any(x => x.IdEsF2501 == esf2501.IdF2501 && x.IdEsF2501Infodep != codigoInfoDep && x.InfodepCpfdep == requestDTO.InfodepCpfdep.RemoverCaracteres());

            if (jaExiste)
            {
                listaErrosInclusao.Add("Não é permitido CPFs duplicados.");
            }

            return listaErrosInclusao;
        }

        #region Métodos Auxiliares

        public void PreencheInfoDep(ref EsF2501Infodep infoDep, EsF2501InfoDepRequestDTO requestDTO, ClaimsPrincipal? usuario, int? codigoFormulario = null)
        {
            if (codigoFormulario.HasValue)
            {
                infoDep.IdEsF2501 = codigoFormulario.Value;
            }
            infoDep.InfodepCpfdep = requestDTO.InfodepCpfdep.RemoverCaracteres();
            infoDep.InfodepDtnascto = requestDTO!.InfodepDtnascto;
            infoDep.InfodepNome = requestDTO!.InfodepNome;
            infoDep.InfodepDepirrf = !string.IsNullOrEmpty(requestDTO!.InfodepDepirrf) && requestDTO!.InfodepDepirrf == "true" ? "S" : null;
            infoDep.InfodepTpdep = requestDTO!.InfodepTpdep;
            infoDep.InfodepDescrdep = requestDTO!.InfodepDescrdep;

            infoDep.LogCodUsuario = usuario!.Identity!.Name;
            infoDep.LogDataOperacao = DateTime.Now;
        }

        public EsF2501InfoDepDTO PreencheInfoDepDTO(ref EsF2501Infodep? infoDep)
        {
            return new EsF2501InfoDepDTO()
            {
                IdEsF2501Infodep = infoDep!.IdEsF2501Infodep,
                IdEsF2501 = infoDep!.IdEsF2501,
                LogCodUsuario = infoDep!.LogCodUsuario,
                LogDataOperacao = infoDep!.LogDataOperacao,
                InfodepCpfdep = infoDep!.InfodepCpfdep,
                InfodepDtnascto = infoDep!.InfodepDtnascto,
                InfodepNome = infoDep!.InfodepNome,
                InfodepDepirrf = infoDep!.InfodepDepirrf,
                InfodepTpdep = infoDep!.InfodepTpdep,
                InfodepDescrdep = infoDep!.InfodepDescrdep,
                DescricaoTpdep = string.Empty
            };
        }

        public IQueryable<EsF2501InfoDepDTO> RecuperaListaInfoDep(long codigoFormulario)
        {

            return from x in _eSocialDbContext.EsF2501Infodep.AsNoTracking()
                   join t7 in _eSocialDbContext.EsTabela07 on x.InfodepTpdep equals t7.CodEsTabela07.ToString() into leftJoin
                   from tab7 in leftJoin.DefaultIfEmpty()
                   where x.IdEsF2501 == codigoFormulario
                   select new EsF2501InfoDepDTO()
                   {
                       IdEsF2501 = x.IdEsF2501,
                       IdEsF2501Infodep = x.IdEsF2501Infodep,
                       LogCodUsuario = x.LogCodUsuario,
                       LogDataOperacao = x.LogDataOperacao,
                       InfodepCpfdep = x!.InfodepCpfdep,
                       InfodepDtnascto = x!.InfodepDtnascto,
                       InfodepNome = x!.InfodepNome,
                       InfodepDepirrf = x!.InfodepDepirrf,
                       InfodepTpdep = x!.InfodepTpdep,
                       InfodepDescrdep = x!.InfodepDescrdep,
                       DescricaoTpdep = !string.IsNullOrEmpty(tab7.DscEsTabela07) ? $"{x.InfodepTpdep} - {tab7.DscEsTabela07}" : string.Empty
                   };
        }

        public async Task RemoveInfoDep(long codigoInfodep, CancellationToken ct)
        {
            var infoDepExclusao = await _eSocialDbContext.EsF2501Infodep.FirstOrDefaultAsync(x => x.IdEsF2501Infodep == codigoInfodep, ct);
            if (infoDepExclusao is not null)
            {
                _eSocialDbContext.Remove(infoDepExclusao);
            }
        }

        public async Task ApagaListaInfoDepECommita(int codigoFormulario, ClaimsPrincipal? usuario, CancellationToken ct)
        {
            var listaInfoDep = await _eSocialDbContext.EsF2501Infodep
                                    .Where(x => x.IdEsF2501 == codigoFormulario).AsNoTracking().ToListAsync(ct);

            foreach (var infoDep in listaInfoDep)
            {
                _eSocialDbContext.Remove(infoDep);
            }

            await _eSocialDbContext.SaveChangesExternalScopeAsync(usuario!.Identity!.Name, true, ct);
        }

        //private FileResult GerarCSV(List<EsF2501InfoDepDTO> resultado, bool ascendente)
        //{
        //    var lista = ascendente ? resultado.ToArray().OrderBy(x => x.CalctribPerref) : resultado.ToArray().OrderByDescending(x => x.CalctribPerref);

        //    var csv = lista.ToCsvByteArray(typeof(ExportaF2501PlanilhaCalcTribMap), sanitizeForInjection: false);
        //    var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

        //    return File(bytes, "text/csv", $"BlocoC_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
        //}

        public async Task<bool> ExisteInfoDepPorIdAsync(long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infodep.AsNoTracking().AnyAsync(x => x.IdEsF2501Infodep == codigoInfoValores, ct);
        }

        public async Task<EsF2501Infodep?> RetornaInfoDepPorIdAsync(long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infodep.AsNoTracking().FirstOrDefaultAsync(x => x.IdEsF2501Infodep == codigoInfoValores, ct);
        }

        public async Task<EsF2501Infodep?> RetornaInfoDepEditavelPorIdAsync(long codigoInfoValores, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infodep.FirstOrDefaultAsync(x => x.IdEsF2501Infodep == codigoInfoValores, ct);
        }

        public async Task<bool> QuantidadeMaximaDeInfoDepExcedida(int quantidadeMaxima, long codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2501Infodep.Where(x => x.IdEsF2501 == codigoFormulario).CountAsync(ct) >= quantidadeMaxima;
        }

        public void AdicionaInfoDepAoContexto(ref EsF2501Infodep esF2501Infodep)
        {
            _eSocialDbContext.Add(esF2501Infodep);
        }

        public void RemoveInfoDepAoContexto(ref EsF2501Infodep esF2501Infodep)
        {
            _eSocialDbContext.Remove(esF2501Infodep);
        }

        #endregion
    }
}


