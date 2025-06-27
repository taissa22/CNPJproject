using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data
{
    public partial class ControleDeAcessoContext : DbContext
    {
        public async Task<List<VPermissoesAcessoWeb>> ObtemPermissoesAsync(string codUsuario)
        {
            OracleParameter p1 = new OracleParameter("P_COD_USUARIO", codUsuario);
            OracleParameter p2 = new OracleParameter("R_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var options = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            string sql = "BEGIN JUR.SP_PERMISSOES_ACESSO_WEB(:P_COD_USUARIO, :R_CURSOR); END;";
            var permissoes = (await VPermissoesAcessoWeb.FromSqlRaw(sql, p1, p2).FromCacheAsync(options)).ToList();
            return permissoes;
        }

        public async Task<bool> TemPermissaoAsync(string codUsuario, params string[] permissoes)
        {
            OracleParameter p1 = new OracleParameter("P_COD_USUARIO", codUsuario);
            OracleParameter p2 = new OracleParameter("R_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var options = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            string sql = "BEGIN JUR.SP_PERMISSOES_ACESSO_WEB(:P_COD_USUARIO, :R_CURSOR); END;";
            var listaPermissoes = (await VPermissoesAcessoWeb.FromSqlRaw(sql, p1, p2).FromCacheAsync(options)).ToList();
            return listaPermissoes.Select(x => x.CodMenu).Intersect(permissoes).Any();
        }

    }
}
