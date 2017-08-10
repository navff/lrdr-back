using System.Collections.Generic;

namespace API.ViewModels
{
    /// <summary>
    /// Отображение результатов выборки с постраничной навигацией и сортировкой
    /// </summary>
    /// <typeparam name="T">Тип результата выборки</typeparam>
    public class PageView<T>
    {
        /// <summary>
        /// Результаты выборки
        /// </summary>
        public  IEnumerable<T> Content { get; set; }

        /// <summary>
        /// Общее количество найденных элементов
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Номер текущей страницы пейджинга
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Общее количество страниц пейджинга
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Поле, по которому выдана сортировка
        /// </summary>
        public string SortBy { get; set; }
    }

    public abstract class PagewViewRequest
    {
        public int Page { get; set; }
        public string SortBy { get; set; }
    }
}