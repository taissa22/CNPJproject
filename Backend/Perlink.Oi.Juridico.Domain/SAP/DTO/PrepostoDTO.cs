using Perlink.Oi.Juridico.Domain.SAP.Interface.IEntity;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class PrepostoDTO : IDualListItem<long>
    {
        public long Id { get; set; }

        public string Descricao { get; set; }
    }
}
