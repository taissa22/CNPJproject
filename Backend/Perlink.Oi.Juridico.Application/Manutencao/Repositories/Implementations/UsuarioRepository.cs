using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Usuario;
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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class UsuarioRepository : IUsuarioRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IUsuarioRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public UsuarioRepository(IDatabaseContext databaseContext, ILogger<IUsuarioRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Usuario> ObterBase() {
            IQueryable<Usuario> query = DatabaseContext.Usuarios.Where(x => x.Ativo).AsNoTracking();            
            return query;
        }

        public CommandResult<PaginatedQueryResult<Usuario>> ObterTodos( ) {
            string logName = "Usuarios";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
           
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();           

            var resultado = new PaginatedQueryResult<Usuario>() {
                Total = total,
                Data = listaBase.ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Usuario>>.Valid(resultado);
        }

        public IEnumerable<UsuarioCommandResult> ObterAtivos()
        {
            return DatabaseContext.Usuarios.AsNoTracking().Where(x => x.Ativo)
                .Select(y => new UsuarioCommandResult { Id = y.Id, Ativo = y.Ativo, Nome = y.Nome })
                .OrderBy(z => z.Nome)
                .ToList();
        }

        public IEnumerable<UsuarioCommandResult> ObterAtivosEInativos()
        {
            return DatabaseContext.Usuarios.AsNoTracking()
                .Select(y => new UsuarioCommandResult { Id = y.Id, Ativo = y.Ativo, Nome = y.Nome})
                .OrderBy(z => z.Nome)
                .ToList();
        }


        public IEnumerable<UsuarioCommandResult> ObterPrepostosAtivos()
        {
            return DatabaseContext.Usuarios.AsNoTracking().Where(x => x.Ativo && x.IndPreposto && !x.Perfil)
                .Select(y => new UsuarioCommandResult { Id = y.Id, Ativo = y.Ativo, Nome = y.Nome })
                .OrderBy(z => z.Nome)
                .ToList();
        }

        public IEnumerable<UsuarioCommandResult> ObterTodosPrepostos()
        {
            return DatabaseContext.Usuarios.AsNoTracking().Where(x => x.IndPreposto && !x.Perfil)
                .Select(y => new UsuarioCommandResult { Id = y.Id, Ativo = y.Ativo, Nome = y.Nome })
                .OrderBy(z => z.Nome)
                .ToList();
        }
    }
}