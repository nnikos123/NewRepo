using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace NLogServer
{


    public class MyNLogClientSession : AppSession<MyNLogClientSession, MyNLogClientReqInfo>
    {
        internal string DeviceSN;

        // Properties related to your session.
        public RemoteClient RemoteClient { get; set; }        
        
    }
     
}
