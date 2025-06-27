using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas
{
    [Obsolete]
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> Respond(object result, IEnumerable<Notification> notifications)
        {
            if (!notifications.Any())
            {
                return Ok(result);
            }
            else
            {
                var translateNotifications = new List<Notification>();

                notifications.ToList().ForEach(notification =>
                {
                    translateNotifications.Add(new Notification(notification.Property, notification.Message));
                });

                return BadRequest(new
                {
                    success = false,
                    errors = translateNotifications
                });
            }
        }
    }
}
