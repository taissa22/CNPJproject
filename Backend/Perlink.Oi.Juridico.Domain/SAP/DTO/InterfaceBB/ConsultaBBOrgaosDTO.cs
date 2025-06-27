using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB {
    public class ConsultaBBOrgaosDTO : OrdernacaoPaginacaoDTO {
        public long? CodigoBBTribunal { get; set; }
        public long? CodigoBBComarca { get; set; }
        public string NomeBBOrgao { get; set; }
        public long? Codigo { get; set; }
    }
    public class BBOrgaosResultadoDTO {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public string NomeBBTribunal { get; set; }
        public long CodigoBBTribunal { get; set; }
        public string NomeBBComarca { get; set; }
        public long CodigoBBComarca { get; set; }
    }
}
