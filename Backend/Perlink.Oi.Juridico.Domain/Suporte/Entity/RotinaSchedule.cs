using Shared.Domain.Impl.Entity;

namespace Perlink.Oi.Juridico.Domain.Suporte.Entity
{
    public class RotinaSchedule : Entity<RotinaSchedule, long>
    {
        public string Nome { get; set; }
        public string Key { get; set; }
        public bool Ativo { get; set; }
        public bool LogEstaAtivo { get; set; }

    }
}
