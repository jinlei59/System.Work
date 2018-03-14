using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syste.Work.Log
{
    public interface ILoggerFactory
    {
        ILogger CreateInstance();
        ILogger CreateInstance(string appsettingString);
    }
}
