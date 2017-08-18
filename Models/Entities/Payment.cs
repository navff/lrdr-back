using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }

        /// <summary>
        /// Время оплаты
        /// </summary>
        [Required]
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
        [Required]
        public Decimal Sum { get; set; }

        /// <summary>
        ///  Id платежа внешней платёжной системы
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Id пользователя внешней платёжной системы
        /// </summary>
        public string ExternalUserId { get; set; }

        /// <summary>
        /// Id пользователя внутри нашей системы
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Описание, как оплатили (для нестандартных способов)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Оплата удалена
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// За что платёж
        /// </summary>
        public PaymentType PaymentType { get; set; }
    }

    public enum PaymentType
    {
        /// <summary>
        /// За использование системы
        /// </summary>
        ForSystemUsing = 0,

        /// <summary>
        /// Оплата за конкретный заказ
        /// </summary>
        ForUserOrder = 1
    }

}
