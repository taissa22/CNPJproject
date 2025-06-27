using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.Processos
{
    public class AdvogadoEscritorioService : BaseCrudService<AdvogadoEscritorio, long>, IAdvogadoEscritorioService
    {
        private readonly IAdvogadoEscritorioRepository repository;
        public AdvogadoEscritorioService(IAdvogadoEscritorioRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<AdvogadoEscritorioDTO>> ConsultarAdvogadoEscritorio(bool ehEscritorio, string codUsuario)
        {
            if (ehEscritorio)
                return await repository.RecuperarAdvogadoEscritorioUsuarioEscritorio(codUsuario);
            else
                return await repository.RecuperarAdvogadoEscritorio();
        }

        public async Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioUsuarioEscritorio(string codUsuario)
        {
            return await repository.RecuperarAdvogadoEscritorioUsuarioEscritorio(codUsuario);
        }

        public async Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioPorCodigoProfissional(long codigoEscritorio)
        {
            return await repository.RecuperarAdvogadoEscritorioPorCodigoProfissional(codigoEscritorio);
        }
    }
}
