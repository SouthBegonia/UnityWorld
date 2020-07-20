using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GDT
{
    /// <summary>
    /// 自定义数据组件
    /// </summary>
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 画质模式配置
        /// </summary>
        [SerializeField]
        private DeviceModelConfig m_DeviceModelConfig = null;

        /// <summary>
        /// 项目构建信息文本
        /// </summary>
        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        /// <summary>
        /// 默认字典
        /// </summary>
        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        /// <summary>
        /// 项目构建信息
        /// </summary>
        private BuildInfo m_BuildInfo = null;

        public DeviceModelConfig DeviceModelConfig
        {
            get
            {
                return m_DeviceModelConfig;
            }
        }

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }

        /// <summary>
        /// 初始化项目构建信息
        /// </summary>
        public void InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }

            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                return;
            }

            GameEntry.Base.GameVersion = m_BuildInfo.GameVersion;
            GameEntry.Base.InternalApplicationVersion = m_BuildInfo.InternalVersion;
        }

        /// <summary>
        /// 初始化默认字典
        /// </summary>
        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!GameEntry.Localization.ParseDictionary(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }
        }
    }
}

