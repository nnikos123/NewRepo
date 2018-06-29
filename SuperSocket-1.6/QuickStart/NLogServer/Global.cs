using SuperSocket.SocketBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace NLogServer
{
    public static class Global
    {
        public static ConcurrentDictionary<string, IAppServer> serversDic = new ConcurrentDictionary<string, IAppServer>();
        public static ConcurrentDictionary<string, MyRequestInfo> DevDict=new ConcurrentDictionary<string, MyRequestInfo>();
        public static ConcurrentQueue<MyRequestInfoDB> DBQueue= new ConcurrentQueue<MyRequestInfoDB>();


    }
}
