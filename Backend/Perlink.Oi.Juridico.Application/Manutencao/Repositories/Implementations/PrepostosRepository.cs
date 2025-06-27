using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class PrepostosRepository : IPrepostosRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IPrepostosRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public PrepostosRepository(IDatabaseContext databaseContext, ILogger<IPrepostosRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<bool> EstaAlocado(string tiposProcessos, int prepostoId)
        {
            var listaProcessos = tiposProcessos.Split(',').ToList();

            var civel = listaProcessos.Contains(TipoProcesso.CIVEL_CONSUMIDOR.Id.ToString());
            var estrategico = listaProcessos.Contains(TipoProcesso.CIVEL_ESTRATEGICO.Id.ToString());
            var juizado = listaProcessos.Contains(TipoProcesso.JEC.Id.ToString());
            var procon = listaProcessos.Contains(TipoProcesso.PROCON.Id.ToString());
            var pex = listaProcessos.Contains(TipoProcesso.PEX.Id.ToString());
            var trabalhista = listaProcessos.Contains(TipoProcesso.TRABALHISTA.Id.ToString());

            bool resultado = (from p in DatabaseContext.AudienciasDosProcessos.
                                                           Where(x => x.Preposto.Id == prepostoId &&

                                                                ((juizado == true && x.Processo.TipoProcessoId == TipoProcesso.JEC.Id) ||
                                                                (civel == true && x.Processo.TipoProcessoId == TipoProcesso.CIVEL_CONSUMIDOR.Id) ||
                                                                (estrategico == true && x.Processo.TipoProcessoId == TipoProcesso.CIVEL_ESTRATEGICO.Id) ||
                                                                (procon == true && x.Processo.TipoProcessoId == TipoProcesso.PROCON.Id) ||
                                                                (trabalhista == true && x.Processo.TipoProcessoId == TipoProcesso.TRABALHISTA.Id) ||
                                                                (pex == true && x.Processo.TipoProcessoId == TipoProcesso.PEX.Id))
                                                                && x.DataAudiencia > System.DateTime.Now.Date).AsNoTracking()
                              select p).Any();

            if (!resultado)
            {
                resultado = (from a in DatabaseContext.AlocacoesPrepostos.Where(x => x.PrepostoId == prepostoId).AsNoTracking()//.Where(x => x.PrepostoId == prepostoId && x.DataAlocacao > System.DateTime.Now.Date).AsNoTracking()
                             join c in DatabaseContext.Comarcas on a.ComarcaId equals c.Id
                             join v in DatabaseContext.TiposDeVara on a.TipoVaraId equals v.Id
                             join p in DatabaseContext.Processos on new { a.ComarcaId, a.VaraId, a.TipoVaraId } equals new { p.ComarcaId, p.VaraId, p.TipoVaraId }
                             join ad in DatabaseContext.AudienciasDosProcessos on a.DataAlocacao.Date equals ad.DataAudiencia.Date

                             where a.DataAlocacao > System.DateTime.Now.Date
                                   && ((juizado == true && p.TipoProcessoId == TipoProcesso.JEC.Id) ||
                                      (civel == true && p.TipoProcessoId == TipoProcesso.CIVEL_CONSUMIDOR.Id) ||
                                      (procon == true && p.TipoProcessoId == TipoProcesso.PROCON.Id))
                                   && p.Id == ad.ProcessoId
                             select a).Any();
            }

            return CommandResult<bool>.Valid(resultado);
        }

        private IQueryable<Preposto> ObterBase(PrepostoSort sort, bool ascending, string pesquisa)
        {
            string logName = nameof(Preposto);
            string lInativo = "[Inativo]";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Preposto> query = (from p in DatabaseContext.Prepostos.AsNoTracking()
                                          join u in DatabaseContext.Usuarios on p.UsuarioId equals u.Id into leftOuterJoin
                                          from u in leftOuterJoin.DefaultIfEmpty()
                                          select new Preposto(p.Id,
                                                              p.Nome,
                                                              p.Ativo,
                                                              p.EhCivelEstrategico,
                                                              p.EhCivel,
                                                              p.EhTrabalhista,
                                                              p.EhJuizado,
                                                              p.UsuarioId,
                                                              u.Nome,
                                                              u.Ativo == true ? true : false,
                                                              p.EhProcon,
                                                              p.EhPex,
                                                              p.EhEscritorio,
                                                              p.Matricula));

            switch (sort)
            {
                case PrepostoSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case PrepostoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case PrepostoSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending);
                    break;

                case PrepostoSort.Usuario:
                    query = query.SortBy(a => a.NomeUsuario, ascending);
                    break;

                case PrepostoSort.Escritorio:
                    query = query.SortBy(a => a.EhEscritorio, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }

            if (pesquisa != null)
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(pesquisa.ToUpper()));
                // var x = query.Where(x => x.Nome == "DOGULAS").ToList();
                // query = query.Where(x => EF.Functions.Like(x.Nome.ToUpper(), pesquisa.ToUpper()));
            }

            return query;
            //return query.WhereIfNotNull(x => EF.Functions.Like(x.Nome.ToUpper(), pesquisa.ToUpper()), pesquisa);
            //return query.WhereIfNotNull(x => x.Nome.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Preposto>> ObterPaginado(int pagina, int quantidade, PrepostoSort sort, bool ascending, string pesquisa)
        {
            string logName = nameof(Preposto);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Preposto>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Preposto>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Preposto>>.Valid(resultado);
        }

        private IQueryable<AlocacoesFuturas> ObterBaseAlocacoes(AlocacaoPrepostoSort sort, bool ascending, int prepostoId, string tiposProcessos)
        {
            string logName = nameof(AlocacoesFuturas);

            var listaProcessos = tiposProcessos.Split(',').ToList();

            bool jec = listaProcessos.Contains("7");
            bool civelConsumidor = listaProcessos.Contains("1");
            bool civelEstrategico = listaProcessos.Contains("9");
            bool procon = listaProcessos.Contains("17");
            bool trabalhista = listaProcessos.Contains("2");
            bool pex = listaProcessos.Contains("18");

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<AlocacoesFuturas> queryAudiencia = (from p in DatabaseContext.AudienciasDosProcessos.
                                                           Where(x => x.Preposto.Id == prepostoId &&

                                                                ((jec == true && x.Processo.TipoProcessoId == TipoProcesso.JEC.Id) ||
                                                                (civelConsumidor == true && x.Processo.TipoProcessoId == TipoProcesso.CIVEL_CONSUMIDOR.Id) ||
                                                                (civelEstrategico == true && x.Processo.TipoProcessoId == TipoProcesso.CIVEL_ESTRATEGICO.Id) ||
                                                                (procon == true && x.Processo.TipoProcessoId == TipoProcesso.PROCON.Id) ||
                                                                (trabalhista == true && x.Processo.TipoProcessoId == TipoProcesso.TRABALHISTA.Id) ||
                                                                (pex == true && x.Processo.TipoProcessoId == TipoProcesso.PEX.Id))

                                                                && x.DataAudiencia > System.DateTime.Now.Date).AsNoTracking()
                                                           select new AlocacoesFuturas("Audiência",
                                                                                       p.DataAudiencia,
                                                                                       p.Processo.Comarca.EstadoId,
                                                                                       p.Processo.Comarca.Nome,
                                                                                       p.Processo.Vara.VaraId,
                                                                                       p.Processo.Vara.TipoVara.Nome,
                                                                                       p.Processo.NumeroProcesso,
                                                                                       TipoProcesso.PorId(p.Processo.TipoProcessoId).Nome,
                                                                                       p.Processo.Id));

            IQueryable<AlocacoesFuturas> queryAlocacao = (from a in DatabaseContext.AlocacoesPrepostos.Where(x => x.PrepostoId == prepostoId).AsNoTracking()//.Where(x => x.PrepostoId == prepostoId && x.DataAlocacao > System.DateTime.Now.Date).AsNoTracking()
                                                          join c in DatabaseContext.Comarcas on a.ComarcaId equals c.Id
                                                          join v in DatabaseContext.TiposDeVara on a.TipoVaraId equals v.Id
                                                          join p in DatabaseContext.Processos on new { a.ComarcaId, a.VaraId, a.TipoVaraId } equals new { p.ComarcaId, p.VaraId, p.TipoVaraId }
                                                          join ad in DatabaseContext.AudienciasDosProcessos on a.DataAlocacao.Date equals ad.DataAudiencia.Date

                                                          where a.DataAlocacao > System.DateTime.Now.Date
                                                                && ((jec == true && p.TipoProcessoId == TipoProcesso.JEC.Id) ||
                                                                   (civelConsumidor == true && p.TipoProcessoId == TipoProcesso.CIVEL_CONSUMIDOR.Id) ||
                                                                   (procon == true && p.TipoProcessoId == TipoProcesso.PROCON.Id))
                                                                && p.Id == ad.ProcessoId
                                                          select new AlocacoesFuturas("Agenda / Pauta",
                                                                                      a.DataAlocacao,
                                                                                      c.EstadoId,
                                                                                      c.Nome,
                                                                                      a.VaraId,
                                                                                      v.Nome,
                                                                                      p.NumeroProcesso,
                                                                                      TipoProcesso.PorId(p.TipoProcessoId).Nome,
                                                                                      p.Id));

            return queryAudiencia.Union(queryAlocacao).OrderBy(x => x.Data);
        }

        public string DescricaoProcesso(TipoVara tipo)
        {
            if (tipo.Eh_CivelConsumidor)
            {
                return "Civel Consumidor";
            }
            else
                if (tipo.Eh_CivelEstrategico)
            {
                return "Civel Estratégico";
            }
            else
                if (tipo.Eh_CriminalJudicial)
            {
                return "Criminal Judicial";
            }
            else
                if (tipo.Eh_Juizado)
            {
                return "Juizado Especial";
            }
            else
                if (tipo.Eh_Procon)
            {
                return "Procon";
            }
            else
                if (tipo.Eh_Trabalhista)
            {
                return "Trabalhista";
            }
            else
                if (tipo.Eh_Tributaria)
            {
                return "Tributário";
            }
            else
                return "";
        }

        public CommandResult<PaginatedQueryResult<AlocacoesFuturas>> ObterAlocacoesFuturas(int pagina, int quantidade, AlocacaoPrepostoSort sort, bool ascending, int prepostoId, string tiposProcessos)
        {
            string logName = nameof(AlocacoesFuturas);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AlocacoesFuturas>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseAlocacoes(sort, ascending, prepostoId, tiposProcessos);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<AlocacoesFuturas>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AlocacoesFuturas>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Preposto>> Obter(PrepostoSort sort, bool direcao, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Preposto>>.Forbidden();
            }

            string logName = nameof(Preposto);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, direcao, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Preposto>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<AlocacoesFuturas>> ObterAlocacao(AlocacaoPrepostoSort sort, bool direcao, int prepostoId, string tiposProcessos)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AlocacoesFuturas>>.Forbidden();
            }

            string logName = nameof(AlocacoesFuturas);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseAlocacoes(sort, direcao, prepostoId, tiposProcessos).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AlocacoesFuturas>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Preposto>> ObterTodos()
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PREPOSTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PREPOSTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Preposto>>.Forbidden();
            }

            string logName = nameof(Preposto);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(PrepostoSort.Nome, true, null).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Preposto>>.Valid(resultado);
        }

        public CommandResult<Preposto> ValidarDuplicidadeDeNomePreposto(string nomePreposto, int prepostoId)
        {
            var prepostoDuplicado = prepostoId != 0 ? DatabaseContext.Prepostos.FirstOrDefault(c => c.Ativo == true && c.Nome.ToUpper().Trim() == nomePreposto.ToUpper().Trim() && c.Id != prepostoId) :
                                                      DatabaseContext.Prepostos.FirstOrDefault(c => c.Ativo == true && c.Nome.ToUpper().Trim() == nomePreposto.ToUpper().Trim());

            return CommandResult<Preposto>.Valid(prepostoDuplicado);
        }
    }
}