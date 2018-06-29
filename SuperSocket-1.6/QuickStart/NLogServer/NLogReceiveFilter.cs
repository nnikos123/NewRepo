using System;
using System.Diagnostics;
using NLogServer.Protocol;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;

namespace NLogServer
{
     
    /// <summary>
    /// It is the kind of protocol that
    /// the first two bytes of each command are { 0x68, 0x68 }
    /// and the last two bytes of each command are { 0x0d, 0x0a }
    /// and the 16th byte (data[15]) of each command indicate the command type
    /// if data[15] = 0x10, the command is a keep alive one
    /// if data[15] = 0x1a, the command is position one
    /// </summary>
    class NLogReceiveFilter : NLogReceiveFilterA<BinaryRequestInfo>
    {
        private readonly static byte[] BeginMark = new byte[] { 0x68, 0x68 };
        private readonly static byte[] EndMark = new byte[] { 0x0d, 0x0a };

        public NLogReceiveFilter()
            : base(BeginMark, EndMark)
        {

        }

        protected override BinaryRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            string sData = "";

            var Data = readBuffer.CloneRange(offset, length);

            for (var i = 0; i < Data.Length; i++)
                    sData = sData + Convert.ToChar(Data[i]);


            //if (tag.Substring(0, 3) == "SN?")
            {

            }


            //Debug.WriteLine(xNow() + " Received from " + Index + ": " + sData);

             
            var info = new BinaryRequestInfo("10", readBuffer.CloneRange(offset,length));

            return info;
            //return new BinaryRequestInfo(BitConverter.ToString(readBuffer, offset + 15, 1), readBuffer.CloneRange(offset, length));
        }
    }
}
