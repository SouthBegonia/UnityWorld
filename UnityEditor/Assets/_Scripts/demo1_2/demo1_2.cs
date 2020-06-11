using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class demo1_2 : MonoBehaviour
{
    [MenuItem("调试/查看版本信息",false,0)]
    static void PrintSomething()
    {
        // 注意：仅有静态函数才可使用该属性
        Debug.Log("当前Unity版本：" + Application.unityVersion);      
    }
}
