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
            LoggerFactory.CreateInstance("records").Info("test info");
            LoggerFactory.CreateInstance("records").Warning("test warning");
            LoggerFactory.CreateInstance("records").Error("test error");
            LoggerFactory.CreateInstance("records").Fatal("test Fatal");

            LoggerFactory.CreateInstance().Debug("test debug");
            LoggerFactory.CreateInstance().Info("test info");
            LoggerFactory.CreateInstance().Warning("test warning");
            LoggerFactory.CreateInstance().Error("test error");
            LoggerFactory.CreateInstance().Fatal("test Fatal");
        }
    }
}
