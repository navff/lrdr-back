using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModels
{
    /// <summary>
    /// https://tech.yandex.ru/money/doc/dg/reference/operation-details-docpage/
    /// </summary>
    public class YmOperationDetailsViewModel
    {
        /// <summary>
        /// Код ошибки, присутствует при ошибке выполнения запроса.
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// Идентификатор операции. Значение параметра соответствует либо значению 
        /// параметра operation_id ответа метода operation-history либо, 
        /// в случае если запрашивается история счета плательщика, 
        /// значению поля payment_id ответа метода process-payment.
        /// </summary>
        public string operation_id { get; set; }

        /// <summary>
        /// Статус платежа (перевода). Значение параметра соответствует значению 
        /// поля status ответа метода operation-history.
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Идентификатор шаблона платежа, по которому совершен платеж. 
        /// Присутствует только для платежей.
        /// </summary>
        public string pattern_id { get; set; }

        /// <summary>
        /// Направление движения средств. Может принимать значения:
        /// in (приход);
        /// out (расход).
        /// </summary>
        public string direction { get; set; }

        /// <summary>
        /// Сумма операции (сумма списания со счета).
        /// </summary>
        public Decimal amount { get; set; }

        /// <summary>
        /// Сумма к получению. Присутствует для исходящих переводов другим пользователям.
        /// </summary>
        public Decimal amount_due { get; set; }

        /// <summary>
        /// Сумма комиссии. Присутствует для исходящих переводов другим пользователям.
        /// </summary>
        public Decimal fee { get; set; }

        /// <summary>
        /// Дата и время совершения операции.
        /// </summary>
        public DateTimeOffset datetime { get; set; }

        /// <summary>
        /// Краткое описание операции (название магазина или источник пополнения).
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Номер счета отправителя перевода. Присутствует для входящих 
        /// переводов от других пользователей.
        /// </summary>
        public string sender { get; set; }

        /// <summary>
        /// Идентификатор получателя перевода. 
        /// Присутствует для исходящих переводов другим пользователям.
        /// </summary>
        public string recipient { get; set; }

        /// <summary>
        /// Тип идентификатора получателя перевода. 
        /// Присутствует для исходяших переводов другим пользователям.
        /// </summary>
        public string recipient_type { get; set; }

        /// <summary>
        /// Сообщение получателю перевода. 
        /// Присутствует для переводов другим пользователям.
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Комментарий к переводу или пополнению. 
        /// Присутствует в истории отправителя перевода или получателя пополнения.
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// Перевод защищен кодом протекции. 
        /// Присутствует для переводов другим пользователям.
        /// </summary>
        public bool codepro { get; set; }

        /// <summary>
        /// Код протекции. Присутствует для исходящих переводов, 
        /// защищённых кодом протекции.
        /// </summary>
        public string protection_code { get; set; }

        /// <summary>
        /// Дата и время истечения срока действия кода протекции. 
        /// Присутствует для входящих и исходящих переводов (от/другим) 
        /// пользователям, защищённых кодом протекции.
        /// </summary>
        public DateTimeOffset expires { get; set; }

        /// <summary>
        /// Дата и время приёма или отмены перевода, защищённого кодом протекции. 
        /// Присутствует для входящих и исходящих переводов, защищённых кодом протекции. 
        /// Если перевод еще не принят/не отвергнут получателем - поле отсутствует.
        /// </summary>
        public DateTimeOffset answer_datetime { get; set; }

        /// <summary>
        /// Метка платежа. Присутствует для входящих и исходящих переводов 
        /// другим пользователям Яндекс.Денег, у которых был указан 
        /// параметр label вызова request-payment.
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// Детальное описание платежа. Строка произвольного формата, 
        /// может содержать любые символы и переводы строк.
        /// </summary>
        public string details { get; set; }

        /// <summary>
        /// Тип операции. Описание возможных типов операций см. в описании
        /// метода operation-history
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Данные о цифровом товаре (пин-коды и бонусы игр, iTunes, Xbox, etc.) 
        /// Поле присутствует при успешном платеже в магазины цифровых товаров. 
        /// </summary>
        public object digital_goods { get; set; }

        /// <summary>
        /// Номер счета получателя в сервисе Яндекс.Деньги.
        /// Для исходящих переводов
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// Номер привязанного мобильного телефона получателя.
        /// Для исходящих переводов
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// CustomerEmail получателя.
        /// Для исходящих переводов
        /// </summary>
        public string email { get; set; }
    }
}