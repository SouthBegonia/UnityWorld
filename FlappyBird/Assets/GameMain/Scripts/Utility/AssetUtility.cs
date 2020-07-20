namespace GDT
{
    /// <summary>
    /// 资源路径相关的实用函数集
    /// </summary>
    public static class AssetUtility
    {
        /// <summary>
        /// 获取配置资源的完整路径
        /// </summary>
        public static string GetConfigAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Configs/{0}.txt", assetName);
        }

        /// <summary>
        /// 获取数据表资源的完整路径
        /// </summary>
        public static string GetDataTableAsset(string assetName)
        {
            return string.Format("Assets/GameMain/DataTables/{0}.txt", assetName);
        }

        /// <summary>
        /// 获取字典资源的完整路径
        /// </summary>
        public static string GetDictionaryAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.xml", GameEntry.Localization.Language.ToString(), assetName);
        }

        /// <summary>
        /// 获取字体资源的完整路径
        /// </summary>
        public static string GetFontAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Localization/{0}/Fonts/{1}.ttf", GameEntry.Localization.Language.ToString(), assetName);
        }

        /// <summary>
        /// 获取场景资源的完整路径
        /// </summary>
        public static string GetSceneAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }

        /// <summary>
        /// 获取音乐资源的完整路径
        /// </summary>
        public static string GetMusicAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Music/{0}.mp3", assetName);
        }

        /// <summary>
        /// 获取声音资源的完整路径
        /// </summary>
        public static string GetSoundAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Sounds/{0}.wav", assetName);
        }

        /// <summary>
        /// 获取实体资源的完整路径
        /// </summary>
        public static string GetEntityAsset(string assetName)
        {
            return string.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }

        /// <summary>
        /// 获取界面资源的完整路径
        /// </summary>
        public static string GetUIFormAsset(string assetName)
        {
            return string.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }

        /// <summary>
        /// 获取界面图片资源的完整路径
        /// </summary>
        public static string GetUISpriteAsset(string assetName)
        {
            return string.Format("Assets/GameMain/UI/UISprites/{0}.png", assetName);
        }

        /// <summary>
        /// 获取界面声音资源的完整路径
        /// </summary>
        public static string GetUISoundAsset(string assetName)
        {
            return string.Format("Assets/GameMain/UI/UISounds/{0}.wav", assetName);
        }
    }
}
