using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class AreaEnvolvidaRepository : BaseCrudRepository<AreaEnvolvida, long>, IAreaEnvolvidaRepository
    {
        public AreaEnvolvidaRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}
