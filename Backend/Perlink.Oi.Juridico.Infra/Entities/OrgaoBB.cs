using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class OrgaoBB
    {
        public int Id { get; private set; }
        public string Nome { get; set; }
        public int TribunalBBId { get; set; }
        public TribunalBB TribunalBB { get; set; }
        public int ComarcaBBId { get; set; }
        public ComarcaBB ComarcaBB { get; set; }
    }
}