using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Entities;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oi.Juridico.Contextos.V2.Extensions
{
    public static class DbContextEntensions
    {
        public static int SaveChanges(this DbContext context, string login, bool isUsuarioInternet)
        {
            var returnedId = 0;

            using (var scope = context.Database.BeginTransaction())
            {
                ExecutarProcedureDeLog(context, login, isUsuarioInternet);
                returnedId = context.SaveChanges();
                scope.Commit();
            }

            return returnedId;
        }

        public static int SaveChangesExternalScope(this DbContext context, string login, bool isUsuarioInternet)
        {
            var returnedId = 0;
            ExecutarProcedureDeLog(context, login, isUsuarioInternet);
            returnedId = context.SaveChanges();            

            return returnedId;
        }

        public async static Task<int> SaveChangesAsync(this DbContext context, string login, bool isUsuarioInternet, CancellationToken cancellationToken = default)
        {
            var returnedId = 0;

            using (var scope = context.Database.BeginTransaction())
            {
                ExecutarProcedureDeLog(context, login, isUsuarioInternet);
                returnedId = await context.SaveChangesAsync(cancellationToken);
                scope.Commit();
            }

            return returnedId;
        }

        public async static Task<int> SaveChangesExternalScopeAsync(this DbContext context, string login, bool isUsuarioInternet, CancellationToken cancellationToken = default)
        {
            var returnedId = 0;            
            ExecutarProcedureDeLog(context, login, isUsuarioInternet);
            returnedId = await context.SaveChangesAsync(cancellationToken);          

            return returnedId;
        }

        public async static Task<int> SaveChangesAsync(this DbContext context, string login, bool isUsuarioInternet, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var returnedId = 0;

            using (var scope = context.Database.BeginTransaction())
            {
                ExecutarProcedureDeLog(context, login, isUsuarioInternet);
                returnedId = await context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                scope.Commit();
            }

            return returnedId;
        }

        public async static Task<int> SaveChangesExternalScopeAsync(this DbContext context, string login, bool isUsuarioInternet, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var returnedId = 0;           
            ExecutarProcedureDeLog(context, login, isUsuarioInternet);
            returnedId = await context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
           
            return returnedId;
        }

        public static void ExecutarProcedureDeLog(this DbContext context, string login, bool isUsuarioInternet)
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(15, retryAttempt => TimeSpan.FromMilliseconds(250))
                .Execute(() =>
                {
                    context.ExecutarProcedureDeLogDoUsuario(login, isUsuarioInternet);
                    var cur = context.ObtemUsurioLogado();
                    if (cur.Status != OracleParameterStatus.Success) throw new Exception("Erro ao obter o usuário logado.");
                    if (((OracleString)cur.Value).Value != login) throw new Exception($"Usuário logado '{login}' é diferente do usuário salvo no banco pela procedure '{((OracleString)cur.Value).Value}'");
                });
        }

        private static void ExecutarProcedureDeLogDoUsuario(this DbContext context, string login, bool isUsuarioInternet)
        {
            var strInsereUsuario = $"BEGIN jur.SP_ACA_INSERE_LOG_USUARIO( '{login}', '{(isUsuarioInternet ? "S" : "N")}' ); END;";
            context.Database.ExecuteSqlRaw(strInsereUsuario);
        }

        private static OracleParameter ObtemUsurioLogado(this DbContext context)
        {
            var sqlGetUser = "BEGIN :CUR := JUR.GETUSER(); END;";           
            OracleParameter cur = new("CUR", OracleDbType.Varchar2, 200, null, ParameterDirection.Output);
            context.Database.ExecuteSqlRaw(sqlGetUser, cur);
            return cur;
        }

        public static void PesquisarPorCaseInsensitive(this DbContext context)
        {
            context.Database.ExecuteSqlRaw("ALTER SESSION SET NLS_COMP = LINGUISTIC;");
            context.Database.ExecuteSqlRaw("ALTER SESSION SET NLS_SORT = BINARY_CI;");
        }

        public static decimal GetNextSequence(this DbContext context, string sequenceName) => 
            context.Database.SqlQueryRaw<decimal>($"SELECT JUR.{sequenceName}.NextVal FROM Dual").AsEnumerable().First();
    }
}
