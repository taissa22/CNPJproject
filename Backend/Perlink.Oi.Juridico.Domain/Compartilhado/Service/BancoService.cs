using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class BancoService : BaseCrudService<Banco, long>, IBancoService
    {
        private readonly IBancoRepository repository;

        public BancoService(IBancoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Banco>> RecuperarNomeBanco()
        {
            // Valido as permissões do usuário pra cada tipo de processo:

            return await repository.RecuperarNomeBanco();
        }

        public async Task<IEnumerable<BancoListaDTO>> RecuperarListaBancos()
        {
            return await repository.RecuperarListaBancos();
        }
    }
}