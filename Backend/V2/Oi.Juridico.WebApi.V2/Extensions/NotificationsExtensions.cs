using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Oi.Juridico.WebApi.V2.Extensions
{
    public static class NotificationsExtensions
    {
        public static string ToNotificationsString(this IEnumerable<Notification> notifications)
        {
            return string.Join("\n", notifications.Select(x => x.ToString()));
        }
    }
}
