using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Text;
using UnityEngine.UI;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    [Header("IP及端口")]
    public string ipaddress = "127.0.0.1";
    public int port = 1234;

    [Header("UI组件")]
    public Text Input;      //InputField中显示输入的text
    public Text Label;      //聊天室显示的text
    public Text userName;   //聊天者名字

    private Socket client_socket;
    private Thread t;
    private byte[] data = new byte[1024];   //数据容器
    private string message = "";            //消息容器

    void Start()
    {
        if (userName.text == "" || userName.text == null)
            userName.text = "匿名";

        OnConnectedToServer();
    }

    void Update()
    {
        if (message != "" && message != null)
        {
            Label.text += "\n" + message;
            message = "";
        }
    }

    /// <summary>
    /// 连接至服务器函数
    /// </summary>
    public void OnConnectedToServer()
    {
        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client_socket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));

        t = new Thread(ReceiveMessage);     //在单独线程中接收消息
        t.Start();
    }

    /// <summary>
    /// 从服务器端接收消息函数
    /// </summary>
    public void ReceiveMessage()
    {
        while (true)
        {
            if (client_socket.Poll(10, SelectMode.SelectRead))
            {
                client_socket.Close();
                break;
            }
            int length = client_socket.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
            //Label.text += "\n" + message;     unity不允许在单独的线程里去操控其组件
        }
    }

    /// <summary>
    /// 发送消息button，规范化需要发送的消息
    /// </summary>
    public void OnSendButtonClick()
    {
        string str = "";
        if (userName.text == null || userName.text == "")
            str = "匿名：" + Input.text;
        else
            str =  userName.text + "：" + Input.text;

        SendMessage(str);
        Input.GetComponentInParent<InputField>().text = "";
    }

    /// <summary>
    /// 发送消息到服务器端函数
    /// </summary>
    /// <param name="message">需要发送的消息</param>
    public new void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        client_socket.Send(data);
    }

    private void OnApplicationQuit()
    {
        client_socket.Shutdown(SocketShutdown.Both);
        client_socket.Close();
    }

    /// <summary>
    /// 连接到服务器button
    /// </summary>
    public void ConnectNetButtonClick()
    {
        if (userName.text == "" || userName.text == null)
            userName.text = "匿名";

        OnConnectedToServer();
    }

    /// <summary>
    /// 断开与服务器连接button
    /// </summary>
    public void CloseNetButtonClick()
    {
        client_socket.Shutdown(SocketShutdown.Both);
        client_socket.Close();
    }
}