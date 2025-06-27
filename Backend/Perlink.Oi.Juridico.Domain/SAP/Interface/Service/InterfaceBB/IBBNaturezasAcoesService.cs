using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB
{
    public interface IBBNaturezasAcoesService : IBaseCrudService<BBNaturezasAcoes, long>
    {
        Task<bool> CodigoBBJaExiste(BBNaturezasAcoes obj);
    }
}
