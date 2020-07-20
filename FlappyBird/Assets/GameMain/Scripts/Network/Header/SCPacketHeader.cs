namespace GDT
{
    /// <summary>
    /// 服务器发往客户端 的 网络消息包头
    /// </summary>
    public sealed class SCPacketHeader : PacketHeaderBase
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
