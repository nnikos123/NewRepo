 
 
using System;
using System.Linq;
using System.Text;
using NLogServer.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace NLogServer
{
    // Here you will be telling the AppServer to use MyAppSession as the default AppSession class and the MyRequestInfo as the defualt RequestInfo

        

    public class myNLogClientAppServer : AppServer<MyNLogClientSession,MyNLogClientReqInfo>
    {

        private MyNLogDevicesAppServer DevAppServer;

        internal byte[] DefaultResponse { get; private set; }

        // Here in constructor telling to use MyReceiveFilter and MyRequestInfo


        public myNLogClientAppServer(MyNLogDevicesAppServer @devAppServer) : base(new DefaultReceiveFilterFactory<myNLogClientReceiveFilter, MyNLogClientReqInfo>())
        {
            DevAppServer = @devAppServer;
            NewRequestReceived += ProcessNewMessage;
        }        
         

        private myNLogClientAppServer() : base(new DefaultReceiveFilterFactory<myNLogClientReceiveFilter, MyNLogClientReqInfo>())
        {
            NewRequestReceived += ProcessNewMessage;
            
        }
        protected override void OnNewSessionConnected(MyNLogClientSession session)
        {
             
            var sProductName = "myserver";
            var major = 1;
            var minor = 0;
            var revision = 0;
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string resp = string.Format("\r\n {0} GPRS Gateway v{1}.{2}.{3}  at {4}\r\n USER NAME>", sProductName, major, minor, revision, now);
            
            session.Send(resp);

            base.OnNewSessionConnected(session);
        }
        // This method/event will fire whenever a new message is received from the client/session
        // After passing through the filter
        // the requestInfo will contain the Unicode string
        private void ProcessNewMessage(MyNLogClientSession session, MyNLogClientReqInfo requestinfo)
        {
            Console.WriteLine(requestinfo.state.ToString());

            switch (requestinfo.state)
            {
                case ClientState.USER_DONE:
                    session.Send("\r\nDEVICE SN>");
                    break;
                case ClientState.SN_DONE:
                    session.Send("\r\nDEVICE PWD>");
                    break;
                case ClientState.PWD_DONE:
                    
                    var sMsg1 = "NO MATCHING CLIENT DEVICE FOUND - WAIT.";
                    var sMsg2 = "CONNECTED";

                    var devs = DevAppServer.GetSessions(x => x.DeviceSN == requestinfo.deviceSN).FirstOrDefault();
                    if (devs!=null)
                    {
                        session.Send("\r\n" + sMsg2 + "\r\n");
                    }
                    else
                    {
                        session.Send("\r\n" + sMsg1 + "\r\n");
                    }
                    

                    session.DeviceSN = requestinfo.deviceSN;
                    break;
                case ClientState.REMOTE:
                    Console.WriteLine(requestinfo.data);
 
                    
                    if (DevAppServer != null)
                    {
                        var devs2 = DevAppServer.GetSessions(x=>x.DeviceSN== requestinfo.deviceSN).FirstOrDefault();

                        MyRequestInfo devRecInfo;
                        Global.DevDict.TryGetValue(devs2.DeviceSN, out devRecInfo);
                         
                        if (devRecInfo != null)
                        {
                            var devSession = DevAppServer.GetSessionByID(devRecInfo.AppSessionId);
                            devSession.Send(requestinfo.data);                            
                        }
                        else
                        {
                            var sMsg3 = "NO MATCHING CLIENT DEVICE FOUND - WAIT.";
                            
                            session.Send("\r\n" + sMsg3 + "\r\n");

                        }
                    }


                    break;
                default:
                    break;
            }

        }
    }

}
