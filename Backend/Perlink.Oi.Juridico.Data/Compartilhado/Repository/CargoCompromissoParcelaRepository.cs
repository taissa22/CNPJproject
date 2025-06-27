using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class CargoCompromissoParcelaRepository : BaseCrudRepository<CargaCompromissoParcela, string>, ICargaCompromissoParcelaRepository
    {
        private readonly JuridicoContext dbContext;

        public CargoCompromissoParcelaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> GravarLogSolicCancelamento(long codLote, string usrCodSolic, string dataSolic)
        {            
            var affectedRows = dbContext.Database.ExecuteSqlCommand($"UPDATE JUR.Carga_Compromisso_Parcela " +
                $"  SET usr_solic_cancelamento='{usrCodSolic}' " +
                $"  , data_solic_cancelamento='{dataSolic}' " +
                $" WHERE JUR.CARGA_COMPROMISSO_PARCELA.ID in " +
                $" ( select ccp.id from JUR.lote_lancamento ll " +
                $" inner join JUR.carga_compromisso_parcela ccp on ccp.cod_processo = ll.cod_processo and " +
                $"  ccp.cod_lancamento = ll.cod_lancamento and ll.cod_lote = {codLote})");

            return affectedRows>0;
        }

        public async Task<bool> GravarCancelamento(long codLote, string comentario, string motivo, string status)
        {
            var affectedRows = dbContext.Database.ExecuteSqlCommand($"UPDATE JUR.Carga_Compromisso_Parcela " +
                $"  SET COMENTARIO_CANCELAMENTO='{comentario}' " +
                $"  , MOTIVO_CANCELAMENTO='{motivo}' " +
                $"  , Status='{status}' " +
                $" WHERE JUR.CARGA_COMPROMISSO_PARCELA.ID in " +
                $" ( select ccp.id from JUR.lote_lancamento ll " +
                $" inner join JUR.carga_compromisso_parcela ccp on ccp.cod_processo = ll.cod_processo and " +
                $"  ccp.cod_lancamento = ll.cod_lancamento and ll.cod_lote = {codLote})");

            return affectedRows > 0;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }        
    }
}
