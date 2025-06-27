using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TipoPrazo;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
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
    internal class TipoPrazoRepository : ITipoPrazoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoPrazoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoPrazoRepository(IDatabaseContext databaseContext, ILogger<ITipoPrazoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<TipoPrazoCommandResult> ObterBase(TipoPrazoSort sort, bool ascending, TipoProcessoManutencao tipoProcesso, string pesquisa = null)
        {
            string logName = "Tipos de Prazo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            //IQueryable<TipoPrazoCommandResult> query = DatabaseContext.TiposPrazos.AsNoTracking();
            IQueryable<TipoPrazoCommandResult> query = TipoPrazoFactory.TipoPrazoCommandResult(tipoProcesso)
                .CreateQuery(DatabaseContext).GerarQuery(sort, ascending, pesquisa);
            
            //bool temPermissao = false;

            switch (tipoProcesso.Id)
            {
                case 1:
                    query = query.Where(x => x.Eh_Civel_Consumidor.HasValue && (bool)x.Eh_Civel_Consumidor && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CIVEL_CONSUMIDOR));
                    
                    break;

                case 2:
                    query = query.Where(x => (bool)x.Eh_Trabalhista && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_TRABALHISTA));
                    
                    break;

                case 3:
                    query = query.Where(x => (bool)x.Eh_Administrativo && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_ADMINISTRATIVO));
                    break;

                case 4:
                    query = query.Where(x => (bool)x.Eh_Tributario_Administrativo && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_TRIB_ADMINISTRATIVO));
                    break;

                case 5:
                    query = query.Where(x => (bool)x.Eh_Tributario_Judicial && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_TRIB_JUDICIAL));
                    break;

                case 7:
                    query = query.Where(x => (bool)x.Eh_Juizado_Especial && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_JUIZADO_ESPECIAL));
                    break;

                case 9:
                    query = query.Where(x => (bool)x.Eh_Civel_Estrategico && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CIVEL_ESTRATIGICO));
                    break;

                case 14:
                    query = query.Where(x => x.Eh_Criminal_Administrativo && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CRIMINAL_ADMINISTRATIVO));
                    break;

                case 15:
                    query = query.Where(x => (bool)x.Eh_Criminal_Judicial && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CRIMINAL_JUDICIAL));
                    break;

                case 17:
                    query = query.Where(x => (bool)x.Eh_Procon && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_PROCON));
                    break;

                case 18.1:
                    query = query.Where(x => (bool)x.Eh_Pex_Consumidor && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_PEX_CONSUMIDOR));
                    break;

                case 18.2:
                    query = query.Where(x => (bool)x.Eh_Pex_Juizado && UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_PEX_JUIZADO));
                    break;
            }

            return query;
        }

        public CommandResult<PaginatedQueryResult<TipoPrazoCommandResult>> ObterPaginado(int pagina, int quantidade, TipoPrazoSort sort, bool ascending, TipoProcessoManutencao tipoProcesso, string pesquisa = null)
        {
            string logName = "Tipos de prazo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PRAZO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PRAZO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<TipoPrazoCommandResult>>.Forbidden();
            }

            if (tipoProcesso == TipoProcessoManutencao.NAO_DEFINIDO)
            {
                Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida("Tipo de Processo " + TipoProcessoManutencao.NAO_DEFINIDO.Descricao) );
                return CommandResult<PaginatedQueryResult<TipoPrazoCommandResult>>.Invalid("Tipo de Processo " + TipoProcessoManutencao.NAO_DEFINIDO.Descricao);
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, tipoProcesso, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TipoPrazoCommandResult>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TipoPrazoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoPrazoCommandResult>> Obter(TipoPrazoSort sort, bool ascending, TipoProcessoManutencao tipoProcesso, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PRAZO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PRAZO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TipoPrazoCommandResult>>.Forbidden();
            }

            string logName = "Tipos de prazo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, tipoProcesso, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoPrazoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IEnumerable<TipoProcessoManutencao>> ObterComboboxTipoProcesso()
        {
            List<TipoProcessoManutencao> tiposProcesso = new List<TipoProcessoManutencao>();

            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CIVEL_CONSUMIDOR))
                tiposProcesso.Add(TipoProcessoManutencao.CIVEL_CONSUMIDOR);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CIVEL_ESTRATIGICO))
                tiposProcesso.Add(TipoProcessoManutencao.CIVEL_ESTRATEGICO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_TRABALHISTA))
                tiposProcesso.Add(TipoProcessoManutencao.TRABALHISTA);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_TRIB_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcessoManutencao.TRIBUTARIO_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_TRIB_JUDICIAL))
                tiposProcesso.Add(TipoProcessoManutencao.TRIBUTARIO_JUDICIAL);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcessoManutencao.ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CRIMINAL_JUDICIAL))
                tiposProcesso.Add(TipoProcessoManutencao.CRIMINAL_JUDICIAL);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_CRIMINAL_ADMINISTRATIVO))
                tiposProcesso.Add(TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_JUIZADO_ESPECIAL))
                tiposProcesso.Add(TipoProcessoManutencao.JEC);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_PROCON))
                tiposProcesso.Add(TipoProcessoManutencao.PROCON);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_PEX_JUIZADO))
                tiposProcesso.Add(TipoProcessoManutencao.PEX_JUIZADO);
            if (UsuarioAtual.TemPermissaoPara(Permissoes.COMBO_TIPO_PRAZO_PEX_CONSUMIDOR))
                tiposProcesso.Add(TipoProcessoManutencao.PEX_CONSUMIDOR);

            return CommandResult<IEnumerable<TipoProcessoManutencao>>.Valid(tiposProcesso);
        }
    }
}