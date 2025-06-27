using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Service;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class BancoDoBrasilStatusParcelasService : BaseCrudService<BBStatusParcelas, long>, IBancoDoBrasilStatusParcelasService
    {
        private readonly IBancoDoBrasilStatusParcelasRepository repository;

        public BancoDoBrasilStatusParcelasService(IBancoDoBrasilStatusParcelasRepository repository) : base(repository)
        {
            this.repository = repository;
        }
    }
}