using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NLogClient
{
    class NLogClient
    {
         public TcpClient socket;
        public List<string> resposes = new List<string>();
        public string resp = "";

    }
}
