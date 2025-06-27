using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB
{
    public interface IBBStatusParcelasService : IBaseCrudService<BBStatusParcelas, long>
    {
        Task<bool> VerificarDuplicidadeCodigoBBStatusParcela(BBStatusParcelas obj);
    }
}