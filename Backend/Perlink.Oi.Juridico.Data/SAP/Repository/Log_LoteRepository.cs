using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository;
using Perlink.Oi.Juridico.Domain.SAP;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class Log_LoteRepository : BaseCrudRepository<Log_Lote, long>, ILog_LoteRepository
    {
        private readonly JuridicoContext dbContext;
          private readonly UsuarioRepository usuarioRepository;
        public Log_LoteRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            usuarioRepository = new UsuarioRepository(dbContext, user);
        }


       public async Task<IEnumerable<LogLoteDTO>> ObterHistorico(long codigoLote)
        {
            var retorno = await context.Set<Log_Lote>()
                                  .Where(p => p.Id == codigoLote)
                                   .Select(log => new LogLoteDTO()
                                   {
                                       Id = log.Id,
                                       DataLog = log.DataLog.ToString("dd/MM/yyyy HH:mm:ss"),
                                       DescricaoStatusPagamento = log.DescricaoStatusPagamento,
                                       NomeUsuario = log.NomeUsuario

                                   }).OrderByDescending(log => log.DataLog).ToListAsync();

           
            
            return retorno.Select(p => ObterUsuarioNome(p)).ToList();
           
        }

        private LogLoteDTO ObterUsuarioNome(LogLoteDTO lote)             
        {
            lote.NomeUsuario = usuarioRepository.ObterUsuarioNome(lote.NomeUsuario).Result;
            return lote;

        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

      

        public async Task<Log_Lote> RecuperarUltimoLog(long codigoLote)
        {
            var Log_Lote = await context.Set<Log_Lote>()
                                   .Where(log_Lote => log_Lote.Id == codigoLote &&
                                    (log_Lote.CodigoStatusPagamentoAntes != log_Lote.CodigoStatusPagamentoDepois ||
                                    log_Lote.CodigoStatusPagamentoDepois == Convert.ToInt32(StatusPagamentoEnum.PedidoSAPPago)))
                                   .OrderByDescending(log_Lote => log_Lote.DataLog)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync();

            return Log_Lote;

        }

      
    }
}
