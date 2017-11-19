using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModels
{
    public class CommentViewModelGet
    {
        public int Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Время, когда коммент был оставлен
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Заказ, к которому относится коммент
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Коммент прочитан пользователем, которому был отправлен
        /// </summary>
        public bool IsReaded { get; set; }

        /// <summary>
        /// Пользователь, который оставил коммент
        /// </summary>
        public UserViewModelShortGet User { get; set; }
    }

    public class CommentViewModelPost
    {
        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Заказ, к которому относится коммент
        /// </summary>
        public int OrderId { get; set; }
    }

    public class CommentViewModelPut
    {
        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }
    }
}