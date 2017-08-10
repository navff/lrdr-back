using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int OwnerUserId { get; set; }
        [ForeignKey("OwnerUserId")]
        public User OwnerUser { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Deadline { get; set; }

        public int? CustomerUserId { get; set; }
        [ForeignKey("CustomerUserId")]
        public User CustomerUser { get; set; }

        public DateTimeOffset Created { get; set; }

        public Decimal Price { get; set; }

        public OrderStatus Status { get; set; }

        public String DeliveryAddress { get; set; }

        /// <summary>
        /// Показывать контрагенту возможность оплаты или нет. Будут случаи, когда заказ я 
        /// создаю не как исполнитель, а как заказчик. Тогда возможность оплаты не нужна
        /// </summary>
        public bool ShowPayment { get; set; }

        public bool IsDeleted { get; set; }
    }

    public enum OrderStatus
    {
        Created = 0,
        Checked = 1,
        Payed = 2,
        Cancelled = 3,
        Done = 4
    }
}
