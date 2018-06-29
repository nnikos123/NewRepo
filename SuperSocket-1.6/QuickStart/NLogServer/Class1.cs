using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLogServer
{

    public class NLogdevice
    {
        private string _sN;
        private DateTime _lastComm;
        object sync = new object();
        private int _counter = 0;

        public string SN { get => _sN; set => _sN = value; }
        public DateTime lastComm
        {
            get
            {
                lock (sync)
                {
                    Thread.Sleep(10000);
                    return _lastComm;
                }
            }
            set
            {
                lock (sync)
                {
                    _lastComm = value;
                }
            }
        }
        public int counter { get => _counter; set => _counter = value; }
        public void inc()
        {
            Interlocked.Increment(ref _counter);
        }
    }

    /// <summary>
    ///   Newer devices transmit 3rd part Status. Format:
    ///   si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=
    ///   Meaning:    
    //    Version|PORNr|WD Nr |PU Time|RSSI |Bat V|ACQ
    /// </summary>
    public class devStatus
    {
        public string Version { get; private set; }
        public string PORNr { get; private set; }
        public string WDNr { get; private set; }
        public string PUTime { get; private set; }
        public string RSSI { get; private set; }
        public string BatV { get; private set; }
        public string ACQ { get; private set; }
        public int DF { get; private set; }
        public devStatus() { }
        public devStatus(string status)
        {
            status = "SI=" + status;
            var parts = status.Split('|');
            for (int i = 0; i < parts.Length; i++)
            {
                var iparts = parts[i].Split('=');

                switch (iparts[0].ToUpper())
                {
                    case "SI":
                        Version = iparts[1];
                        break;
                    case "P":
                        PORNr = iparts[1];
                        break;
                    case "WD":
                        WDNr = iparts[1];
                        break;
                    case "PUT":
                        PUTime = iparts[1];
                        break;
                    case "S":
                        RSSI = iparts[1];
                        break;
                    case "V":
                        BatV = iparts[1];
                        break;
                    case "A":
                        ACQ = iparts[1];
                        break;
                    case "DF":
                        DF = int.Parse(iparts[1]);
                        break;

                    default:
                        break;
                }
            }




        }

        public devStatus Clone()
        {
            lock (this)
            {
                return new devStatus()
                {
                    Version = this.Version,
                    PORNr = this.PORNr,
                    WDNr = this.WDNr,
                    PUTime = this.PUTime,
                    RSSI = this.RSSI,
                    BatV = this.BatV,
                    ACQ = this.ACQ,
                    DF = this.DF
                };

            }
        }
    }

       

        public class MyRequestInfoOld : RequestInfo<byte[]>
    {
        public int DeviceId { get; set; }
        public string Unicode { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryRequestInfo"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="body">The body.</param>
        public MyRequestInfoOld(string key, byte[] body)
            : base(key, body)
        {

        }
    }

    public class MyNLogClientReqInfo : IRequestInfo
    {
        internal string data;

        internal string userName { get; set; }
        internal string deviceSN { get; set; }
        internal string devicePWD { get; set; }

        public string Key { get; set; }
        public ClientState state { get; set; }

        public MyNLogClientReqInfo() { }
    }

    public class MyRequestInfo : IRequestInfo
    {
        internal string Pwd;
        internal devStatus nLogStatus;
        internal string messageToSend;
        private int _counter;

        public string DeviceSN { get; set; }
        public string data { get; set; }
        public string status { get; set; }
        public string[] statusParts { get; set; }

        public string Key { get; set; }

        public sessionstate stage { get; set; }

        public string AppSessionId { get; set; }
        public object SetupReply { get; internal set; }
        private DateTime _lastPost { get; set; }




        private readonly object sync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryRequestInfo"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="body">The body.</param>

        public MyRequestInfo()

        {

        }


        public void setLastTimePost()
        {
            lock (sync)
            {
                _lastPost = DateTime.Now;
            }
        }
        public DateTime getLastTimePost()
        {
            lock (sync)
            {
                return _lastPost;
            }
        }
        public int counter { get => _counter; set => _counter = value; }

        public void inc()
        {
            Interlocked.Increment(ref _counter);
        }

        internal MyRequestInfo Clone()
        {
            return new MyRequestInfo()
            {
                DeviceSN = this.DeviceSN,
                AppSessionId = this.AppSessionId,
                Key = this.Key,
                _lastPost = this.getLastTimePost(),
                SetupReply = this.SetupReply,
                status = this.status,
                stage = this.stage,
                Pwd = this.Pwd,
                data = this.data,
                statusParts = this.statusParts,
                nLogStatus = this.nLogStatus.Clone(),
                messageToSend = this.messageToSend

            };
        }

        public static implicit operator MyRequestInfo(sessionstate v)
        {
            throw new NotImplementedException();
        }
    }



}
