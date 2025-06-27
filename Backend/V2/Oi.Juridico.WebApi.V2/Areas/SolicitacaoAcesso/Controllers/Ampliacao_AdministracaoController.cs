using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.SolicitacaoAcessoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Helpers;
using System.Threading;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.Contextos.V2.Extensions;
using Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.DTOs.Ampliacao_Administracao.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.DTOs.Ampliacao_Administracao;
using System.Numerics;
using Oi.Juridico.Contextos.V2.PerfilContext.Data;

namespace Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.Controllers
{
    /// <summary>
    /// Api SolicitacaoAcesso Resposável pelo controle de solicitação de acessos.
    /// </summary>
    [Route("SolicitacaoAcesso/[controller]")]
    [ApiController]
    public class Ampliacao_AdministracaoController : ControllerBase
    {
        private readonly SolicitacaoAcessoContext _dbSolicitacao;
        private readonly PerfilContext _dbPerfil;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        public Ampliacao_AdministracaoController(SolicitacaoAcessoContext dbSolicitacao, PerfilContext dbPerfil)
        {
            _dbSolicitacao = dbSolicitacao;
            _dbPerfil = dbPerfil;
        }

        #region Métodos Publicos

        /// <summary></summary>
        /// <returns></returns>
        [HttpPost, Route("Count")]
        public async Task<ActionResult<int>> CountAsync(FiltroAdministracaoSolicitacaoAmpliacao filtro, CancellationToken ct)
        {
            var listaDeSolicitacao = ListaDeAcaSolicitacaoAcessoAmpliacao(filtro, TipoSolicitacaoAcessoEnum.Ampliacao);

            return Ok(await listaDeSolicitacao.CountAsync(ct));
        }

        /// <summary></summary>
        /// <returns></returns>
        [HttpPost, Route("Pesquisa")]
        [PascalCase]
        public async Task<IActionResult> PesquisaAsync(FiltroAdministracaoSolicitacaoAmpliacao filtro, CancellationToken ct)
        {
            var take = 10;
            var skip = take * (filtro.Pagina - 1);

            var listaDeSolicitacao = ListaDeAcaSolicitacaoAcessoAmpliacao(filtro, TipoSolicitacaoAcessoEnum.Ampliacao);

            var queryEscritorios = _dbSolicitacao.VEscritorios.AsNoTracking().Select(x => new { x.CodProfissional, x.NomProfissional });

            var solicitacaoAcesso = await (from solicitaca2_ in _dbSolicitacao.AcaSolicitacaoAcesso.AsNoTracking()
                                           join usuario7_ in _dbSolicitacao.AcaUsuario on solicitaca2_.CodUsuarioSolicitacao equals usuario7_.CodUsuario into leftJoinUsuario7
                                           from usuario7_ in leftJoinUsuario7.DefaultIfEmpty()
                                           where listaDeSolicitacao.Contains(solicitaca2_.Id)
                                           select new PesquisaResponse
                                           {
                                               UsuarioSolicitante = usuario7_.NomeUsuario + " - " + usuario7_.CodUsuario,
                                               UsuarioSolicitado = solicitaca2_.NomeUsuario,
                                               DataSolicitacao = solicitaca2_.DataSolicitacaoAcesso,
                                               Id = (int)solicitaca2_.Id,
                                               Status = solicitaca2_.CodStatusSolicitacao!.Value,
                                               CodProfissional = usuario7_.AcaUsuarioEscritorio.Select(x => x.CodProfissional).ToList(),
                                           })
                                           .OrderBy(x => x.DataSolicitacao)
                                           .Skip(skip)
                                           .Take(take)
                                           .ToListAsync(ct);

            var gestores = await _dbSolicitacao.SolAprovacaoPerfil
                .AsNoTracking()
                .Where(x => solicitacaoAcesso.Select(s => s.Id).Contains((int)x.IdSolicitacaoAcesso))
                .Select(x => new { x.IdSolicitacaoAcesso, x.CodUsuarioGestorNavigation.NomeUsuario })
                .Distinct()
                .ToArrayAsync(ct);

            var administradores = await _dbSolicitacao.SolAprovacaoPerfil
                .AsNoTracking()
                .Where(x => solicitacaoAcesso.Select(s => s.Id).Contains((int)x.IdSolicitacaoAcesso))
                .Select(x => new { x.IdSolicitacaoAcesso, x.CodUsuarioAdministrador, x.CodUsuarioAdministradorNavigation.NomeUsuario })
                .ToArrayAsync(ct);

            foreach (var item in solicitacaoAcesso)
            {
                ct.ThrowIfCancellationRequested();
                item.UsuarioAdministrador = administradores.Where(x => x.IdSolicitacaoAcesso == item.Id).Select(x => x.NomeUsuario + " - " + x.CodUsuarioAdministrador).FirstOrDefault() ?? "";
                item.Gestores = string.Join(", ", gestores.Where(x => x.IdSolicitacaoAcesso == item.Id).Select(x => x.NomeUsuario).Distinct().OrderBy(x => x));
                if (item.CodProfissional?.Any() ?? false)
                {
                    item.Escritorios = string.Join(", ", await queryEscritorios.Where(e => item.CodProfissional.Contains(e.CodProfissional!.Value)).Select(e => e.NomProfissional).ToArrayAsync(ct));
                }
            }

            return Ok(solicitacaoAcesso);
        }

        /// <summary>Realiza o download do Csv de Solicitação</summary>
        /// <returns>Arquivo gerado</returns>
        [HttpPost, Route("DownloadCsv/{tipoCsv:int}")]
        public async Task<IActionResult> DownloadCsvAsync(TipoDeCsvEnum tipoCsv, FiltroAdministracaoSolicitacaoAmpliacao filtro, CancellationToken ct)
        {
            var lista = new List<DownloadCsvResponse>();
            var listaDeSolicitacao = ListaDeAcaSolicitacaoAcessoAmpliacao(filtro, TipoSolicitacaoAcessoEnum.Ampliacao);

            lista.AddRange(await ObterPorSolicitacaoAcessoDownloadAsync(listaDeSolicitacao, ct));

            var dt = DateTime.Now;
            var random = new Random(DateTime.Now.Millisecond);
            var nomeDoArquivo = $"relatorio_juridico_ampliacao_usuario_{dt:yyyy_MM_dd}_{dt.Hour}h{dt.Minute}m{dt.Second}s_{random.Next(1, 9999):0000}.csv";
            var header = ">>> SOLICITAÇÕES DE AMPLIAÇÃO ENCONTRADAS";

            if (tipoCsv == TipoDeCsvEnum.PontoVirgulaApenas)
            {
                var csv = lista.ToCsvByteArray(typeof(DownloadCsvResponseMap), header: header);
                return base.File(csv, "application/csv", nomeDoArquivo);
            }
            else
            {
                var csv = lista.ToCsvByteArray(typeof(DownloadCsvResponseMap), delimiter: ",", textoComAspas: true, header: header);
                return base.File(csv, "application/csv", nomeDoArquivo);
            }
        }

        #endregion

        #region Métodos Internos

        private async Task<List<DownloadCsvResponse>> ObterPorSolicitacaoAcessoDownloadAsync(IQueryable<decimal> solicitacoesId, CancellationToken ct)
        {
            var escritorios = await _dbSolicitacao.VEscritorios.AsNoTracking().Select(x => new { x.CodProfissional, x.NomProfissional }).ToArrayAsync(ct);

            var solicitacaoAcesso = await (from this_ in _dbSolicitacao.SolAprovacaoPerfil.AsNoTracking()
                                           join solicitaca2_ in _dbSolicitacao.AcaSolicitacaoAcesso on this_.IdSolicitacaoAcesso equals solicitaca2_.Id into leftJoinSolicitacao
                                           from solicitaca2_ in leftJoinSolicitacao.DefaultIfEmpty()
                                           join usuario6_ in _dbSolicitacao.AcaUsuario on this_.CodUsuarioAdministrador equals usuario6_.CodUsuario into leftJoinUsuario6
                                           from usuario6_ in leftJoinUsuario6.DefaultIfEmpty()
                                           join usuario7_ in _dbSolicitacao.AcaUsuario on solicitaca2_.CodUsuarioSolicitacao equals usuario7_.CodUsuario into leftJoinUsuario7
                                           from usuario7_ in leftJoinUsuario7.DefaultIfEmpty()
                                           join usuario8_ in _dbSolicitacao.AcaUsuario on this_.CodUsuarioGestor equals usuario8_.CodUsuario into leftJoinUsuario8
                                           from usuario8_ in leftJoinUsuario8.DefaultIfEmpty()
                                           join usuario9_ in _dbSolicitacao.AcaUsuario on this_.CodPerfil equals usuario9_.CodUsuario into leftJoinUsuario9
                                           from usuario9_ in leftJoinUsuario9.DefaultIfEmpty()
                                           join origem in _dbSolicitacao.AcaOrigemUsuario on usuario7_.CodOrigemUsuario equals origem.CodOrigemUsuario into leftJoinOrigem
                                           from origem in leftJoinOrigem.DefaultIfEmpty()
                                           where solicitacoesId.Contains(this_.IdSolicitacaoAcesso!.Value)
                                           select new DownloadCsvResponse
                                           {
                                               UsuarioSolicitante = usuario7_.NomeUsuario,
                                               UsuarioSolicitado = solicitaca2_.NomeUsuario,
                                               Login = usuario7_.CodUsuario,
                                               Email = usuario7_.DscEmail,
                                               Origem = origem.NomOrigemUsuario,
                                               SituacaoOrigem = usuario7_.CodSituacaoUsuario,
                                               DataHoraSolicitacao = solicitaca2_.DataSolicitacaoAcesso.ToString("dd/MM/yyyy HH:mm:ss"),
                                               StatusRenovacaoAcesso = solicitaca2_.IdStatusRenovacaoAcessoNavigation.Id == 5 ? "Ampliação Realizada" : solicitaca2_.IdStatusRenovacaoAcessoNavigation.Descricao,
                                               StatusDaSolicitacaoDeAcessoEnum = solicitaca2_.CodStatusSolicitacao!.ToString() ?? "",
                                               AdministradorAprovador = new[] { (decimal)StatusRenovacaoDeAcessoEnum.RenovacaoRealizada, (decimal)StatusRenovacaoDeAcessoEnum.NegadaAdministrador }.Contains(solicitaca2_.IdStatusRenovacaoAcessoNavigation.Id) ? usuario6_.NomeUsuario : "",
                                               DataHoraReativacao = this_.DatAcaoAdministrador.HasValue ? this_.DatAcaoAdministrador!.Value.ToString("dd/MM/yyyy HH:mm:ss") : this_.DatAcaoGestor.HasValue ? this_.DatAcaoGestor!.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                               DataValidadeSenha = usuario7_.DatValidadeSenha.HasValue ? usuario7_.DatValidadeSenha!.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                               Perfil = usuario9_.NomeUsuario,
                                               CodPerfil = this_.CodPerfil,
                                               GestorResponsavel = this_.CodUsuarioGestor == this_.CodUsuarioSolicitante ? "Gestor default" : usuario8_.NomeUsuario,
                                               Aprovado = !this_.DatAcaoGestor.HasValue ? "" : this_.DatAcaoAdministrador.HasValue ? (this_.IndAprovacaoAdministrador == "S" ? "Sim" : "Não") : this_.DatAcaoGestor.HasValue ? (this_.IndAprovacaoGestor == "S" ? "Sim" : "Não") : "",
                                               DataHora = this_.DatAcaoGestor.HasValue ? this_.DatAcaoGestor!.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                                               Observacao = !this_.DatAcaoGestor.HasValue || string.IsNullOrEmpty(solicitaca2_.Observacao) ? "" : solicitaca2_.Observacao,
                                               DescricaoRejeicao = solicitaca2_.DescricaoRejeicao,
                                               DatAcaoGestor = this_.DatAcaoGestor,
                                               DatAcaoAdministrador = new[] { (decimal)StatusRenovacaoDeAcessoEnum.RenovacaoRealizada, (decimal)StatusRenovacaoDeAcessoEnum.NegadaAdministrador }.Contains(solicitaca2_.IdStatusRenovacaoAcessoNavigation.Id) ? this_.DatAcaoAdministrador : null,
                                               IndAprovacaoGestor = this_.IndAprovacaoGestor,
                                               IndAprovacaoAdministrador = this_.IndAprovacaoAdministrador,
                                               CodProfissional = usuario7_.AcaUsuarioEscritorio.Select(x => x.CodProfissional).ToList(),
                                           }).ToListAsync(ct);

            Dictionary<string, string> NomesPerfisExcluidos = new Dictionary<string, string>();

            foreach (var item in solicitacaoAcesso)
            {
                ct.ThrowIfCancellationRequested();

                if (item.CodProfissional?.Any() ?? false)
                {
                    item.Escritorio = string.Join(", ", escritorios.Where(e => item.CodProfissional.Contains(e.CodProfissional!.Value)).Select(e => e.NomProfissional));
                }

                if(String.IsNullOrEmpty(item.Perfil))
                {
                    if (NomesPerfisExcluidos.ContainsKey(item.CodPerfil))
                    {
                        item.Perfil = NomesPerfisExcluidos[item.CodPerfil];
                    }
                    else
                    {
                        var log = _dbPerfil.AcaLogUsuario.AsNoTracking().Where(l => l.CodUsuario == item.CodPerfil && l.Operacao == "E").FirstOrDefault();

                        if (log != null)
                        {
                            item.Perfil = $"{log.NomeUsuarioA} [Excluído]";
                            NomesPerfisExcluidos.Add(item.CodPerfil, item.Perfil);
                        }
                    }
                }

                if (!item.DatAcaoGestor.HasValue)
                {
                    item.Aprovado = "";
                    item.Observacao = "";
                }
                else if (item.DatAcaoAdministrador.HasValue)
                {
                    if (item.IndAprovacaoAdministrador == "N")
                    {
                        string dataAcao = item.DatAcaoAdministrador.Value.ToString("dd/MM/yyyy HH:mm");
                        //item.Observacao = this.MontaObservacaoValida(item.DescricaoRejeicao, item.AdministradorAprovador, dataAcao, item.Perfil, false).Replace("\n", " | ").Replace("\r", " | ");
                        //item.Aprovado = "Não";
                        var observacao = this.MontaObservacaoValida(item.DescricaoRejeicao!, item.AdministradorAprovador!, dataAcao, item.Perfil, false);
                        if (string.IsNullOrEmpty(observacao) && dataAcao != null && item.Observacao != null && item.GestorResponsavel != null)
                            observacao = this.MontaObservacaoValida(item.Observacao, item.GestorResponsavel, dataAcao, item.Perfil, true);
                        item.Aprovado = "Não";
                        item.Observacao = observacao;
                    }
                    else
                    {
                        item.Aprovado = "Sim";
                        item.Observacao = "";
                    }
                }
                else if (item.DatAcaoGestor.HasValue)
                {
                    if (item.IndAprovacaoGestor == "N")
                    {
                        string dataAcao = item.DatAcaoGestor.Value.ToString("dd/MM/yyyy HH:mm");
                        //item.Observacao = this.MontaObservacaoValida(item.Observacao, item.GestorResponsavel, dataAcao, item.Perfil, true).Replace("\n", " | ").Replace("\r", " | ");
                        var observacao = this.MontaObservacaoValida(item.Observacao, item.GestorResponsavel!, dataAcao, item.Perfil, true);
                        if (string.IsNullOrEmpty(observacao) && dataAcao != null && item.DescricaoRejeicao != null && item.AdministradorAprovador != null)
                            observacao = this.MontaObservacaoValida(item.DescricaoRejeicao, item.AdministradorAprovador, dataAcao, item.Perfil, false);
                        item.Aprovado = "Não";
                    }
                    else
                    {
                        item.Aprovado = "Sim";
                        item.Observacao = "";
                    }
                }
            }

            return solicitacaoAcesso;
        }

        private string MontaObservacaoValida(string observacao, string usuarioAprovador, string dataAprovacaoRejeicao, string perfil, bool tipoAprovador)
        {
            if (string.IsNullOrEmpty(observacao))
                return "";

            string observacaoFinal = "";
            if (!usuarioAprovador.Equals("Gestor default"))
            {
                string[] observacaoLista = observacao.Split('\n');

                foreach (var obs in observacaoLista)
                {
                    if (obs.Contains('+'))
                    {
                        var motivos = obs.Split('[');
                        for (int i = 1; i < motivos.Length; i++)
                        {
                            if (!motivos[i].Equals(" ") && !motivos[i].Equals(""))
                            {
                                string perfisDeMotivoAtual = this.BuscarPerfisDeMotivo(motivos[1]);

                                if (!String.IsNullOrEmpty(perfil))
                                {
                                    var motivoObs = perfil.Replace(" ", "").ToLower();
                                    if (perfisDeMotivoAtual.Contains(motivoObs) && motivos[0].Replace(" ", "").ToLower().Contains(usuarioAprovador.Replace(" ", "").ToLower()))
                                        observacaoFinal = string.Format("{0} |  Solicitação de ampliação de acesso negada pelo(s) {1}(es) em {2}", motivos[0], tipoAprovador ? "Gestor" : "Administrador", dataAprovacaoRejeicao);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (observacaoLista.Length == 1)
                            observacaoFinal = observacaoLista.First();

                        if (observacaoFinal.Equals(""))
                        {
                            foreach (var obsAprovadorData in observacaoLista)
                            {
                                if (!obsAprovadorData.Contains('+') && obsAprovadorData.Contains(usuarioAprovador) && obsAprovadorData.Contains(dataAprovacaoRejeicao))
                                    observacaoFinal += obsAprovadorData + " | ";
                            }
                            if (!observacaoFinal.Equals(""))
                                observacaoFinal += " | " + observacaoLista[observacaoLista.Length - 1];
                        }

                        if (observacaoFinal.Equals(""))
                        {
                            foreach (var obsAprovador in observacaoLista)
                            {
                                if (!obsAprovador.Contains('+') && obsAprovador.Contains(usuarioAprovador))
                                    observacaoFinal += obsAprovador + " | ";
                            }
                            if (!observacaoFinal.Equals(""))
                                observacaoFinal += " | " + observacaoLista[observacaoLista.Length - 1];
                        }

                    }
                }
            }
            return observacaoFinal;
        }

        private string BuscarPerfisDeMotivo(string perfisDeMotivo)
        {
            string[] perfis = perfisDeMotivo.Split('+');
            string finalMotivos = "";
            for (int i = 0; i < perfis.Length; i++)
            {
                if (!perfis[i].Equals(""))
                    finalMotivos += perfis[i].Replace(" ", "").Remove(0, 3).ToLower() + " ";
            }
            return finalMotivos;
        }


        private IQueryable<decimal> ListaDeAcaSolicitacaoAcessoAmpliacao(FiltroAdministracaoSolicitacaoAmpliacao filtros, TipoSolicitacaoAcessoEnum tipo)
        {
            _dbSolicitacao.PesquisarPorCaseInsensitive();

            var acaSolicitacaoAcesso = _dbSolicitacao.AcaSolicitacaoAcesso
                .AsNoTracking()
                .Where(x => x.CodTipoSolicitacaoAcesso.Equals(EnumHelper.GetEnumText(tipo)));

            if (filtros.ListaDeStatus.Any())
            {
                acaSolicitacaoAcesso = acaSolicitacaoAcesso.Where(x => filtros.ListaDeStatus.Contains(x.CodStatusSolicitacao.HasValue ? x.CodStatusSolicitacao!.Value : 0));
            }

            if (filtros.DataSolicitacaoIni.HasValue && filtros.DataSolicitacaoFim.HasValue)
            {
                acaSolicitacaoAcesso = acaSolicitacaoAcesso.Where(x =>
                x.DataSolicitacaoAcesso >= filtros.DataSolicitacaoIni.Value
                && x.DataSolicitacaoAcesso <= filtros.DataSolicitacaoFim.Value.AddDays(1).AddMinutes(-1));
            }

            if (!string.IsNullOrEmpty(filtros.Login))
            {
                acaSolicitacaoAcesso = filtros.TiposDePesquisaEmLogin == TipoPesquisaEnum.Contem ?
                    acaSolicitacaoAcesso.Where(x => x.CodUsuarioSolicitacao.Contains(filtros.Login)) :
                    acaSolicitacaoAcesso.Where(x => x.CodUsuarioSolicitacao.Equals(filtros.Login));

            }

            if (!string.IsNullOrEmpty(filtros.UsuarioSolicitante))
            {
                acaSolicitacaoAcesso = filtros.TiposDePesquisaEmNomeSolicitante == TipoPesquisaEnum.Contem ?
                    acaSolicitacaoAcesso.Where(x => x.CodUsuarioSolicitacaoNavigation.NomeUsuario.Contains(filtros.UsuarioSolicitante)) :
                    acaSolicitacaoAcesso.Where(x => x.CodUsuarioSolicitacaoNavigation.NomeUsuario.Equals(filtros.UsuarioSolicitante));
            }

            if (!string.IsNullOrEmpty(filtros.UsuarioSolicitado))
            {
                acaSolicitacaoAcesso = filtros.TiposDePesquisaEmNomeSolicitado == TipoPesquisaEnum.Contem ?
                    acaSolicitacaoAcesso.Where(x => x.NomeUsuario.Contains(filtros.UsuarioSolicitado)) :
                    acaSolicitacaoAcesso.Where(x => x.NomeUsuario.Equals(filtros.UsuarioSolicitado));
            }

            if (filtros.CodigoEscritorio > 0)
            {
                var usuarioEscritorio = _dbSolicitacao.AcaUsuarioEscritorio.Where(x => x.CodProfissional == filtros.CodigoEscritorio);
                acaSolicitacaoAcesso = acaSolicitacaoAcesso.Where(x => usuarioEscritorio.Any(ue => ue.CodUsuario == x.CodUsuarioSolicitacao));
            }

            return acaSolicitacaoAcesso.Select(x => x.Id);
        }

        #endregion

    }
}
