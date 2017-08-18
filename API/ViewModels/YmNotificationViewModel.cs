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
        /// <summary>
        /// Для переводов из кошелька - p2p-incoming
        /// Для переводов с произвольной карты - card-incoming.
        /// </summary>
        public string notification_type { get; set; }

        /// <summary>
        /// Идентификатор операции в истории счета получателя.
        /// </summary>
        public string operation_id { get; set; }

        /// <summary>
        /// Сумма операции.
        /// </summary>
        public Decimal amount { get; set; }

        /// <summary>
        /// Сумма, которая списана со счета отправителя.
        /// </summary>
        public Decimal withdraw_amount { get; set; }

        /// <summary>
        /// Код валюты счета пользователя. Всегда 643 (рубль РФ согласно ISO 4217).
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Дата и время совершения перевода.
        /// </summary>
        public DateTimeOffset datetime { get; set; }

        /// <summary>
        /// Для переводов из кошелька - номер счета отправителя.
        /// Для переводов с произвольной карты - параметр содержит пустую строку.
        /// </summary>
        public string sender { get; set; }

        /// <summary>
        /// Для переводов из кошелька — перевод защищен кодом протекции.
        /// Для переводов с произвольной карты — всегда false.
        /// </summary>
        public bool codepro { get; set; }

        /// <summary>
        /// Метка платежа. Если метки у платежа нет, параметр содержит пустую строку.
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// SHA-1 hash параметров уведомления.
        /// </summary>
        public string sha1_hash { get; set; }

        /// <summary>
        /// Флаг означает, что уведомление тестовое. По умолчанию параметр отсутствует.
        /// </summary>
        public bool test_notification { get; set; }

        /// <summary>
        /// Флаг означает, что пользователь не получил перевод. Возможные причины:
        /// Перевод заморожен, так как на счете получателя достигнут лимит доступного остатка.Отображается в поле hold ответа метода account-info.
        /// Перевод защищен кодом протекции. В этом случае codepro = true.
        /// </summary>
        public bool unaccepted { get; set; }

        /// <summary>
        /// ФИО отправителя перевода. Если не запрашивались, параметры содержат пустую строку.
        /// </summary>
        public string lastname { get; set; }

        /// <summary>
        /// ФИО отправителя перевода. Если не запрашивались, параметры содержат пустую строку.
        /// </summary>
        public string firstname { get; set; }

        /// <summary>
        /// ФИО отправителя перевода. Если не запрашивались, параметры содержат пустую строку.
        /// </summary>
        public string fathersname { get; set; }

        /// <summary>
        /// Адрес электронной почты отправителя перевода. Если email не запрашивался, параметр содержит пустую строку.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Телефон отправителя перевода. Если телефон не запрашивался, параметр содержит пустую строку.
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Адрес, указанный отправителем перевода для доставки. Если адрес не запрашивался, параметры содержат пустую строку.
        /// </summary>
        public string city { get; set; }

        public string street { get; set; }

        public string building { get; set; }

        public  string suite { get; set; }

        public string flat { get; set; }

        public string zip { get; set; }

    }
}