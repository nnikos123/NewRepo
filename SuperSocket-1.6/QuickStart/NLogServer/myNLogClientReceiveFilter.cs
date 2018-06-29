using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogServer
{
    public enum ClientState { USER=0, USER_DONE, SN, SN_DONE, PWD, PWD_DONE, REMOTE}

    public class myNLogClientReceiveFilter : IReceiveFilter<MyNLogClientReqInfo>
    {
        ClientState state = ClientState.USER;
        string authHeader = null;
        string userName = null;
        string deviceSN = null;
        string devicePWD = null;
        string data = null;
        public MyNLogClientReqInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            int crIndex = -1;
            string dataUnicode = Encoding.ASCII.GetString(readBuffer, offset, length);

            if (state==ClientState.USER_DONE)            
                state = ClientState.SN;
            if (state == ClientState.SN_DONE)
                state = ClientState.PWD;
            if (state == ClientState.PWD_DONE)
                state = ClientState.REMOTE;


            switch (state)
            {
                case ClientState.USER:
                    authHeader += dataUnicode;
                    crIndex = authHeader.IndexOf("\r\n");
                    if (crIndex>-1)
                    {
                        userName = authHeader.Substring(0, crIndex);
                        authHeader = null;
                        state = ClientState.USER_DONE;
                    }
                    break;
                
                case ClientState.SN:
                    authHeader += dataUnicode;
                    crIndex = authHeader.IndexOf("\r\n");
                    if (crIndex > -1)
                    {
                        deviceSN = authHeader.Substring(0, crIndex);
                        authHeader = null;
                        state = ClientState.SN_DONE;
                    }

                    break;
                case ClientState.PWD:
                    authHeader += dataUnicode;
                    crIndex = authHeader.IndexOf("\r\n");
                    if (crIndex > -1)
                    {
                        devicePWD= authHeader.Substring(0, crIndex);
                        authHeader = null;
                        data = "";
                        state = ClientState.PWD_DONE;
                    }

                    break;
                case ClientState.REMOTE:
                    data += dataUnicode;
                    crIndex = data.IndexOf("\r\n");
                    if (crIndex > -1)
                    {
                        data = data.Substring(0, crIndex);
                        data += "\r\n";
                    }

                    break;
                default:
                    break;
            }


            try
            {
                var reqInfo = new MyNLogClientReqInfo();
                reqInfo.state = state;
                   
                if (userName!=null)
                {
                    reqInfo.userName = userName;
                }
                if (deviceSN!= null)
                {
                    reqInfo.deviceSN = deviceSN;
                }
                if (devicePWD != null)
                {
                    reqInfo.devicePWD = devicePWD;
                }
                if (data!=null && data.Length>0)
                {
                    reqInfo.data = data;
                    data = "";
                }
                 
                return reqInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int LeftBufferSize { get; }
        public IReceiveFilter<MyNLogClientReqInfo> NextReceiveFilter { get; }
        public FilterState State { get; }
    }
}
