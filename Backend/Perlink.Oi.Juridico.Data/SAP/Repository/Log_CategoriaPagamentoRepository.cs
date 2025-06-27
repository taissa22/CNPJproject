using Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class Log_CategoriaPagamentoRepository : BaseCrudRepository<Log_CategoriaPagamento, long>, ILog_CategoriaPagamentoRepository
    {
        private readonly JuridicoContext dbContext;
          private readonly UsuarioRepository usuarioRepository;
        public Log_CategoriaPagamentoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            usuarioRepository = new UsuarioRepository(dbContext, user);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

    }
}
