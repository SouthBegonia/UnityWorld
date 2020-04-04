using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 聊天室_服务器端_TCP
{
    /// <summary>
    /// 客户端类
    /// </summary>
    public class Client
    {
        private Socket clientsocket;
        private Thread t;       //线程
        private byte[] data = new byte[1024];
        private int clientID = 0;

        public Client(Socket s, int clientID)
        {
            this.clientsocket = s;
            this.clientID = clientID;

            //启动一个线程，处理客户端数据接收
            t = new Thread(ReceiveMessage);
            t.Start();
        }

        private void ReceiveMessage()
        {
            //一直接收客户端数据
            while (true)
            {
                if (clientsocket.Poll(10, SelectMode.SelectRead))
                {
                    clientsocket.Close();
                    break;
                }

                int length = clientsocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length);
                
                //接收到数据时，要把这个数据分发到客户端
                //广播消息
                Program.BroadCastMessage(message);
                Console.WriteLine("收到 {0} 的消息：{1}", clientID, message);
            }
        }

        //发送消息给客户端
        public void SendMessage(string message,int clientID)
        {
            //message = clientID.ToString() + " " + message;
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientsocket.Send(data);
        }

        //判断是否连接
        public bool Connected
        {
            get { return clientsocket.Connected; }
        }
    }
}