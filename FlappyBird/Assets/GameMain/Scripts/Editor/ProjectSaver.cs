using UnityEditor;
using UnityEngine;

namespace GDT.Editor
{
    public static class ProjectSaver
    {
        /// <summary>
        /// 存储可序列化的资源。
        /// </summary>
        /// <remarks>等同于执行 Unity 菜单 File/Save Project。</remarks>
        [MenuItem("GDT/Save Assets &s")]
        public static void SaveAssets()
        {
#if UNITY_5_5_OR_NEWER

            DeviceModelConfig dmc = ScriptableObject.CreateInstance<DeviceModelConfig>();
            AssetDatabase.CreateAsset(dmc, "Assets/DeviceModelConfig.asset");

            AssetDatabase.SaveAssets();
#else
            EditorApplication.SaveAssets();
#endif
            Debug.Log("You have saved the serializable assets in the project.");
        }
    }
}
