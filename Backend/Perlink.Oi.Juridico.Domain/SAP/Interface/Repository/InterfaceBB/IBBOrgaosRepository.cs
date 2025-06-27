using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB {
    public interface IBBOrgaosRepository : IBaseCrudRepository<BBOrgaos, long> {
        Task<ICollection<BBOrgaosResultadoDTO>> RecuperarPorFiltros(ConsultaBBOrgaosDTO consultaBBOrgaosDTO);
        Task<int> RecuperarTotalRegistros(ConsultaBBOrgaosDTO consultaBBOrgaosDTO);
        Task<bool> ExisteBBComarcaAssociadoBBOrgao(long codigo);
        Task<bool> CodgioOrgaoBBExiste(BBOrgaos entidade);
        Task<bool> OrgaoBBAssociadocomTribunais(long id);
        Task<BBOrgaos> RecuperarPorCodigoBB(long v);
    }
}
