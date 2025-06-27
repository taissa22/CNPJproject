using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class CompromissoProcessoRepository : BaseCrudRepository<CompromissoProcesso, long>, ICompromissoProcessoRepository
    {
        private readonly JuridicoContext dbContext;
        public CompromissoProcessoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task AtualizarCompromisso(long codigoProcesso, long codigoCompromisso) {

                string sql = String.Format(@"
                                BEGIN
                                     jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'S');
                                     jur.SP_ATUALIZA_VALOR_COMPROMISSO({1}, {2});
                                     jur.SP_ATUALIZA_STATUS_COMPROMISSO({1}, {2});
                                     jur.SP_ATUALIZA_IND_CALC_MEDIA({1});
                                END;
                                ", user.Login, codigoProcesso, codigoCompromisso);

                await dbContext.Database.ExecuteSqlCommandAsync(sql);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

    }
}
