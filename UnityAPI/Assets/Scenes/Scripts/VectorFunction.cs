using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFunction : MonoBehaviour
{
    public Vector3 a;
    public Vector3 b;

    private void Start()
    {
        a = new Vector3(1, 2, 1);
        b = new Vector3(5, 6, 0);
    }

    private void OnGUI()
    {
        //1.点乘积的返回值
        float c = Vector3.Dot(a, b);

        //向量a,b的夹角，得到的是弧度制，将其转换为角度
        //normalized：返回单位向量，用以表示该向量的方向
        //Mathf.Rad2Deg：弧度制转换为角度，本身为一常量值 360/(2*Pi)
        float angle = Mathf.Acos(Vector3.Dot(a.normalized, b.normalized)) * Mathf.Rad2Deg;
        Debug.Log("a.normalized = " + a.normalized);
        Debug.Log("B.normalized = " + b.normalized);

        GUILayout.Label("向量a,b的点乘积为： " + c);
        GUILayout.Label("向量a,b的夹角为： " + angle);

        //2.交叉乘积的返回值
        Vector3 e = Vector3.Cross(a, b);
        Vector3 d = Vector3.Cross(b, a);

        //向量a,b的夹角
        angle = Mathf.Asin(Vector3.Distance(Vector3.zero, Vector3.Cross(a.normalized, b.normalized))) * Mathf.Rad2Deg;

        GUILayout.Label("向量a*b为： " + e);
        GUILayout.Label("向量b*a为： " + d);
        GUILayout.Label("向量a,b的夹角为： " + angle);
    }
}
