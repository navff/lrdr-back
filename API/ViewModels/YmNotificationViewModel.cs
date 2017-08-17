using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModels
{
    /// <summary>
    /// https://tech.yandex.ru/money/doc/dg/reference/notification-p2p-incoming-docpage/
    /// </summary>
    public class YmNotificationViewModelPost
    {
        public string notification_type { get; set; }

    }
}