using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camps.Tools
{
    public static class Contracts
    {
        public static void Assert(params Boolean[] conditions)
        {
            foreach (bool condition in conditions)
            {
                if (!condition)
                {
                    throw new ArgumentException("[CONTRACT FAILED] ");
                }
            }
        }

        public static void Assert(string message, params Boolean[] conditions)
        {
            if (message == null)
            {
                message = "[CONTRACT FAILED] ";
            }
            else
            {
                message = "[CONTRACT FAILED] " + message;
            }

            foreach (bool condition in conditions)
            {
                if (!condition)
                {
                    throw new ArgumentException(message);
                }
            }
        }
    }
}
