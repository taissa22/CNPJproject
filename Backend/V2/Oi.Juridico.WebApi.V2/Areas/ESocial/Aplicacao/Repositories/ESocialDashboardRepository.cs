using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Tools;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using System.Globalization;
using System.Linq;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Repositories
{
    public class ESocialDashboardRepository
    {
        private readonly ESocialDbContext _eSocialDbContext;

        public ESocialDashboardRepository(ESocialDbContext eSocialDbContext)
        {
            _eSocialDbContext = eSocialDbContext;
        }

        public async Task<ESocialDashboardDTO?> ObtemDadosDashboard(DateTime dataPeriodoVigenteInicial, DateTime dataPeriodoVigenteFinal, bool naoFiltrarEmpresas, List<int> filtroEmpresas, bool naoFiltrarEstados, List<string> filtroEstados, CancellationToken ct)
        {
            var query = _eSocialDbContext.EsDashboard.Where(x => x.DatPeriodoVigente.Date >= dataPeriodoVigenteInicial.Date
                                                && x.DatPeriodoVigente.Date <= dataPeriodoVigenteFinal.Date
                                                && (naoFiltrarEmpresas || filtroEmpresas.Contains(x.IdEsEmpresaAgrupadora))
                                                && (naoFiltrarEstados || filtroEstados.Contains(x.CodEstado)))
                                                .GroupBy(x => new { x.IdEsEmpresaAgrupadora, x.DatPeriodoVigente })
                                                .Select(x => new ESocialDashboardDTO()
                                                {
                                                    IdEsEmpresaAgrupadora = x.Key.IdEsEmpresaAgrupadora,
                                                    DatPeriodoVigente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Key.DatPeriodoVigente.ToString(@"MMMM yyyy", new CultureInfo("pt-BR"))),
                                                    NumTotGeral = x.Sum(y => y.NumTotPendentes) + x.Sum(y => y.NumTotEnviados) + x.Sum(y => y.NumTotRetificados) + x.Sum(y => y.NumTotExcluidos),
                                                    NumTotPendentes = x.Sum(y => y.NumTotPendentes),
                                                    NumTotPendNaoAnalisados = x.Sum(y => y.NumTotPendNaoAnalisados),
                                                    NumTotPendNaoIniciados = x.Sum(y => y.NumTotPendNaoIniciados),
                                                    NumTotPendRascunhoEsc = x.Sum(y => y.NumTotPendRascunhoEsc),
                                                    NumTotPendRascunhoCont = x.Sum(y => y.NumTotPendRascunhoCont),
                                                    NumTotPendRascunhoEnv = x.Sum(y => y.NumTotPendRascunhoEnv),
                                                    NumTotPendRetornoCritica = x.Sum(y => y.NumTotPendRetornoCritica),
                                                    NumTotEnviados = x.Sum(y => y.NumTotEnviados),
                                                    NumTotEnvRetornoOk = x.Sum(y => y.NumTotEnvRetornoOk),
                                                    NumTotEnvAguardRetorno = x.Sum(y => y.NumTotEnvAguardRetorno),
                                                    NumTotEnvSemRecibo = x.Sum(y => y.NumTotEnvSemRecibo),
                                                    NumTotRetificados = x.Sum(y => y.NumTotRetificados),
                                                    NumTotExcluidos = x.Sum(y => y.NumTotExcluidos),
                                                    NumSlaNaoAnalisado = x.Sum(y => y.NumSlaNaoAnalisado),
                                                    NumSlaNaoIniciados = x.Sum(y => y.NumSlaNaoIniciados),
                                                    NumSlaRascunhoEsc = x.Sum(y => y.NumSlaRascunhoEsc),
                                                    NumSlaRascunhoCon = x.Sum(y => y.NumSlaRascunhoCon),
                                                    NumSlaRascunhoEnv = x.Sum(y => y.NumSlaRascunhoEnv),
                                                    NumSlaAguardRetorno = x.Sum(y => y.NumSlaAguardRetorno),
                                                    NumSlaRetornoCritica = x.Sum(y => y.NumSlaRetornoCritica),
                                                    NumSlaSemRecibo = x.Sum(y => y.NumSlaSemRecibo),
                                                    NumSlaVencidos = x.Sum(y => y.NumSlaVencidos),
                                                    NumSlaVencendo = x.Sum(y => y.NumSlaVencendo),
                                                    TipoPrazoSlaVencendo = ((ESocialTipoPrazoSlaVencendo)x.Select(y => y.TipoPrazoSlaVencendo).FirstOrDefault()).ToDescription(),
                                                    DatUltimaAtualizacao = x.Select(y => y.DatUltimaAtualizacao).FirstOrDefault()
                                                });

            return await query.FirstOrDefaultAsync(ct);
        }

        public async Task<int> ObetemQuantidadeErrosEnvio(bool naoFiltrarEmpresas, List<int> filtroEmpresas, bool naoFiltrarEstados, List<string> filtroEstados, CancellationToken ct)
        {
            var listaRetorno = await _eSocialDbContext.VEsAcompanhamento
                .Where(x =>
                    x.StatusFormulario == EsocialStatusFormulario.ErroProcessamento.ToByte()).ToListAsync(ct);


            listaRetorno = listaRetorno.Where(x => (naoFiltrarEmpresas || filtroEmpresas.Contains(Convert.ToInt32(x.IdEsEmpresaAgrupadora)))
                                                   && (naoFiltrarEstados || filtroEstados.Contains(x.CodEstado))).ToList();


            return listaRetorno.Count;
            //return query.Count<VEsAcompanhamento>();
        }

        public async Task<List<RetornoListaDefaultDTO>?> ObtemFiltroEmpresaAgrupadora(string loginUsuario, CancellationToken ct)
        {
            var usuarioEmpresaDoGrupo = _eSocialDbContext.AcaUsuarioEmpresaGrupo.AsNoTracking().Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodParte);

            if (usuarioEmpresaDoGrupo.Any())
            {
                var listaEnum = await (from e in _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking()
                                       join p in _eSocialDbContext.Parte on e.IdEsEmpresaAgrupadora equals p.IdEsEmpresaAgrupadora
                                       where usuarioEmpresaDoGrupo.Contains(p.CodParte)
                                       select new RetornoListaDefaultDTO()
                                       {
                                           Id = e.IdEsEmpresaAgrupadora,
                                           Descricao = e.NomEmpresaAgrupadora
                                       }).Distinct().ToListAsync(ct);
                return listaEnum;
            }
            else
            {
                var listaEnum = await (from e in _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking()
                                       select new RetornoListaDefaultDTO()
                                       {
                                           Id = e.IdEsEmpresaAgrupadora,
                                           Descricao = e.NomEmpresaAgrupadora,
                                           Default = e.IdEsEmpresaAgrupadora == 1
                                       }).ToListAsync(ct);
                return listaEnum;
            }
        }

        public async Task<List<RetornoListaUFDTO>?> ObtemFiltroUF(CancellationToken ct)
        {
            var _listaEstado = await _eSocialDbContext.Estado
                    .Select(e => new RetornoListaUFDTO()
                    {
                        Id = e.CodEstado,
                        Descricao = e.NomEstado
                        //string.Format("{0} - {1}", e.CodEstado, e.NomEstado)
                    }).OrderBy(e => e.Id).ToListAsync(ct);

            return _listaEstado;
        }
    }
}
