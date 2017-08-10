using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Models;
using Models.Entities;

namespace API.ViewModels
{
    public class OrderViewModelGet
    {
        public int Id { get; set; }

        /// <summary>
        /// GUID-код, который будет в адресной строке
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Id пользователя, который создал заказ
        /// </summary>
        public int OwnerUserId { get; set; }

        /// <summary>
        /// Пользователь, который создал заказ
        /// </summary>
        public UserViewModelGet OwnerUser { get; set; }

        /// <summary>
        /// Название заказа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дэдлайн заказа
        /// </summary>
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// Id клиента, для которого делается заказ
        /// </summary>
        public int? CustomerUserId { get; set; }

        /// <summary>
        /// Клиент для которого делается заказ
        /// </summary>
        public UserViewModelGet CustomerUser { get; set; }

        /// <summary>
        /// Время создания заказа
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Стоимость заказа
        /// </summary>
        public Decimal Price { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Адрес, по которому доставить (если нужно)
        /// </summary>
        public String DeliveryAddress { get; set; }

        /// <summary>
        /// Показывать контрагенту возможность оплаты или нет. Будут случаи, когда заказ я 
        /// создаю не как исполнитель, а как заказчик. Тогда возможность оплаты не нужна
        /// </summary>
        public bool ShowPayment { get; set; }

        /// <summary>
        /// Удалён в корзину
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class OrderShortViewModelGet
    {
        public int Id { get; set; }

        /// <summary>
        /// GUID-код, который будет в адресной строке
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Id пользователя, который исполняет заказ
        /// </summary>
        public int OwnerUserId { get; set; }

        /// <summary>
        /// Пользователь, который создал заказ
        /// </summary>
        public UserViewModelGet OwnerUser { get; set; }

        /// <summary>
        /// Название заказа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дэдлайн заказа
        /// </summary>
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// Id клиента, для которого делается заказ
        /// </summary>
        public int? CustomerUserId { get; set; }

        /// <summary>
        /// Клиент для которого делается заказ
        /// </summary>
        public UserViewModelGet CustomerUser { get; set; }

        /// <summary>
        /// Время создания заказа
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Стоимость заказа
        /// </summary>
        public Decimal Price { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Удалён в корзину
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class OrderViewModelPost
    {

        /// <summary>
        /// Id пользователя, который исполняет заказ
        /// </summary>
        public int OwnerUserId { get; set; }


        /// <summary>
        /// Название заказа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дэдлайн заказа
        /// </summary>
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// Id клиента, для которого делается заказ
        /// </summary>
        public int? CustomerUserId { get; set; }

        /// <summary>
        /// Время создания заказа
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Удалён в корзину
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class OrderChangeStatusViewModelPost
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderChangePriceViewModelPost
    {
        public int OrderId { get; set; }
        public Decimal Price { get; set; }
    }

    public class SearchOrderViewModelGet : PagewViewRequest
    {
        public string Word { get; set; }
        public int userId { get; set; }
    }

}