using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;

namespace NLogServer
{



    class Program
    {
        static void Main(string[] args)
        {

            //string message = "SN?" + (char)0 + "044D0906" + (char)0 + "si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=ON";
            //var data = ASCIIEncoding.ASCII.GetBytes(message);


            //var txt=ASCIIEncoding.ASCII.GetString(data);
            var s = new myServer();
            s.Setup();
            s.StartServer();
            //s.TestCustomProtocol();

            Console.ReadKey();

        }
    }

  
}
