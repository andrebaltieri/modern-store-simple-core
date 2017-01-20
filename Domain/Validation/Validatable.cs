using System;
using System.Collections.Generic;

namespace ModernStore.Validation
{
    public abstract class Validatable
    {
        public Validatable()
        {
            Notifications = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Notifications { get; private set; }
        
        public void AddNotification(string notification)
        {
            Notifications.Add(Guid.NewGuid().ToString(), notification);
        }

        public void AddNotification(Dictionary<string, string> notifications)
        {
            foreach (var item in notifications)
                Notifications.Add(item.Key, item.Value);
        }

        public bool IsValid(){
            return Notifications.Count == 0;
        }
    }
}