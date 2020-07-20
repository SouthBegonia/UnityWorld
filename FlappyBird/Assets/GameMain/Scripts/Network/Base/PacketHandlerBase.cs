using GameFramework.Network;

namespace GDT
{
    /// <summary>
    /// 网络消息包处理器基类
    /// </summary>
    public abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract int Id
        {
            get;
        }

        public abstract void Handle(object sender, Packet packet);
    }
}
