using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Seedwork.Notifying
{
    public abstract class Notifiable : INotifiable
    {
        private HashSet<Notification> NotificationsSet { get; } = new HashSet<Notification>();

        protected void ClearNotifications()
        {
            NotificationsSet.Clear();
        }

        public IEnumerable<Notification> Notifications => NotificationsSet;

        public bool HasNotifications => !NotificationsSet.Count.Equals(0);

        public void AddNotification(string property, string message)
        {
            NotificationsSet.Add(new Notification(property, message));
        }

        public void AddNotification(Notification notification)
        {
            NotificationsSet.Add(notification);
        }

        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                AddNotification(notification);
            }
        }

        public void AddNotifications(Notifiable notifiable)
        {
            AddNotifications(notifiable.Notifications);
        }

        public void AddNotifications(IEnumerable<Notifiable> notifiables)
        {
            foreach (var notifiable in notifiables)
            {
                AddNotifications(notifiable.Notifications);
            }
        }
    }
}