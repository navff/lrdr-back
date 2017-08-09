using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.ViewModels
{
    /// <summary>
    /// Временный файл
    /// </summary>
    public class TempFileViewModelGet
    {
        /// <summary>
        /// ID файла
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id формы, на которой закачали файл
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// Имя файла с расширением
        /// </summary>
        [Required]
        public string Filename { get; set; }

        /// <summary>
        /// УРЛ, по которому можно скачать файл
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Является главной картинкой для активности
        /// </summary>
        public bool IsMain { get; set; }
    }

    /// <summary>
    /// Временный файл
    /// </summary>
    public class TempFileViewModelPost
    {
        /// <summary>
        /// Id формы, на которой закачали файл
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// Имя файла с расширением
        /// </summary>
        [Required]
        public string Filename { get; set; }

        /// <summary>
        /// Двоичные данные файла
        /// </summary>
        [Required]
        public byte[] Data { get; set; }

        /// <summary>
        /// Является главной картинкой
        /// </summary>
        public bool IsMain { get; set; }
    }
}