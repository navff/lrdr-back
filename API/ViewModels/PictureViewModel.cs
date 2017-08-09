using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Entities;

namespace API.ViewModels
{
    /// <summary>
    /// Картинка
    /// </summary>
    public class PictureViewModelGet
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Главная картинка объекта
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// Ссылка на объект
        /// </summary>
        public int? LinkedObjectId { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        public LinkedObjectType LinkedObjectType { get; set; }

        public string Url { get; set; }
    }

    /// <summary>
    /// Картинка
    /// </summary>
    public class PictureViewModelShortGet
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// УРЛ с картинкой
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Картинка главная для объекта
        /// </summary>
        public bool IsMain { get; set; }
    }
}