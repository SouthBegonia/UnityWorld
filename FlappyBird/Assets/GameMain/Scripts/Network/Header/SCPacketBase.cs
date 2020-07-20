namespace GDT
{
    /// <summary>
    /// 服务器发往客户端 的 网络消息包基类
    /// </summary>
    public abstract class SCPacketBase : PacketBase
    {
        public override PacketType PacketType
        {
            get
            {
                return PacketType.ServerToClient;
            }
        }
    }
}
