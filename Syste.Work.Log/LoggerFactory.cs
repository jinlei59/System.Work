using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syste.Work.Log
{
    public class LoggerFactory
    {
        private static ILoggerFactory _factory = null;
        public static void SetLoggerFactory(ILoggerFactory factory)
        {
            if (_factory == null)
                _factory = factory;
            else
                throw new Exception("LoggerFactory Existed");
        }

        public static ILogger CreateInstance()
        {
            if (_factory == null)
                throw new Exception("LoggerFactory Invalidate");
            return _factory.CreateInstance();
        }

        public static ILogger CreateInstance(string appsettingString)
        {
            if (_factory == null)
                throw new Exception("LoggerFactory Invalidate");
            return _factory.CreateInstance(appsettingString);
        }
    }
}
