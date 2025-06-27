using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{

    internal class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoDocumentoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoDocumentoRepository(IDatabaseContext databaseContext, ILogger<ITipoDocumentoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }


        private IQueryable<TipoDocumentoCommandResult> ObterBase(TipoProcesso tipoProcesso, TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipos de Prazo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            IQueryable<TipoDocumentoCommandResult> query = TipoDocumentoFactory.TipoDocumentoCommandResult(tipoProcesso)
                .CreateQuery(DatabaseContext).GerarQuery(sort,ascending,pesquisa);            

            bool temPermissao = false;

            switch (tipoProcesso.Id)
            {
                case 1:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CIVEL_CONSUMIDOR);
                    break;
                case 2:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRABALHISTA);
                    break;
                case 3:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_ADMINISTRATIVO);
                    break;
                case 4:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRIB_ADMINISTRATIVO);
                    break;
                case 5:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRIB_JUDICIAL);
                    break;
                case 6:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRABALHISTA_ADMINISTRATIVO);
                    break;
                case 7:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_JUIZADO_ESPECIAL);
                    break;
                case 9:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CIVEL_ESTRATIGICO);
                    break;
                case 12:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CIVEL_ADMINISTRATIVO);
                    break;
                case 14:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CRIMINAL_ADMINISTRATIVO);
                    break;
                case 15:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CRIMINAL_JUDICIAL);
                    break;
                case 17:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_PROCON);
                    break;
                case 18:
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_PEX);
                    break;
            }


            query = query.Where(x => temPermissao && x.CodTipoProcesso == tipoProcesso.Id);

            return query;
        }

        public CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>> ObterPaginado(TipoProcesso tipoProcesso, int pagina, int quantidade, TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {

            string logName = "Tipos de documentos";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TipoDocumentoCommandResult>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>> Obter(TipoProcesso tipoProcesso, TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>>.Forbidden();
            }

            string logName = "Tipos de prazo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>>.Valid(resultado);
        }


        public CommandResult<IEnumerable<TipoProcesso>> ObterComboboxTipoProcesso()
        {
            List<TipoProcesso> tiposProcesso = new List<TipoProcesso>();

            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CIVEL_CONSUMIDOR))
                tiposProcesso.Add(TipoProcesso.CIVEL_CONSUMIDOR);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CIVEL_ESTRATIGICO))
                tiposProcesso.Add(TipoProcesso.CIVEL_ESTRATEGICO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRABALHISTA))
                tiposProcesso.Add(TipoProcesso.TRABALHISTA);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRABALHISTA_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.TRABALHISTA_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRIB_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.TRIBUTARIO_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_TRIB_JUDICIAL))
                tiposProcesso.Add(TipoProcesso.TRIBUTARIO_JUDICIAL);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CIVEL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.CIVEL_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CRIMINAL_JUDICIAL))
                tiposProcesso.Add(TipoProcesso.CRIMINAL_JUDICIAL);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_CRIMINAL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcesso.CRIMINAL_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_JUIZADO_ESPECIAL))
                tiposProcesso.Add(TipoProcesso.JEC);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_PROCON))
                tiposProcesso.Add(TipoProcesso.PROCON);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_DOCUMENTO_PEX))
                tiposProcesso.Add(TipoProcesso.PEX);
       

            return CommandResult<IEnumerable<TipoProcesso>>.Valid(tiposProcesso);
        }

        //public CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>> ObterPaginadoDoConsumidor(int pagina, int quantidade, TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        //{
        //    string logName = "Tipos de documentos";
        //    Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
        //    if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
        //    {
        //        Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
        //        return CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>>.Forbidden();
        //    }

        //    Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
        //    var listaBase = ObterBaseConsumidor(sort, ascending, pesquisa);

        //    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
        //    var total = listaBase.Count();

        //    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
        //    var skip = Pagination.PagesToSkip(quantidade, total, pagina);

        //    var resultado = new PaginatedQueryResult<TipoDocumentoCommandResult>()
        //    {
        //        Total = total,
        //        Data = listaBase.Skip(skip).Take(quantidade).ToArray()
        //    };

        //    Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
        //    return CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>>.Valid(resultado);
        //}

        //public CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>> ObterParaExportacaoConsumidor( TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        //{
        //    if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DOCUMENTO))
        //    {
        //        Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DOCUMENTO, UsuarioAtual.Login));
        //        return CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>>.Forbidden();
        //    }

        //    string logName = "Tipo de Documento";
        //    Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

        //    Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
        //    var resultado = ObterBaseConsumidor(sort, ascending, pesquisa).ToArray();

        //    Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
        //    return CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>>.Valid(resultado);
        //}

        //private IQueryable<TipoDocumentoCommandResult> ObterBaseConsumidor( TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        //{
        //    string logName = "Tipos de Prazo";
        //    Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

        //    var listaTipoDocumentoMigracao = DatabaseContext.TiposDocumentos.Where(x => x.CodTipoProcesso == 9).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();

        //    IQueryable<TipoDocumentoCommandResult> query = from a in DatabaseContext.TiposDocumentos.AsNoTracking()
        //                                           join ma in DatabaseContext.TipoDocumentoMigracao on a.Id equals ma.CodTipoDocCivelConsumidor into LeftJoinMa
        //                                           from ma in LeftJoinMa.DefaultIfEmpty()
        //                                           where a.CodTipoProcesso == 1
        //                                                   select new TipoDocumentoCommandResult(
        //                                               a.Id,
        //                                               a.Descricao,
        //                                               a.MarcadoCriacaoProcesso,
        //                                               a.Ativo,
        //                                               a.IndRequerDatAudiencia,
        //                                               a.IndPrioritarioFila,
        //                                               a.IndDocumentoProtocolo,
        //                                               a.IndDocumentoApuracao,
        //                                               a.IndEnviarAppPreposto,                                                       
        //                                               (int?)ma.CodTipoDocCivelEstrategico == null ? false : listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelEstrategico) != null ? listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelEstrategico).Ativo : false,
        //                                               (int?)ma.CodTipoDocCivelEstrategico,
        //                                               (int?)ma.CodTipoDocCivelEstrategico == null ? null : listaTipoDocumentoMigracao.FirstOrDefault(z => z.Id == ma.CodTipoDocCivelEstrategico).Descricao

        //                                           );


        //    switch (sort)
        //    {
        //        case TipoDocumentoSort.Codigo:
        //            query = query.SortBy(a => a.Id, ascending);
        //            break;

        //        case TipoDocumentoSort.CadastraProcesso:
        //            if (ascending)
        //            {
        //                query = query.OrderBy(a => a.MarcadoCriacaoProcesso).ThenBy(a => a.Id);
        //            }
        //            else
        //            {
        //                query = query.OrderByDescending(a => a.MarcadoCriacaoProcesso).ThenByDescending(a => a.Id);
        //            }
        //            break;

        //        case TipoDocumentoSort.PrioritarioNaFila:
        //            if (ascending)
        //            {
        //                query = query.OrderBy(a => a.IndPrioritarioFila).ThenBy(a => a.Id);
        //            }
        //            else
        //            {
        //                query = query.OrderByDescending(a => a.IndPrioritarioFila).ThenByDescending(a => a.Id);
        //            }
        //            break;

        //        case TipoDocumentoSort.Descricao:
        //        default:
        //            query = query.SortBy(a => a.Descricao, ascending);
        //            break;
        //    }

        //    query = query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        //    return query;
        //}

        //public CommandResult<IReadOnlyCollection<TipoDocumento>> ObterDescricaoDeParaCivelEstrategico()
        //{
        //    IQueryable<TipoDocumento> query = DatabaseContext.TiposDocumentos.AsNoTracking().Where(x => x.TipoProcesso.Id == 9);

        //    return CommandResult<IReadOnlyCollection<TipoDocumento>>.Valid(query.ToArray());
        //}
    }
}
