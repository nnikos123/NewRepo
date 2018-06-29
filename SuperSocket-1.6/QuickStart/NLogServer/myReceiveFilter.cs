using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogServer
{
    //'Newer devices transmit 3rd part Status. Format:
    //'si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=...|DF=...
    //'Meaning:
    //'Version|PORNr|WD Nr |PU Time|RSSI |Bat V|ACQ
    //public class NLogStatus
    //{
    //    public string Version { get; set; }
    //    public string PORNr { get; set; }
    //    public string WDNr { get; set; }
    //    public string PUTime { get; set; }
    //    public string RSSI { get; set; }
    //    public string BatV { get; set; }
    //    public string ACQ { get; set; }
    //    public string DF { get; set; }
    //}
    //public enum sessionstate { HEADER, dataStarted, dataEnded, remote }
    public enum sessionstate { HEADER, HEADER_DONE, DATA,  DATA_CAPTUM, DATA_DONE, DATA_SKIP, SETUP, SETUP_DONE, SETUP_SKIP, REMOTE, SETUP_ERROR, SN_ERROR_LENGTH,
        SN_ERROR_NOT_EXISTS
    }

    public class MyReceiveFilter : IReceiveFilter<MyRequestInfo>
    {
      public  sessionstate comState = sessionstate.HEADER;
      
        devStatus devStatus =null;
        private int DF=0;
        string startDataDel = "[[[[[[";
        string endDataDel = "]]]]]";

        string startCaptumDel = "{{{{{{";
        string endCaptumDel = "}}}}}}";


        char nullDel = (char)0;
        // This Method (Filter) is called whenever there is a new request from a connection/session 
        //- This sample method will convert the incomming Byte Array to Unicode string

        string data = "";
         
        string headerInfo = "";
        string total = "";
        string SN = "";
        string status=null;

        string pwd = string.Empty;
        string setupReply = "";
        private int startCuptumIdx;
        private int endCaptumIdx;

        bool dataReceived,captumReceived;
        public MyRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            int startDataIdx, endDataIdx;
            
            rest = 0;

            string dataUnicode = Encoding.ASCII.GetString(readBuffer, offset, length);
            
            //Console.WriteLine(dataUnicode);
            total += dataUnicode;

            if (comState== sessionstate.DATA_SKIP)
            {
                comState = sessionstate.REMOTE;
            }
            

            switch (comState)
            {
                case sessionstate.HEADER:
                    headerInfo += dataUnicode;
                    var headerParts = headerInfo.Split(nullDel);

                    if (headerParts == null || headerParts.Length < 4)
                        return null;
                    SN = headerParts[0];
                    pwd = headerParts[1];
                    status = headerParts[2];
                    data = null;
                    comState = sessionstate.HEADER_DONE;

                    devStatus = new devStatus(status);
                    DF = devStatus.DF;
                    break;

                case sessionstate.DATA:

                    data += dataUnicode;

                    startDataIdx = data.IndexOf(startDataDel);
                    endDataIdx = data.IndexOf(endDataDel);

                    startCuptumIdx = data.IndexOf(startCaptumDel);
                    endCaptumIdx = data.IndexOf(endCaptumDel);

                    if (endDataIdx >= 0 && startDataIdx >= 0)
                    {
                        //data var contains all data recs                        
                        var lengthData = data.Length - startDataIdx - (data.Length - endDataIdx + 6);
                        data = data.Substring(startDataIdx);
                        endDataIdx = data.IndexOf(endDataDel);
                        data = data.Substring(0, endDataIdx + endDataDel.Length);
                        
                        dataReceived = true;
                    }

                    if (endCaptumIdx >= 0 && startCuptumIdx >= 0)
                    {
                        //data var contains all data recs                        
                        var lengthData = data.Length - startCuptumIdx - (data.Length - endCaptumIdx + 6);
                        data = data.Substring(startCuptumIdx);
                        endDataIdx = data.IndexOf(endCaptumDel);
                        data = data.Substring(0, endCaptumIdx + endDataDel.Length);
                       
                        captumReceived = true;
                    }

                    switch (DF)
                    {
                        case 1:
                            if(dataReceived)
                                comState = sessionstate.DATA_DONE;
                                break;
                        case 2:
                            if (captumReceived)
                                comState = sessionstate.DATA_DONE;
                            break;
                        case 3:
                            if (dataReceived && captumReceived)
                                comState = sessionstate.DATA_DONE;
                            break;

                        default:
                            break;
                    }
                    break;
                case sessionstate.SETUP:
                    setupReply += dataUnicode;

                    if (setupReply.IndexOf('\0') == -1) break;                                    

                    var iNullPos = setupReply.Split(nullDel);
                    if (iNullPos.Length>1)
                    {
                        comState = sessionstate.SETUP_DONE;
                        setupReply = iNullPos[0];
                    }
                    break;
                case sessionstate.REMOTE:

                    break;
                default:
                    break;
            }

            try
            {
                var deviceRequest = new MyRequestInfo();
                deviceRequest.stage = comState;
                switch (comState)
                {
                    case sessionstate.HEADER:
                        return null;                        
                    case sessionstate.HEADER_DONE:
                    case sessionstate.DATA:
                    case sessionstate.DATA_DONE:
                    case sessionstate.DATA_SKIP:
                        deviceRequest.Key = comState.ToString();
                        deviceRequest.DeviceSN = SN;
                        deviceRequest.Pwd = pwd;
                        deviceRequest.status = status;
                        deviceRequest.data = data;
                        deviceRequest.nLogStatus = devStatus;
                        setupReply = "";

                        break;
                    case sessionstate.SETUP_DONE:
                        deviceRequest.Key = comState.ToString();
                        deviceRequest.DeviceSN = SN;
                        deviceRequest.Pwd = pwd;
                        deviceRequest.status = status;
                        deviceRequest.SetupReply = setupReply;
                        setupReply = "";
                        break;
                    case sessionstate.REMOTE:
                        deviceRequest.Key = comState.ToString();
                        deviceRequest.DeviceSN = SN;
                        deviceRequest.Pwd = pwd;
                        deviceRequest.status = "";
                        deviceRequest.data = dataUnicode;
                        deviceRequest.nLogStatus = devStatus;
                        setupReply = "";

                        break;
                    default:
                        break;
                }

                return deviceRequest;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool validateSN(ref string sN)
        {
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int LeftBufferSize { get; }
        public IReceiveFilter<MyRequestInfo> NextReceiveFilter { get; }
        public FilterState State { get; }
    }
}
