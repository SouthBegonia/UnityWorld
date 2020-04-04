using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace 聊天室_服务器端_TCP
{
    class Program
    {
        //存放客户端
        static List<Client> clientList = new List<Client>();
        static int clientID = 0;

        //广播消息
        public static void BroadCastMessage(string message)
        {
            //创建未连接的list
            var notConnectedList = new List<Client>();
            foreach (Client client in clientList)
            {
                if (client.Connected)   //给所有连接的客户端发送消息;
                    client.SendMessage(message, clientID);
                else    //把未连接的客户端加入list
                {
                    notConnectedList.Add(client);
                }
            }

            //移除未连接的客户端
            foreach (var temp in notConnectedList)
            {
                clientList.Remove(temp);
            }
        }

        static void Main(string[] args)
        {
            //实例化服务器端Socket并指定IP地址类型（IPV4）,套接字类型（流类型），通信协议（TCP）
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //绑定终端（设置IP地址，端口号（范围为0-65535之间随意取，为避免端口号已被其他软件占用，可以取大点））
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));

            //开始监听，等待客户端接入，接入后执行后续操作
            tcpServer.Listen(100);
            Console.WriteLine("开始监听");

            //循环等待客户端接入
            while (true)
            {

                Socket ClientSocket = tcpServer.Accept();
                Console.WriteLine("客户端{0}连接进来了！",clientID);
                Client client = new Client(ClientSocket, clientID);
                clientList.Add(client);
                clientID++;
            }
        }
    }
}