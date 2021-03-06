﻿using System;
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
        /// Пользователь-создатель
        /// </summary>
        public UserViewModelGet PostedByUser { get; set; }


        /// <summary>
        /// Пользователь-исполнитель
        /// </summary>
        public UserViewModelGet ContractorUser { get; set; }

        /// <summary>
        /// Название заказа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дэдлайн заказа
        /// </summary>
        public DateTimeOffset Deadline { get; set; }


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

        /// <summary>
        /// Все сообщения заказа прочитаны исполнителем
        /// </summary>
        public bool IsReadedByContractor { get; set; }

        /// <summary>
        /// Все сообщения заказа прочитаны клиентом
        /// </summary>
        public bool IsReadedByCustomer { get; set; }
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
        public int ContractorUserId { get; set; }

        /// <summary>
        /// Пользователь, который создал заказ
        /// </summary>
        public UserViewModelGet ContractorUser { get; set; }

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


        /// <summary>
        /// Все сообщения заказа прочитаны исполнителем
        /// </summary>
        public bool IsReadedByContractor { get; set; }

        /// <summary>
        /// Все сообщения заказа прочитаны клиентом
        /// </summary>
        public bool IsReadedByCustomer { get; set; }
    }

    public class OrderViewModelPost
    {

        /// <summary>
        /// Id пользователя, который исполняет заказ
        /// </summary>
        public int ContractorUserId { get; set; }


        /// <summary>
        /// Название заказа
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Дэдлайн заказа
        /// </summary>
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// Id клиента, для которого делается заказ
        /// </summary>
        [Required]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Адрес доставки
        /// </summary>
        public String DeliveryAddress { get; set; }

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
        public string Word { get; set; } = null;
        public int? CustomerUserId { get; set; } = null;
        public int? ContractorUserId { get; set; } = null;
        public bool? isPaid { get; set; } = null;
    }

}