using SuperSocket.SocketBase.Protocol;

namespace NLogServer
{
    public interface IDBStoreMessage
    {
        MyRequestInfo requestInfo { get; set; }
    }
    public class MyRequestInfoDB : IDBStoreMessage
    {
        public MyRequestInfoDB()
        {
        }

       
        public MyRequestInfo requestInfo { get; set; }
    }
}