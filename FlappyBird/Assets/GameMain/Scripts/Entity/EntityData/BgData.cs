using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    /// <summary>
    /// 背景实体数据
    /// </summary>
    public class BgData : EntityData
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed { get; private set; }

        /// <summary>
        /// 到达此目标时产生新的背景实体
        /// </summary>
        public float SpawnTarget { get; private set; }

        /// <summary>
        /// 到达此目标时隐藏自身
        /// </summary>
        public float HideTarget { get; private set; }

        /// <summary>
        /// 移动起始点
        /// </summary>
        public float StartPostion { get; private set; }

        public BgData(int entityId, int typeId, float moveSpeed, float startPostion) : base(entityId, typeId)
        {
            MoveSpeed = moveSpeed;
            SpawnTarget = -8.66f;
            HideTarget = -36.4f;
            StartPostion = startPostion;
        }
    }
}

