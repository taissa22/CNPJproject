namespace Perlink.Oi.Juridico.Domain.SAP.Interface.IEntity
{
    /**
     * Representa itens que irão à Dual List
     */
    public interface IDualListItem<TIdType>
    {
        TIdType Id { get; set; }
        string Descricao { get; set; }
    }
}
