using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Operations
{
    public class PageViewDTO<T>
    {
        /// <summary>
        /// Результаты выборки
        /// </summary>
        public IEnumerable<T> Content { get; set; }

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
        public OrderSorting SortBy { get; set; }
    }
}
