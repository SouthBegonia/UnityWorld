namespace GDT
{
    /// <summary>
    /// 客户端发往服务器 的 网络消息包基类
    /// </summary>
    public abstract class CSPacketBase : PacketBase
    {
        public override PacketType PacketType
        {
            get
            {
                return PacketType.ClientToServer;
            }
        }
    }
}
