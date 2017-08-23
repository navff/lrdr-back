using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class File
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Extension { get; set; }

        public int? LinkedObjectId { get; set; }
        public LinkedObjectType LinkedObjectType { get; set; }

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

    public enum LinkedObjectType
    {
        User = 0,
        Comment = 1,
        TempForm = 10,
    }


}
