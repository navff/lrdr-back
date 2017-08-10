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
        
    }

    public enum LinkedObjectType
    {
        User = 0,
        Comment = 1
    }


}
