using ProtoBuf;
using System;

namespace GDT
{
    /// <summary>
    /// 客户端发往服务器 的 心跳包
    /// </summary>
    [Serializable, ProtoContract(Name = @"CSHeartBeat")]
    public partial class CSHeartBeat : CSPacketBase
    {
        public CSHeartBeat()
        {

        }

        public override int Id
        {
            get
            {
                return 1;
            }
        }

        public override void Clear()
        {

        }
    }
}
