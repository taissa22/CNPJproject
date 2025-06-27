using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Extensions
{
    public static class NotificationsExtensions
    {
        public static string ToNotificationsString(this IEnumerable<Notification> notifications)
        {
            return string.Join("\n", notifications.Select(x => x.ToString()));
        }
    }
}
