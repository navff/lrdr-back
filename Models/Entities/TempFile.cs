using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class TempFile
    {
        /// <summary>
        /// ID файла
        /// </summary>
        [Key]
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
        /// Двоичные данные файла
        /// </summary>
        [Required]
        public byte[] Data { get; set; }

        /// <summary>
        /// Главная картинка объекта
        /// </summary>
        public bool IsMain { get; set; }
    }

}
