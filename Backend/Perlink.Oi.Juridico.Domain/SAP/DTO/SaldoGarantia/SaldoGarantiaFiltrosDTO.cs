using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia
{
    public class SaldoGarantiaFiltrosDTO
    {
        public IEnumerable<BancoListaDTO> ListaBancos { get; set; }
        public IEnumerable<EmpresaDoGrupoDTO> ListaEmpresaDoGrupo { get; set; }
        public IEnumerable<EstadoDTO> ListaEstados { get; set; }
    }
}
