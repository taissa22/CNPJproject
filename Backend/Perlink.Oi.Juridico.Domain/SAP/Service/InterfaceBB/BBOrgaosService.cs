using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB {
    public class BBOrgaosService : BaseCrudService<BBOrgaos, long>, IBBOrgaosService {
        private readonly IBBOrgaosRepository repository;
        public BBOrgaosService(IBBOrgaosRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<bool> CodgioOrgaoBBExiste(BBOrgaos entidade) {
            return await repository.CodgioOrgaoBBExiste(entidade);
        }

        public async Task<bool> ExisteBBComarcaAssociadoBBOrgao(long codigo)
        {
            return await repository.ExisteBBComarcaAssociadoBBOrgao(codigo);
        }

        public async Task<ICollection<BBOrgaosResultadoDTO>> RecuperarPorFiltros(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            return await repository.RecuperarPorFiltros(consultaBBOrgaosDTO);
        }

        public async Task<int> RecuperarTotalRegistros(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            return await repository.RecuperarTotalRegistros(consultaBBOrgaosDTO);
        }

        public Task<bool> OrgaoBBAssociadocomTribunais(long id) {
            return repository.OrgaoBBAssociadocomTribunais(id);
        }
    }
}
