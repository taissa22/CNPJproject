using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarGrupoPedidoCommand : Validatable, IValidatable
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public int TipoProcessoId { get; private set; }
        public float MultaMedia { get; private set; }
        public float ToleranciaMultaMedia { get; private set; }

        public override void Validate()
        {
            if (Id <= 0)
            {
                AddNotification(nameof(Id), "Id é requerido.");
            }
        }
    }
}