using Syste.Work.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlogapp
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerFactory.SetLoggerFactory(new NLogFactory());
            LoggerFactory.CreateInstance().Debug("test debug");
            LoggerFactory.CreateInstance().LogInfo("test info");
            LoggerFactory.CreateInstance().LogWarning("test warning");
            LoggerFactory.CreateInstance().LogError("test error");
            LoggerFactory.CreateInstance().Fatal("test Fatal");
        }
    }
}
