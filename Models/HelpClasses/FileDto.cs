using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.HelpClasses
{
    public class FileDto : Entities.File
    {
        public string Path { get; set; }
        public string PathThumb { get; set; }

    }
}
