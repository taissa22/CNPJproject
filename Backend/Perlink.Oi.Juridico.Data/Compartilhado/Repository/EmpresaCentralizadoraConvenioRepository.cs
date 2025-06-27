using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class EmpresaCentralizadoraConvenioRepository : BaseCrudRepository<EmpresasCentralizadorasConvenio, long>, IEmpresaCentralizadoraConvenioRepository {
        private readonly JuridicoContext dbContext;

        public EmpresaCentralizadoraConvenioRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user) {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }

        public async Task<string> BuscarAgencia(long codigoEmpresaCentralizadora, string siglaEstado) {
            return await dbContext.EmpresasCentralizadorasConvenios
                .Where(e => e.CodigoEmpresaCentralizadora == codigoEmpresaCentralizadora && e.CodigoEstado == siglaEstado)
                .Select(e => string.Format("{0}-{1}", e.NumeroAgenciaDepositaria, e.DigitoAgenciaDepositaria)).FirstOrDefaultAsync();
        }
    }
}
