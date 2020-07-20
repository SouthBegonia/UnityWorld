using GameFramework;
using UnityGameFramework.Runtime;

namespace GDT
{
    public static class LocalizationExtension
    {
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryName">字典名称</param>
        /// <param name="userData">用户自定义数据</param>
        public static void LoadDictionary(this LocalizationComponent localizationComponent, string dictionaryName, object userData = null)
        {
            if (string.IsNullOrEmpty(dictionaryName))
            {
                Log.Warning("Dictionary name is invalid.");
                return;
            }

            localizationComponent.LoadDictionary(dictionaryName, AssetUtility.GetDictionaryAsset(dictionaryName), userData);
        }
    }
}
