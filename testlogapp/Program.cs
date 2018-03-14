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

            LoggerFactory.CreateInstance("records").Debug("test debug");
            LoggerFactory.CreateInstance("records").LogInfo("test info");
            LoggerFactory.CreateInstance("records").LogWarning("test warning");
            LoggerFactory.CreateInstance("records").LogError("test error");
            LoggerFactory.CreateInstance("records").Fatal("test Fatal");

            LoggerFactory.CreateInstance().Debug("test debug");
            LoggerFactory.CreateInstance().LogInfo("test info");
            LoggerFactory.CreateInstance().LogWarning("test warning");
            LoggerFactory.CreateInstance().LogError("test error");
            LoggerFactory.CreateInstance().Fatal("test Fatal");
        }
    }
}
