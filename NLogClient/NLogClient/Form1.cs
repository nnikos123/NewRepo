using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLogClient
{
    
    public partial class Form1 : Form
    {
        byte[] buffer = new byte[1024];

        string sInData="";
        TcpClient socket;

        int NC = 2;
        
        ConcurrentDictionary<string,NLogClient> clients =new ConcurrentDictionary<string, NLogClient>();

        int counter = 0;
        public Form1()
        {
 


            InitializeComponent();

            string strFile = @"F:\01\G-Server\data.log ";

            StreamReader reader;
            

            //Setup a file stream reader to read the text file.

            reader = new StreamReader(new FileStream(strFile, FileMode.Open, FileAccess.Read));

            // While there is data to be read, read each line into a rich edit box control.

            while (reader.Peek() > -1)
            {
                sInData += reader.ReadToEnd();
            }
            reader.Close();
            // EndPoint serverAddress = new IPEndPoint(IPAddress.Parse("192.168.1.99"), 2021);

            sInData = sInData.Substring(0, sInData.Length - 2);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var serverAddress1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 555);
            NC = Convert.ToInt32(nclients.Value);
            for (int i = 0; i < NC; i++)
            {
                socket = new TcpClient();

                var NLogClient = new NLogClient()
                {
                    socket = socket,

                };
                               
                socket.Connect(serverAddress1);
                clients.TryAdd(socket.Client.LocalEndPoint.ToString(), NLogClient);
                socket.ReceiveBufferSize = 1024;
                socket.GetStream().BeginRead(buffer, 0, buffer.Length, receive, socket);
                
            }
            //EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 555);
            //socket2 = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //socket2.Connect(ep);
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            var enc = clients.GetEnumerator();
            while (enc.MoveNext())
            {
                enc.Current.Value.socket.Close();
            }
            clients.Clear();
        }
        private void receive(IAsyncResult ar)
        {
            var mc = (TcpClient)ar.AsyncState;
            if (!mc.Connected)
            {
                return;
            }
            NetworkStream ns = mc.GetStream();
            int bytesRead = 0;
            try
            {
                bytesRead = ns.EndRead(ar);
            }
            catch (Exception)
            {

                return;
            }
            if (bytesRead == 0)
            {
                log("bytesRead==0");
                return;
            }

            var str = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);

            NLogClient client;
            if(clients.TryGetValue(mc.Client.LocalEndPoint.ToString(), out client))
            {

                var newstr = client.resp + str;

                var idx1 = newstr.IndexOf("\r\n", 0);
                if (idx1 > -1)
                {
                    var reply = newstr.Substring(0, idx1);
                    client.resposes.Add(reply);
                    str = newstr.Remove(0,reply.Length+"\r\n".Length);
                    client.resp = str;
                    var str2 = mc.Client.LocalEndPoint.ToString() + "->" + reply;
                    log(str2);
                    Interlocked.Increment(ref counter);
                }
                                
            }
            

            mc.GetStream().BeginRead(buffer, 0, buffer.Length, receive, mc);


        }

        void log(string str)
        {
             
            if (this.InvokeRequired)
            {
                textBox1.BeginInvoke(new Action(() => textBox1.Text += str + "\r\n"));
               
            }
            else
                textBox1.Text += str + "\r\n";
            
        }
        private void btnSendStatus_Click(object sender, EventArgs e)
        {

            var message1= "" + (char)0 + "myPwd" + (char)0 + "si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=ON|DF=0" + (char)0;
            var SN = "000Z000";

            //var message = "000Z0000" + (char)0 + "" + (char)0 + "si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=ON" + (char)0;

            

            var enc = clients.GetEnumerator();
            int j = 0;
            while (enc.MoveNext())
            {
                var message = SN + (j++).ToString() + message1;
                var data = ASCIIEncoding.ASCII.GetBytes(message);

                Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        enc.Current.Value.socket.GetStream().Write(data, i, 1);
                        Thread.Sleep(0);
                    }
                }).Wait();
            }

             
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            var data = ASCIIEncoding.ASCII.GetBytes(sInData);

            //var enc = clients.GetEnumerator();
            //var enc = clients.GetEnumerator();
            //while (enc.MoveNext())
            //{
                //enc.Current.Value.socket.GetStream().Write(data, 0, data.Length);
            //}
            Parallel.ForEach(clients, (c) =>
            {
                 
                c.Value.socket.GetStream().Write(data, 0, data.Length);
            }
             
            );

            MessageBox.Show("sendDone");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var dataSection = ASCIIEncoding.ASCII.GetBytes(sInData);

            string message = "044D0906" + (char)0 + (char)0 + "si.3.6.|P=...|WD=...|PUT=...|S=...|V=...|A=ON" + (char)0;
            int ipos = 1;
            message += sInData.Substring(0, ipos);
            var data = ASCIIEncoding.ASCII.GetBytes(message);
            socket.GetStream().Write(data,0,data.Length);


            var message2 = sInData.Substring(ipos , 10);


            var m = message + message2;

            

            data = ASCIIEncoding.ASCII.GetBytes(message2);
            socket.GetStream().Write(data, 0, data.Length);

            var message3 = sInData.Substring(11);
            data = ASCIIEncoding.ASCII.GetBytes(message3);
            socket.GetStream().Write(data, 0, data.Length);

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            counter = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Text = counter.ToString();
        }

        private void btnSetupAck_Click(object sender, EventArgs e)
        {
            var enc = clients.GetEnumerator();
            while (enc.MoveNext())
            {
                Task.Factory.StartNew(() =>
                {
                    var data = ASCIIEncoding.ASCII.GetBytes("SETUP=1\0");
                    for (int i = 0; i < data.Length; i++)
                    {
                        enc.Current.Value.socket.GetStream().Write(data, i, 1);
                        Thread.Sleep(50);
                    }
                }).Wait();
            }
        }

        private void btnSendCaptum_Click(object sender, EventArgs e)
        {
            
            var m= "{{{{{{";
            m += "qwerty";
            m+= "}}}}}}";
            var data = ASCIIEncoding.ASCII.GetBytes(m);
            //var enc = clients.GetEnumerator();
            //var enc = clients.GetEnumerator();
            //while (enc.MoveNext())
            //{
            //enc.Current.Value.socket.GetStream().Write(data, 0, data.Length);
            //}
            Parallel.ForEach(clients, (c) =>
            {

                c.Value.socket.GetStream().Write(data, 0, data.Length);
            }

            );

            
        }

        private void btnSendReply_Click(object sender, EventArgs e)
        {
            var m = "";
            m += "ACQ ON\r\n";
            m += "OK\r\n";
            var data = ASCIIEncoding.ASCII.GetBytes(m);
            //var enc = clients.GetEnumerator();
            //var enc = clients.GetEnumerator();
            //while (enc.MoveNext())
            //{
            //enc.Current.Value.socket.GetStream().Write(data, 0, data.Length);
            //}
            Parallel.ForEach(clients, (c) =>
            {

                c.Value.socket.GetStream().Write(data, 0, data.Length);
            }

            );

        }
    }
}
