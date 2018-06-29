using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLogServer.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace NLogServer
{
    // Here you will be telling the AppServer to use MyAppSession as the default AppSession class and the MyRequestInfo as the defualt RequestInfo

    public class MyNLogDevicesAppServer : AppServer<MyNLogDevicesSession, MyRequestInfo>
    {
        public static ConcurrentDictionary<string, NLogdevice> dict = new ConcurrentDictionary<string, NLogdevice>();
        public myNLogClientAppServer ClientAppServer;

        internal byte[] DefaultResponse { get; private set; }

        // Here in constructor telling to use MyReceiveFilter and MyRequestInfo
        Thread thread;
        public MyNLogDevicesAppServer(myNLogClientAppServer @ClientAppServer) : base(new DefaultReceiveFilterFactory<MyReceiveFilter, MyRequestInfo>())
        {
            ClientAppServer = @ClientAppServer;
            NewRequestReceived += ProcessNewMessage;
            DefaultResponse = ASCIIEncoding.ASCII.GetBytes("SN?\r\n");

            dict.TryAdd("1", new NLogdevice()
            {
                SN = "1",
                lastComm = DateTime.Now
            });

            //dict.TryAdd("2", new NLogdevice()
            //{
            //    SN = "2",
            //    lastComm = DateTime.Now
            //});


            thread = new Thread(new ThreadStart(MsgGuard));
            thread.Start();

            thread = new Thread(new ThreadStart(MsgGuard1));
            thread.Name = "MsgGuard1";
            thread.Start();
 
        }

        private void MsgGuard1()
        {
            while (true)
            {

                var devs = Global.DevDict.Values.ToArray();


                foreach (var d in devs)
                {
                    var t = d.getLastTimePost();
                    if ((DateTime.Now-t).TotalSeconds>10000.0)
                    {
                        d.setLastTimePost();
                        d.inc();
                        try
                        {
                            var s = this.GetSessionByID(d.AppSessionId);
                            s.Send(d.DeviceSN+":$\r\n");
                            
                            //this.AsyncRun(() => s.Send(d.DeviceSN + ":$\r\n"));
                        }
                        catch (Exception ex)
                        {

                            
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }
     
        private void MsgGuard()
        {
            while (true)
            {


                while (!Global.DBQueue.IsEmpty)
                {
                    MyRequestInfoDB info;
                    if (Global.DBQueue.TryDequeue(out info))
                    {
                        switch (info.requestInfo.stage)
                        {                             
                            case sessionstate.HEADER_DONE:
                                Logger.Info("HEADER_DONE:");
                                break;
                            case sessionstate.DATA_DONE:

                                break;
                            
                            case sessionstate.SETUP_DONE:

                                break;
                            
                            case sessionstate.SETUP_ERROR:
                                break;
                            case sessionstate.SN_ERROR_LENGTH:
                                break;
                            default:
                                break;
                        }
                    }


                }


                var devs = Global.DevDict.GetEnumerator();
                while (devs.MoveNext())
                {
                    var c = devs.Current.Value;
                    if ((DateTime.Now-c.getLastTimePost()).TotalSeconds>1000000.0)
                    {

                        c.setLastTimePost();
                        IAppServer appserver;
                        Global.serversDic.TryGetValue("NLogDevices", out appserver);
                        var session = this.GetSessionByID(c.AppSessionId);
                        session.Send(c.messageToSend);
                        //Global.DevDict[c.DeviceSN] = c;
                    }
                }

                    Thread.Sleep(20);
            }
        }

        protected override void OnNewSessionConnected(MyNLogDevicesSession session)
        {            
            session.Send(DefaultResponse,0, DefaultResponse.Length);            
            base.OnNewSessionConnected(session);

        }
        // This method/event will fire whenever a new message is received from the client/session
        // After passing through the filter
        // the requestInfo will contain the Unicode string
        private void ProcessNewMessage(MyNLogDevicesSession session, MyRequestInfo requestinfo)
        {

            
            if (requestinfo.DeviceSN == null || requestinfo.DeviceSN.Length == 0 || requestinfo.Pwd == null)
                return;
            //---------------HEADER_DONE -------------------------------------------------------------------------
            //
            if (requestinfo.stage == sessionstate.HEADER_DONE) //full info for the first time. 
            {
                //check if SN exists in the DB
                bool deviceSNexists = getDeviceSN(requestinfo.DeviceSN);

                if (deviceSNexists)
                {
                    
                    session.DeviceSN = requestinfo.DeviceSN;
                    requestinfo.AppSessionId = session.SessionID;
                    requestinfo.setLastTimePost();
                  string   replyConn = getReplyMessage(requestinfo);

                    requestinfo.messageToSend = replyConn;
                    requestinfo.setLastTimePost();
                    Global.DevDict.TryAdd(session.DeviceSN, requestinfo.Clone());
                    
                    session.Send(replyConn); // now device can send the data file if exists
                        
                    //Global.DBQueue.Enqueue(new MyRequestInfoDB()
                    //{
                    //    requestInfo = requestinfo,
                    //});

                    //get DF and update comState

                    switch (requestinfo.nLogStatus.DF)
                    {
                        case 0:
                            ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.DATA_SKIP;
                            break;
                        case 1:
                            ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.DATA;
                            break;
                        case 2:
                            ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.DATA;
                            break;
                        default:
                            ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.DATA_SKIP;
                            break;
                    }
                }
                else
                {
                    Global.DBQueue.Enqueue(new MyRequestInfoDB()
                    {
                        requestInfo = requestinfo.stage= sessionstate.SN_ERROR_NOT_EXISTS,
                    });
                    session.Send("CLOSE");
                    session.Close();
                }

            }
            //---------------HEADER_DONE END -------------------------------------------------------------------------


            //---------------DATA_DONE , DATA_SKIP  -------------------------------------------------------------------------
            //
            else if (requestinfo.stage == sessionstate.DATA_DONE || requestinfo.stage == sessionstate.DATA_SKIP)
            {
                if (requestinfo.stage == sessionstate.DATA_DONE)
                {
                    Global.DBQueue.Enqueue(new MyRequestInfoDB()
                    {
                        requestInfo = requestinfo,
                    });
                    session.Send("FILE OK\r\n");
                    requestinfo.setLastTimePost();
                    Console.WriteLine("file ok");
                }

                // DATA_DONE check if we have pending setup for this Device
                string setupNLog = getSetup();
                if (setupNLog!=null)
                {
                    //change the state
                    ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.SETUP;
                    //send the setup to device
                    session.Send(setupNLog);
                    requestinfo.setLastTimePost();
                }
                else//no setup
                {
                    if (ClientAppServer != null)
                    {
                        //get active clients for this device and change state if any
                        var clientSessions = ClientAppServer.GetSessions(x => x.DeviceSN == requestinfo.DeviceSN).ToList();
                        if (clientSessions!=null)
                        {
                            ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.REMOTE;
                            foreach (var cs in clientSessions)
                            {
                                var sMsg2 = "REQUESTED CONNECTION: " + session.DeviceSN ;                                
                                cs.Send("\r\n" + sMsg2 + "\r\n");                                
                            }
                        }
                        else // no active client connection. change to REMOTE to be prepeare for any new one
                        {
                            ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.REMOTE;
                        }
                    }
                }
            }
            //---------------DATA_DONE OR DATA_SKIP END  -------------------------------------------------------------------------

            //---------------SETUP_DONE  -------------------------------------------------------------------------
            //
            else if (requestinfo.stage == sessionstate.SETUP_DONE)
            {
                //Log Setup_done
                Global.DBQueue.Enqueue(new MyRequestInfoDB()
                {
                    requestInfo = requestinfo,
                });

                if (ClientAppServer != null)
                {
                    //get active clients for this device and change state if any
                    if (ClientAppServer.GetSessions(x => x.DeviceSN == requestinfo.DeviceSN).Count() > 0)
                        ((NLogServer.MyReceiveFilter)session.m_ReceiveFilter).comState = sessionstate.REMOTE;
                }
            }
            //---------------SETUP_DONE  END -----------------------------------------------------------------

            //---------------REMOTE  -------------------------------------------------------------------------
            //
            else if (requestinfo.stage== sessionstate.REMOTE)
            {
                if (ClientAppServer != null)
                {
                    //get active sessions
                    var clientSessions = ClientAppServer.GetSessions(x => x.DeviceSN == requestinfo.DeviceSN).ToList();

                    foreach (var c in clientSessions)
                    {
                        var s = ClientAppServer.GetSessionByID(c.SessionID);
                        try
                        {
                             s.Send(requestinfo.data);
                        }
                        catch { }
                    }

                }
                
            }//switch stage
            //---------------REMOTE  END -------------------------------------------------------------------------

            if (Global.DevDict.Keys.Contains(session.DeviceSN)) // already reg
            {
                var oldReq = Global.DevDict[requestinfo.DeviceSN];

                //check if there is another session opened for this SN to forcibly close it.
                var oldAppSession = GetSessionByID(oldReq.AppSessionId);
                if (oldAppSession != null && oldAppSession.SessionID != session.SessionID)
                {
                    oldAppSession.Close();
                }

            }




        }

        private bool getDeviceSN(string deviceSN)
        {
            bool res = true;

            return res;
        }

        private string getSetup()
        {
            string setup = null;

            setup = "<Setup>\r\n";
            return null;
        }

        private string getReplyMessage(MyRequestInfo requestinfo)
        {
            string replyConn;
            if (ClientAppServer != null)
            {
                //get active clients for this device and change state if any
                if (ClientAppServer.GetSessions(x => x.DeviceSN == requestinfo.DeviceSN).Count() > 0)
                    replyConn = DateTime.Now.ToString("ddMMyyHHmmss") + "REQUEST\r\n";
                else
                    replyConn = DateTime.Now.ToString("ddMMyyHHmmss") + "CONNECTED\r\n";
            }
            else
                replyConn = DateTime.Now.ToString("ddMMyyHHmmss") + "CONNECTED\r\n";
            return replyConn;
        }

        protected override void OnSessionClosed(MyNLogDevicesSession session, CloseReason reason)
        {
            MyRequestInfo ireqDevInfo;
            if (session.DeviceSN!=null)
            {
                Global.DevDict.TryRemove(session.DeviceSN, out ireqDevInfo);
            }
            
            base.OnSessionClosed(session, reason);
        }
    }
     
}
