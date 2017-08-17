using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models.Entities;

namespace API.ViewModels
{
    public class PaymentViewModelGet
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Время оплаты
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
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
        /// Описание, как оплатили (для нестандартных способов)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Оплата удалена
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class PaymentViewModelShortGet
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Время оплаты
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
        public Decimal Sum { get; set; }

        /// <summary>
        /// Оплата удалена
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class PaymentViewModelPost
    {
        [Required]
        public int OrderId { get; set; }

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
        /// Описание, как оплатили (для нестандартных способов)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Оплата удалена
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}