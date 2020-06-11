using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class MyFirstWindow : EditorWindow
{
    string bugReporterName = "";
    string description = "";
    GameObject buggyGameObject;

    MyFirstWindow()
    {
        this.titleContent = new GUIContent("Bug Reporter");
    }

    [MenuItem("调试/Bug Reporter",false,1)]
    static void ShowWindow()
    {
        // 打开Bug Repoter的窗口
        EditorWindow.GetWindow(typeof(MyFirstWindow));
    }

    private void OnGUI()
    {
        // 竖直方向
        GUILayout.BeginVertical();

        // 主标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Bug Reporter");
        
        // Bug Name
        GUILayout.Space(10);
        bugReporterName = EditorGUILayout.TextField("Bug Name", bugReporterName);

        // 当前场景
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUILayout.Label("当前场景：" + EditorSceneManager.GetActiveScene().name);

        // 当前时间
        GUILayout.Space(10);
        GUILayout.Label("时间：" + System.DateTime.Now);

        // 目标对象
        GUILayout.Space(10);
        buggyGameObject = (GameObject)EditorGUILayout.ObjectField("Buggy Game Object", buggyGameObject, typeof(GameObject), true);
        /* EditorGUILayout.ObjectField(string label, Object obj, System.Type objType, bool allowSceneObjects)
         * Label：字段名称
         * obj：字段显示的物体
         * objType：显示物体的类型
         * allowSceneObjects：是否允许选择场景内任意物体
         */

        // 描述文本区域
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("描述", GUILayout.MaxWidth(80));
        description = EditorGUILayout.TextArea(description, GUILayout.MaxHeight(75));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // 记录BUG按钮
        if (GUILayout.Button("记录BUG"))
        {
            SaveBug();
        }

        // 保存截图按钮
        if (GUILayout.Button("保存截图"))
        {
            SaveBugWithScreenshot();
        }

        GUILayout.EndVertical();
    }

    public void SaveBug()
    {
        Directory.CreateDirectory("Assets/BugRepots/" + bugReporterName);
        StreamWriter sw = new StreamWriter("Assets/BugRepots/" + bugReporterName + "/" + bugReporterName + ".txt");
        sw.WriteLine("Bug名称：" + bugReporterName);
        sw.WriteLine("出现时间：" + System.DateTime.Now.ToString());
        sw.WriteLine("出现场景：" + EditorSceneManager.GetActiveScene().name);
        sw.WriteLine("BUG描述：" + description);
        sw.Close();
    }

    public void SaveBugWithScreenshot()
    {
        Directory.CreateDirectory("Assets/BugRepots/" + bugReporterName);
        StreamWriter sw = new StreamWriter("Assets/BugRepots/" + bugReporterName + "/" + bugReporterName + ".txt");
        sw.WriteLine("Bug名称：" + bugReporterName);
        sw.WriteLine("出现时间：" + System.DateTime.Now.ToString());
        sw.WriteLine("出现场景：" + EditorSceneManager.GetActiveScene().name);
        sw.WriteLine("BUG描述：" + description);
        sw.Close();

        UnityEngine.ScreenCapture.CaptureScreenshot("Assets/BugRepots/" + bugReporterName + "/" + bugReporterName + "_ScreenShot" + ".png");       
    }
}
