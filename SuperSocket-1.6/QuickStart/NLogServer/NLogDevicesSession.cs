using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace NLogServer
{

    public enum RemoteClientType { NLog,NlogApp}
    public class RemoteClient
    {
        public RemoteClientType type { get; set; }
        public string Key { get; set; }
        public string Pwd { get; set; }
        public string User { get; set; }

    }

    public class MyNLogDevicesSession : AppSession<MyNLogDevicesSession, MyRequestInfo>
    {
        // Properties related to your session.
        public string  DeviceSN{ get; set; }        
        
       
    }
     
}
