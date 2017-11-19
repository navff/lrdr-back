using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Models.Entities;

namespace Models.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        /// <summary>
        /// GUID-код, который будет в адресной строке
        /// </summary>
        public string Code { get; set; }

        public int ContractorUserId { get; set; }
        public User ContractorUser { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Deadline { get; set; }

        public int? CustomerUserId { get; set; }
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
        /// Все комменты прочитаны исполнителем
        /// </summary>
        public bool IsReadedByContractor { get; set; }

        /// <summary>
        /// Все комменты прочитаны заказчиком
        /// </summary>
        public bool IsReadedByCustomer { get; set; }
    }
}
