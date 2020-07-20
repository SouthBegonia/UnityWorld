namespace GDT
{
    /// <summary>
    /// 客户端发往服务器 的 网络消息包头基类
    /// </summary>
    public sealed class CSPacketHeader : PacketHeaderBase
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
