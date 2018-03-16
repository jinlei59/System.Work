using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.Shell.MVP.Krypton
{
    public class MainService : IMainService
    {
        public string SayHello(string name)
        {
            return string.Format("Hi {0}", name);
        }
    }
}
