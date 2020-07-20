using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDT
{
    /// <summary>
    /// 项目构建信息
    /// </summary>
    public class BuildInfo
    {
        /// <summary>
        /// 游戏版本
        /// </summary>
        public string GameVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 内部版本
        /// </summary>
        public int InternalVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 版本检查地址
        /// </summary>
        public string CheckVersionUrl
        {
            get;
            set;
        }
    }
}

