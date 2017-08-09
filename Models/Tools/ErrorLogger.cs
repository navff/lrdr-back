using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Camps.Tools
{
    public class ErrorLogger
    {
        public static void Log(string message, Exception exception)
        {
            Contracts.Assert(!String.IsNullOrEmpty(message), exception!=null);

            string loggerName = "ErrorLogger";
            if (exception.TargetSite != null && exception.TargetSite.DeclaringType!=null)
            {
                loggerName = exception.TargetSite.DeclaringType.FullName;
            }
            var logger = LogManager.GetLogger(loggerName);
            logger.Error(message + " | " + exception.StackTrace);
        }
    }
}
