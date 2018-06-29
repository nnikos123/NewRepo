using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace NLogServer
{
    public class myServer
    {
        private myNLogClientAppServer NLogClientServer;
        private IServerConfig m_Config, m_ConfigNLogClient;

        private MyNLogDevicesAppServer NLogDevicesServer;

        public static ConcurrentDictionary<string, MyRequestInfo> NLogDevices = new ConcurrentDictionary<string, MyRequestInfo>();
        
        public void Setup()
        {
            //SecureTestServer.config


            ICertificateConfig certificate = new CertificateConfig()
            {
                FilePath = @"c:\Projects\EYDAP\Code\SuperSocket-1.6\supersocket.pfx",
                Password = "supersocket",
                ClientCertificateRequired = false,
            };


            m_Config = new ServerConfig
            {
                 
                Port = 555,
                Ip = "Any",                
                MaxConnectionNumber = 10000,
                Mode = SocketMode.Tcp,
                Name = "NLogDevices",
                MaxRequestLength=50000,
                 
                 //Certificate=certificate,
                  //Security="tls"
            };
            m_ConfigNLogClient = new ServerConfig
            {

                Port = 8200,
                Ip = "Any",
                MaxConnectionNumber = 10,
                Mode = SocketMode.Tcp,
                Name = "NLogClient",
                MaxRequestLength = 50000
                //Certificate=certificate,
                //Security="tls"
            };


            NLogDevicesServer = new MyNLogDevicesAppServer(NLogClientServer);
            NLogDevicesServer.Setup(new RootConfig(), m_Config, logFactory: new ConsoleLogFactory());

            NLogClientServer = new myNLogClientAppServer(NLogDevicesServer);
            NLogClientServer.Setup(new RootConfig(), m_ConfigNLogClient, logFactory: new ConsoleLogFactory());
            NLogDevicesServer.ClientAppServer = NLogClientServer;
            Global.serversDic.TryAdd(NLogDevicesServer.Name, NLogDevicesServer);
            Global.serversDic.TryAdd(NLogClientServer.Name, NLogDevicesServer);
        }
        public void StartServer()
        {
            NLogDevicesServer.Start();
            NLogClientServer.Start();
        }


        public void StopServer()
        {
            NLogDevicesServer.Stop();
        }
        private static byte[] m_StartMark = new byte[] { 0x68, 0x68 };
        private static byte[] m_EndMark = new byte[] { 0x0d, 0x0a };
        public void TestCustomProtocol()
        {


            string strFile = @"F:\01\G-Server\data.log ";

            StreamReader reader;
            string sInData = "";

            //Setup a file stream reader to read the text file.

            reader = new StreamReader(new FileStream(strFile, FileMode.Open, FileAccess.Read));

            // While there is data to be read, read each line into a rich edit box control.

            while (reader.Peek() > -1)
            {
                sInData += reader.ReadToEnd();
            }
            reader.Close();
            //FileSaveTest(1, sInData)

            // EndPoint serverAddress = new IPEndPoint(IPAddress.Parse("192.168.1.99"), 2021);

            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2021);

            Random rd = new Random();


            byte[] b = new byte[5];

            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverAddress);
                socket.Receive(b);


                for (int i = 0; i < 10; i++)
                {
                    List<byte> dataSource = new List<byte>();

                    int messageRepeat = rd.Next(1, 5);

                    StringBuilder sb = new StringBuilder();

                    for (int j = 0; j < messageRepeat; j++)
                    {
                        sb.Append(Guid.NewGuid().ToString().Replace("-", string.Empty));
                    }

                    dataSource.AddRange(m_StartMark);
                    dataSource.AddRange(Encoding.ASCII.GetBytes(sb.ToString(0, rd.Next(20, sb.Length))));
                    dataSource.AddRange(m_EndMark);

                    byte[] data = dataSource.ToArray();

                    if (i % 2 == 0)
                        data[15] = 0x10;
                    else
                        data[15] = 0x1a;

                    string message = "SN?" + (char)0 + "044D0906" + (char)0 + "si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=ON";
                    message = "044D0906" + (char)0 + (char)0 + "si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=ON" + (char)0;
                    data = ASCIIEncoding.ASCII.GetBytes(message);

                    socket.Send(data);
                    sInData=sInData.Substring(0, sInData.Length - 2);
                    data = ASCIIEncoding.ASCII.GetBytes(sInData);
                    Thread.Sleep(1000);
                    //socket.Send(data);

                    byte[] response = new byte[100];

                    int read = 0;
                    string mres = "";
                    while (read < response.Length)
                    {
                        read += socket.Receive(response, read, response.Length - read, SocketFlags.None);
                        mres = ASCIIEncoding.ASCII.GetString(response, 0, read);

                    }


                }
            }
        }
    }
}
