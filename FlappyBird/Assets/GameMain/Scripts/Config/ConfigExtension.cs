using GameFramework;
using UnityGameFramework.Runtime;

namespace GDT
{
    public static class ConfigExtension
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="userData">用户自定义数据</param>
        public static void LoadConfig(this ConfigComponent configComponent, string configName, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("Config name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName, AssetUtility.GetConfigAsset(configName), userData);
        }
    }
}
