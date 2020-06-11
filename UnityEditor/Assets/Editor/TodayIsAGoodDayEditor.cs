using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 选择对TodayIsAGoodDay脚本进行定制
[CustomEditor(typeof(TodayIsAGoodDay))]
public class TodayIsAGoodDayEditor : Editor
{
    /// <summary>
    /// 重写TodayIsAGoodDay脚本的Inspector页面
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 取得目标脚本
        TodayIsAGoodDay t = target as TodayIsAGoodDay;

        // 根据ShowName决定是否显示Name
        if(t.ShowName)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));      
        }

        // 最终应用到目标脚本的inspector
        serializedObject.ApplyModifiedProperties();
    }
}
