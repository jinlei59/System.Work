using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syste.Work.Log
{
    public class NLogFactory : ILoggerFactory
    {
        public ILogger CreateInstance()
        {
            return new NLogLogger();
        }

        public ILogger CreateInstance(string appsettingString)
        {
            return new NLogLogger(appsettingString);
        }
    }
}
