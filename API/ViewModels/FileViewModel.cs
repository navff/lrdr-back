using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models.Entities;

namespace API.ViewModels
{
    public class FileViewModelGet
    {
        /// <summary>
        /// Внутренний Id файла
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Расширение
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Id связанного объекта
        /// </summary>
        public int? LinkedObjectId { get; set; }

        /// <summary>
        /// Тип связанного объекта
        /// </summary>
        public LinkedObjectType LinkedObjectType { get; set; }

        /// <summary>
        /// Дата создания файла
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Временный файл передаётся с FormId. Затем он 
        /// будет удалён, как только будет сохранена остальная
        /// форма.
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// Код Guid, для непрямого доступа к файловой системе,
        /// чтобы невозможно было предсказать имя соседнего файла
        /// </summary>
        public string Code { get; set; }
    }


    public class FileViewModelPost
    {

        /// <summary>
        /// Имя файла
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Расширение
        /// </summary>
        [Required]
        public string Extension { get; set; }

        /// <summary>
        /// Временный файл передаётся с FormId. Затем он 
        /// будет удалён, как только будет сохранена остальная
        /// форма.
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// Содержимое файла в Base64
        /// </summary>
        [Required]
        public string Data { get; set; }
    }

    public class FileToExistsObjectViewModelPost
    {

        /// <summary>
        /// Имя файла
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Расширение
        /// </summary>
        [Required]
        public string Extension { get; set; }

        /// <summary>
        /// Ссылка на объект, к которому прикрепляем файл
        /// </summary>
        [Required]
        public int LinkedObjectId { get; set; }

        /// <summary>
        /// Тип объекта, к которому прикрепляем файл
        /// </summary>
        [Required]
        public LinkedObjectType LinkedObjectType { get; set; }

        /// <summary>
        /// Содержимое файла в Base64
        /// </summary>
        [Required]
        public string Data { get; set; }
    }

}