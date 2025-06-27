using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data;
using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Entities;
using Oi.Juridico.Shared.V2.Helpers;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.RequestDTOs;
using Oi.Juridico.WebApi.V2.Services;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.Services
{
    public class AgendaAudienciaService
    {
        private readonly AgendaAudienciaContext _agendaDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public AgendaAudienciaService(AgendaAudienciaContext agendaDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _agendaDbContext = agendaDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        public void PreencheEntidadeAgendaPreposto(AgendaPrepostoRequestDTO requestDTO, ref ReclamadaPrepostoAudiencia? reclamadaPrepostoAudiencia, ClaimsPrincipal? user)
        {
            if (reclamadaPrepostoAudiencia is null)
            {
                reclamadaPrepostoAudiencia = new ReclamadaPrepostoAudiencia();
            }

            reclamadaPrepostoAudiencia!.CodProcesso = requestDTO.CodProcesso;
            reclamadaPrepostoAudiencia!.SeqAudiencia = requestDTO.SeqAudiencia;
            reclamadaPrepostoAudiencia!.CodParte = requestDTO.CodParte;

            reclamadaPrepostoAudiencia!.CodUsuarioUltAlteracao = user!.Identity!.Name;
            reclamadaPrepostoAudiencia!.DatUltAtualizacao = DateTime.Now;

            reclamadaPrepostoAudiencia!.CodPreposto = requestDTO.CodPreposto;
            reclamadaPrepostoAudiencia!.DatAudiencia = requestDTO.DatAudiencia;

        }

        public async Task<RetornoPaginadoDTO<VAgendaTrabalhistaDTO>> ConsultarAgenda(AgendaTrabalhistaRequestDTO requestDTO, int? ordenarPor, int? pagina, int? quantidade, RetornoPaginadoDTO<VAgendaTrabalhistaDTO> resultado, CancellationToken ct)
        {
            _agendaDbContext.PesquisarPorCaseInsensitive();

            var queryAgenda = from agenda in _agendaDbContext.VAgendaTrabalhista.AsNoTracking().Where(requestDTO.BuildFilter())
                              join estado in _agendaDbContext.Estado.Select(e => new { e.CodEstado, e.NomEstado }) on agenda.Estado equals estado.CodEstado into groupEstado
                              from estado in groupEstado.DefaultIfEmpty()
                              select new VAgendaTrabalhistaDTO
                              {
                                  ClassificacaoHierarquica = agenda.ClassificacaoHierarquica,
                                  CodProcesso = agenda.CodProcesso,
                                  CodTipoVara = agenda.CodTipoVara,
                                  CodVara = agenda.CodVara,
                                  Comarca = agenda.Comarca,
                                  DataUltAtualizacao = agenda.DataUltAtualizacao,
                                  DateAudiencia = agenda.DateAudiencia,
                                  EscritorioProcesso = agenda.EscritorioProcesso,
                                  Estado = agenda.Estado,
                                  HorarioAudiencia = agenda.HorarioAudiencia,
                                  NumeroProcesso = agenda.NumeroProcesso,
                                  ProcessoAtivo = agenda.ProcessoAtivo,
                                  TipoAudiencia = agenda.TipoAudiencia,
                                  TipoVara = agenda.TipoVara,
                                  UsuarioUltAteracao = agenda.UsuarioUltAteracao,
                                  Estrategico = agenda.Estrategico,
                                  ClassificacaoProcesso = agenda.ClassificacaoProcesso,
                                  SeqAudiencia = agenda.SeqAudiencia,
                                  NomEstado = estado.NomEstado,
                                  DescModalidade = agenda.ModalidadeAtivo == "S" ? agenda.DescModalidade : !string.IsNullOrEmpty(agenda.DescModalidade) ? $"{agenda.DescModalidade} [INATIVO]" : "",
                                  DescLocalidade = agenda.LocalidadeAtivo == "S" ? agenda.DescLocalidade : !string.IsNullOrEmpty(agenda.DescLocalidade) ? $"{agenda.DescLocalidade} [INATIVO]" : "",
                              };

            var listaAgendaTrabalhista = await queryAgenda.ToListAsync(ct);


            var _listaEstado = await BuscaListaEstados(listaAgendaTrabalhista);
            var totalGeral = listaAgendaTrabalhista.Count;


            if (listaAgendaTrabalhista is not null && listaAgendaTrabalhista.Count > 0)
            {
                var listaFiltrada = listaAgendaTrabalhista!.Where(x => string.IsNullOrEmpty(requestDTO.EstadoSelecionado) ? x.Estado == _listaEstado.FirstOrDefault()!.Id : x.Estado == requestDTO.EstadoSelecionado);

                listaAgendaTrabalhista = ordenarPor.HasValue && ordenarPor == 1 ? [.. listaFiltrada.OrderBy(x => x.DateAudiencia).ThenBy(x => x.Comarca).ThenBy(x => x.CodVara).ThenBy(x => x.HorarioAudiencia)]
                                                                                : [.. listaFiltrada.OrderBy(x => x.DateAudiencia).ThenBy(x => x.HorarioAudiencia)];
            }

            foreach (var agenda in listaAgendaTrabalhista!)
            {
                agenda.Reclamantes = await ObterParteProcessoReclamantes(agenda.CodProcesso, ct);
                agenda.Reclamadas = await ObterParteProcessoReclamadas(agenda.CodProcesso, agenda.SeqAudiencia, ct);

                DateTime novaDataHora = new DateTime(agenda.DateAudiencia!.Value.Year, agenda.DateAudiencia!.Value.Month, agenda.DateAudiencia!.Value.Day, agenda.HorarioAudiencia!.Value.Hour, agenda.HorarioAudiencia!.Value.Minute, agenda.HorarioAudiencia!.Value.Second);
                agenda.HorarioAudiencia = novaDataHora;
            }


            var total = listaAgendaTrabalhista.Count;
            var skip = PaginationHelper.PagesToSkip(quantidade!.Value, total, pagina!.Value);

            resultado = new RetornoPaginadoDTO<VAgendaTrabalhistaDTO>
            {
                Total = total,
                TotalGeral = totalGeral,
                Lista = listaAgendaTrabalhista != null ? listaAgendaTrabalhista.Skip(skip).Take(quantidade!.Value) : new List<VAgendaTrabalhistaDTO> { },
                ListaEstados = _listaEstado.Any() ? _listaEstado : new List<RetornoListaUfDTO>()
            };

            return resultado;
        }

        public async Task<RetornoPaginadoDTO<VAgendaTrabalhistaDTO>> ConsultarAgendaPorEstado(AgendaTrabalhistaRequestDTO requestDTO, int? ordenarPor, int? pagina, int? quantidade, RetornoPaginadoDTO<VAgendaTrabalhistaDTO> resultado, CancellationToken ct)
        {

            _agendaDbContext.PesquisarPorCaseInsensitive();

            var queryAgenda = from agenda in _agendaDbContext.VAgendaTrabalhista.AsNoTracking().Where(requestDTO.BuildFilter())
                              join estado in _agendaDbContext.Estado.Select(e => new { e.CodEstado, e.NomEstado }) on agenda.Estado equals estado.CodEstado into groupEstado
                              from estado in groupEstado.DefaultIfEmpty()
                              where agenda.Estado == requestDTO.EstadoSelecionado
                              select new VAgendaTrabalhistaDTO
                              {
                                  ClassificacaoHierarquica = agenda.ClassificacaoHierarquica,
                                  CodProcesso = agenda.CodProcesso,
                                  CodTipoVara = agenda.CodTipoVara,
                                  CodVara = agenda.CodVara,
                                  Comarca = agenda.Comarca,
                                  DataUltAtualizacao = agenda.DataUltAtualizacao,
                                  DateAudiencia = agenda.DateAudiencia,
                                  EscritorioProcesso = agenda.EscritorioProcesso,
                                  Estado = agenda.Estado,
                                  HorarioAudiencia = agenda.HorarioAudiencia,
                                  NumeroProcesso = agenda.NumeroProcesso,
                                  ProcessoAtivo = agenda.ProcessoAtivo,
                                  TipoAudiencia = agenda.TipoAudiencia,
                                  TipoVara = agenda.TipoVara,
                                  UsuarioUltAteracao = agenda.UsuarioUltAteracao,
                                  Estrategico = agenda.Estrategico,
                                  ClassificacaoProcesso = agenda.ClassificacaoProcesso,
                                  SeqAudiencia = agenda.SeqAudiencia,
                                  NomEstado = estado.NomEstado,
                                  DescModalidade = agenda.ModalidadeAtivo == "S" ? agenda.DescModalidade : !string.IsNullOrEmpty(agenda.DescModalidade) ? $"{agenda.DescModalidade} [INATIVO]" : "",
                                  DescLocalidade = agenda.LocalidadeAtivo == "S" ? agenda.DescLocalidade : !string.IsNullOrEmpty(agenda.DescLocalidade) ? $"{agenda.DescLocalidade} [INATIVO]" : "",
                              };

            queryAgenda = ordenarPor.HasValue && ordenarPor == 1 ? queryAgenda.OrderBy(x => x.DateAudiencia).ThenBy(x => x.Comarca).ThenBy(x => x.CodVara).ThenBy(x => x.HorarioAudiencia)
                                                                 : queryAgenda.OrderBy(x => x.DateAudiencia).ThenBy(x => x.HorarioAudiencia);

            var listaAgendaTrabalhista = await queryAgenda.ToListAsync(ct);

            var total = listaAgendaTrabalhista.Count;
            var pagesToSkip = PaginationHelper.PagesToSkip(quantidade!.Value, total, pagina!.Value);


            listaAgendaTrabalhista = listaAgendaTrabalhista.Skip(pagesToSkip).Take(quantidade!.Value).ToList();


            if (listaAgendaTrabalhista != null)
            {

                foreach (var agenda in listaAgendaTrabalhista!)
                {
                    agenda.Reclamantes = await ObterParteProcessoReclamantes(agenda.CodProcesso, ct);
                    agenda.Reclamadas = await ObterParteProcessoReclamadas(agenda.CodProcesso, agenda.SeqAudiencia, ct);

                    DateTime novaDataHora = new DateTime(agenda.DateAudiencia!.Value.Year, agenda.DateAudiencia!.Value.Month, agenda.DateAudiencia!.Value.Day, agenda.HorarioAudiencia!.Value.Hour, agenda.HorarioAudiencia!.Value.Minute, agenda.HorarioAudiencia!.Value.Second);
                    agenda.HorarioAudiencia = novaDataHora;
                }
            }

            resultado = new RetornoPaginadoDTO<VAgendaTrabalhistaDTO>
            {
                Total = total,
                Lista = listaAgendaTrabalhista != null ? listaAgendaTrabalhista : new List<VAgendaTrabalhistaDTO> { }
            };

            return resultado;
        }
        public async Task<List<VAgendaTrabalhistaExportarDTO>> ConsultaExportarAgenda(AgendaTrabalhistaRequestDTO requestDTO, int? ordenarPor, List<VAgendaTrabalhistaExportarDTO> resultado, CancellationToken ct)
        {
            _agendaDbContext.PesquisarPorCaseInsensitive();


            var queryAgenda = from a in _agendaDbContext.VAgendaTrabalhista.AsNoTracking().Where(requestDTO.BuildFilter())
                              join pp in _agendaDbContext.ParteProcesso on a.CodProcesso equals pp.CodProcesso
                              where pp.CodTipoParticipacao == 4
                              join p in _agendaDbContext.Parte.AsNoTracking() on pp.CodParte equals p.CodParte
                              join e in _agendaDbContext.Estado.AsNoTracking() on a.Estado equals e.CodEstado
                              join rpa in _agendaDbContext.ReclamadaPrepostoAudiencia.AsNoTracking()
                                  on new { x1 = p.CodParte, x2 = a.CodProcesso, x3 = a.SeqAudiencia } equals new { x1 = rpa.CodParte, x2 = rpa.CodProcesso, x3 = rpa.SeqAudiencia } into LeftJoinReclamadaAudiencia
                              from rpa in LeftJoinReclamadaAudiencia.DefaultIfEmpty()
                              join pt in _agendaDbContext.Preposto on rpa.CodPreposto equals pt.CodPreposto into LeftPreposto
                              from pt in LeftPreposto.DefaultIfEmpty()
                              select new VAgendaTrabalhistaExportarDTO()
                              {
                                  ClassificacaoHierarquica = a.ClassificacaoHierarquica,
                                  CodProcesso = a.CodProcesso,
                                  CodTipoVara = a.CodTipoVara,
                                  CodVara = a.CodVara,
                                  Comarca = a.Comarca,
                                  DataUltAtualizacao = a.DataUltAtualizacao,
                                  DateAudiencia = a.DateAudiencia,
                                  EscritorioProcesso = a.EscritorioProcesso,
                                  Estado = a.Estado,
                                  HorarioAudiencia = a.HorarioAudiencia,
                                  NumeroProcesso = a.NumeroProcesso,
                                  ProcessoAtivo = a.ProcessoAtivo,
                                  TipoAudiencia = a.TipoAudiencia,
                                  TipoVara = a.TipoVara,
                                  UsuarioUltAteracao = a.UsuarioUltAteracao,
                                  Estrategico = a.Estrategico,
                                  ClassificacaoProcesso = a.ClassificacaoProcesso,
                                  SeqAudiencia = a.SeqAudiencia,
                                  Reclamadas = p.NomParte,
                                  Preposto = pt.NomPreposto,
                                  DescricaoEstado = $"{a.Estado} - {e.NomEstado}",
                                  DescModalidade = a.ModalidadeAtivo == "S" ? a.DescModalidade : !string.IsNullOrEmpty(a.DescModalidade) ? $"{a.DescModalidade} [INATIVO]" : "",
                                  DescLocalidade = a.LocalidadeAtivo == "S" ? a.DescLocalidade : !string.IsNullOrEmpty(a.DescLocalidade) ? $"{a.DescLocalidade} [INATIVO]" : "",
                                  Link = a.Link,
                              };


            queryAgenda = ordenarPor.HasValue && ordenarPor == 1 ? queryAgenda.OrderBy(x => x.Estado).ThenBy(x => x.DateAudiencia).ThenBy(x => x.Comarca).ThenBy(x => x.CodVara).ThenBy(x => x.HorarioAudiencia)
                                                    : queryAgenda.OrderBy(x => x.Estado).ThenBy(x => x.DateAudiencia).ThenBy(x => x.HorarioAudiencia);

            var listaAgendaTrabalhista = await queryAgenda.ToListAsync(ct);


            foreach (var agenda in listaAgendaTrabalhista!)
            {
                agenda.Reclamantes = await ObterParteProcessoReclamantes(agenda.CodProcesso, ct);

                DateTime novaDataHora = new DateTime(agenda.DateAudiencia!.Value.Year, agenda.DateAudiencia!.Value.Month, agenda.DateAudiencia!.Value.Day, agenda.HorarioAudiencia!.Value.Hour, agenda.HorarioAudiencia!.Value.Minute, agenda.HorarioAudiencia!.Value.Second);
                agenda.HorarioAudiencia = novaDataHora;
            }

            resultado = listaAgendaTrabalhista;


            return resultado!;
        }

        public async Task<List<int?>> ConsultarAgendaPorEstadoPreposto(AgendaTrabalhistaRequestDTO requestDTO, CancellationToken ct)
        {

            _agendaDbContext.PesquisarPorCaseInsensitive();

            #region preenche datas se vazias

            if (!requestDTO.DataAudienciaDe.HasValue)
            {
                DateTime hoje = DateTime.Today;

                DateTime segundaProximaSemana = hoje.AddDays(7 - (int)hoje.DayOfWeek + (int)DayOfWeek.Monday);

                DateTime sextaProximaSemana = segundaProximaSemana.AddDays(4);

                requestDTO.DataAudienciaDe = segundaProximaSemana;
                requestDTO.DataAudienciaAte = sextaProximaSemana;
            }
            #endregion

            var queryAgenda = from agenda in _agendaDbContext.VAgendaTrabalhista.AsNoTracking().Where(requestDTO.BuildFilter())
                              join pp in _agendaDbContext.ParteProcesso on agenda.CodProcesso equals pp.CodProcesso
                              where pp.CodTipoParticipacao == 4
                              join p in _agendaDbContext.Parte.AsNoTracking() on pp.CodParte equals p.CodParte
                              join e in _agendaDbContext.Estado.AsNoTracking() on agenda.Estado equals e.CodEstado
                              join rpa in _agendaDbContext.ReclamadaPrepostoAudiencia.AsNoTracking()
                                  on new { x1 = p.CodParte, x2 = agenda.CodProcesso, x3 = agenda.SeqAudiencia } equals new { x1 = rpa.CodParte, x2 = rpa.CodProcesso, x3 = rpa.SeqAudiencia } into LeftJoinReclamadaAudiencia
                              from rpa in LeftJoinReclamadaAudiencia.DefaultIfEmpty()
                              select rpa.CodPreposto;

            var listaAgendaTrabalhista = await queryAgenda.Distinct().ToListAsync(ct);

            return listaAgendaTrabalhista;
        }


        #region Busca Listas
        public async Task<List<RetornoAgendaDTO>> ConsultaModalidadeAudiencia(CancellationToken ct)
            {
                var _listaModalidade = await _agendaDbContext.ModalidadeAudiencia
                    .Select(e => new RetornoAgendaDTO()
                    {
                        Id = e.CodModalidadeAudiencia,
                        Descricao = e.IndAtivo == "S" ? e.DscModalidadeAudiencia : $"{e.DscModalidadeAudiencia} [INATIVO]" 

                    }).OrderBy(e => e.Id).ToListAsync(ct);

                return _listaModalidade;
            }

            public async Task<List<RetornoAgendaDTO>> ConsultaLocalidaeAudiencia(CancellationToken ct)
            {
                var _listaModalidade = await _agendaDbContext.LocalidadeAudiencia
                    .Select(e => new RetornoAgendaDTO()
                    {
                        Id = e.CodLocalidadeAudiencia,
                        Descricao = e.IndAtivo == "S" ? e.DscLocalidadeAudiencia : $"{e.DscLocalidadeAudiencia} [INATIVO]"

                    }).OrderBy(e => e.Id).ToListAsync(ct);

                return _listaModalidade;
            }
        #endregion

        #region Métodos privados

        private async Task<string> ObterParteProcessoReclamantes(int? codProcesso, CancellationToken ct)
        {

            var _listaReclamante = await (from p in _agendaDbContext.Parte.AsNoTracking()
                                          join pp in _agendaDbContext.ParteProcesso.AsNoTracking() on p.CodParte equals pp.CodParte
                                          where pp.CodTipoParticipacao == 3 && pp.CodProcesso == codProcesso
                                          select new RetornoParteDTO()
                                          {
                                              Id = p.CodParte,
                                              Descricao = p.NomParte

                                          }).OrderBy(o => o.Descricao).ToListAsync(ct);

            var reclamantes = string.Join(", ", _listaReclamante.Select(x => x.Descricao).ToArray());


            return reclamantes;
        }
        private async Task<IEnumerable<RetornoParteDTO>> ObterParteProcessoReclamadas(int? codProcesso, int? seqAudiencia, CancellationToken ct)
        {

            var _listaReclamada = await (from p in _agendaDbContext.Parte.AsNoTracking()
                                         join pp in _agendaDbContext.ParteProcesso.AsNoTracking() on p.CodParte equals pp.CodParte
                                         join au in _agendaDbContext.AudienciaProcesso on pp.CodProcesso equals au.CodProcesso
                                         join rpa in _agendaDbContext.ReclamadaPrepostoAudiencia on new { x1 = p.CodParte, x2 = au.CodProcesso, x3 = au.SeqAudiencia } equals new { x1 = rpa.CodParte, x2 = rpa.CodProcesso, x3 = rpa.SeqAudiencia } into LeftJoinReclamadaAudiencia
                                         from rpa in LeftJoinReclamadaAudiencia.DefaultIfEmpty()
                                         join pt in _agendaDbContext.Preposto on rpa.CodPreposto equals pt.CodPreposto into LeftPreposto
                                         from pt in LeftPreposto.DefaultIfEmpty()
                                         where pp.CodTipoParticipacao == 4 && au.CodProcesso == codProcesso && au.SeqAudiencia == seqAudiencia
                                         select new RetornoParteDTO()
                                         {
                                             Id = p.CodParte,
                                             Descricao = p.NomParte,
                                             CodPreposto = pt.CodPreposto

                                         }).OrderBy(o => o.Descricao).ToListAsync(ct);


            return _listaReclamada;
        }
        private async Task<List<RetornoListaUfDTO>> BuscaListaEstados(List<VAgendaTrabalhistaDTO> listaAgenda)
        {
            //var listaEstadoAgrupado = (from lista in listaAgenda
            //group lista by new { lista.Estado } into l
            //             select new {codEstado = l.Key.Estado, qtd = l.Count() }).ToList();

            var listaEstado = (from lista in listaAgenda.DistinctBy(x => x.Estado)
                               join estadoGrupo in (from lista in listaAgenda
                                                    group lista by new { lista.Estado } into l
                                                    select new { codEstado = l.Key.Estado, qtd = l.Count() })
                               on lista.Estado equals estadoGrupo.codEstado
                               select new RetornoListaUfDTO()
                               {
                                   Id = lista!.Estado!,
                                   Descricao = lista!.NomEstado!.ToUpper(),
                                   IdDescricao = $"{lista!.Estado!.ToUpper()} - {lista.NomEstado.ToUpper()} ({estadoGrupo.qtd} audiência{(estadoGrupo.qtd > 1 ? "s" : string.Empty)})"

                               }).OrderBy(e => e.Descricao).ToList();

            return listaEstado;
        }

        #endregion

    }
}
