using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camps.BLL.Utilities
{
    public delegate Task AsyncEventHandler<T>(object sender, T modifiedObject);
}
