using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Filename { get; set; }

        public bool IsMain { get; set; }
       
        public int LinkedObjectId { get; set; }

        public LinkedObjectType LinkedObjectType { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }

    public enum LinkedObjectType
    {
        Activity = 1,
        Organizer = 2,
        User = 3
    }
}
