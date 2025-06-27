using Perlink.Oi.Juridico.Domain.SAP.Interface.IEntity;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class AdvogadoEscritorioDTO : IDualListItem<long>
    {
        public long Id { get; set; }

        public string Descricao { get; set; }

        public long? CodigoInterno { get; set; }
    }    
}
