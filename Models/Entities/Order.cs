using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        /// <summary>
        /// GUID-код, который будет в адресной строке
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Code { get; set; }

        public int ContractorUserId { get; set; }
        [ForeignKey("ContractorUserId")]
        public User ContractorUser { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Deadline { get; set; }

        public int? CustomerUserId { get; set; }
        [ForeignKey("CustomerUserId")]
        public User CustomerUser { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public Decimal Price { get; set; }

        public OrderStatus Status { get; set; }

        public String DeliveryAddress { get; set; }

        /// <summary>
        /// Показывать контрагенту возможность оплаты или нет. Будут случаи, когда заказ я 
        /// создаю не как исполнитель, а как заказчик. Тогда возможность оплаты не нужна
        /// </summary>
        public bool ShowPayment { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// Которым пользователем создан заказ
        /// </summary>
        public int PostedByUserId { get; set; }
        [ForeignKey("PostedByUserId")]
        public User PostedByUser { get; set; }
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
