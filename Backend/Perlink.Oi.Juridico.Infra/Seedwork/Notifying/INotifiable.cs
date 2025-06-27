using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Seedwork.Notifying
{
    public interface INotifiable
    {
        IEnumerable<Notification> Notifications { get; }

        bool HasNotifications { get; }
    }
}