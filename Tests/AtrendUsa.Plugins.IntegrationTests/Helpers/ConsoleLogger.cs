using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Logging;
using Nop.Services.Logging;

namespace AtrendUsa.Plugins.IntegrationTests.Helpers
{
    public class ConsoleLogger : ILogger
    {
        #region Implementation of ILogger

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public void DeleteLog(Log log)
        {
            throw new NotImplementedException();
        }

        public void ClearLog()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Log> GetAllLogs(DateTime? fromUtc, DateTime? toUtc, string message, LogLevel? logLevel, int pageIndex,
            int pageSize)
        {
            throw new NotImplementedException();
        }

        public Log GetLogById(int logId)
        {
            throw new NotImplementedException();
        }

        public IList<Log> GetLogByIds(int[] logIds)
        {
            throw new NotImplementedException();
        }

        public Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", Customer customer = null)
        {
            Console.WriteLine("{0} : {1} - {2}", logLevel, shortMessage, fullMessage);

            return new Log();
        }

        #endregion
    }
}
