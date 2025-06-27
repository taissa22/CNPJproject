using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB
{
    public class BBStatusParcelasService : BaseCrudService<BBStatusParcelas, long>, IBBStatusParcelasService
    {
        private readonly IBBStatusParcelasRepository repository;

        public BBStatusParcelasService(IBBStatusParcelasRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public Task<bool> VerificarDuplicidadeCodigoBBStatusParcela(BBStatusParcelas obj)
        {
            return repository.VerificarDuplicidadeCodigoBBStatusParcela(obj);
        }
    }
}