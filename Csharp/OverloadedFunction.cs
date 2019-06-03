using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadedFunction : MonoBehaviour
{
    /*函数重载：根据所传递参数类型和数量不同而做出不同操作的功能*/

    private void Awake()
    {
        /*根据传递参数类型不同，调用不同的Add()*/
        print(Add(1.0f, 2.5f));

        print(Add(new Vector3(1, 0, 0), new Vector3(0, 1, 0)));

        Color colorA = new Color(0.5f, 1, 0, 1);
        Color colorB = new Color(0.25f, 0.33f, 0, 1);
        print(Add(colorA, colorB));
        // 输出：RGBA(0.750, 1.000, 0.000, 1.000)
    }

    float Add(float f0,float f1)
    {
        return (f0 + f1);
    }

    Vector3 Add(Vector3 v0,Vector3 v1)
    {
        return (v0 + v1);
    }

    Color Add(Color c0,Color c1)
    {
        /* RGBA colors 格式：(r,g,b,a) 
        r,g,b 表示红绿蓝 的颜色分量，范围0~1
        a 表示颜色的透明度，范围0~1，0透明，1不透明
        */
        float r, g, b;
        float a;

        //Mathf.Min()可以接收任何类型的参数，返回接收参数的最小值
        r = Mathf.Min(c0.r + c1.r, 1.0f);
        g = Mathf.Min(c0.g + c1.g, 1.0f);
        b = Mathf.Min(c0.b + c1.b, 1.0f);
        a = Mathf.Min(c0.a + c1.a, 1.0f);

        return (new Color(r, g, b, a));
    }

    private void Start()
    {
        /*Params 关键字的应用*/
        print(Add2(1));
        print(Add2(1, 2));
        print(Add2(1, 2, 3));
        print(Add2(1, 2, 3, 4));
        print(Add3("a", "b", "c"));

        /*实现递归*/
        print(Fac(-1));
        print(Fac(0));
        print(Fac(5));
    }

    //params 关键字：将函数接收的同类型的所有参数合并到一个int数组 ints
    int Add2(params int[] ints)
    {
        int sum = 0;
        foreach(int i in ints)
        {
            sum += i;
        }
        return (sum);
    }

    string Add3(params string[] str)
    {
        string s="";
        foreach(string temp in str)
        {
            s += temp;
        }
        return s;
    }

    //递归函数
    int Fac(int n)
    {
        if (n < 0)
            return (0);
        if (n == 0)
            return (1);
        int result = n * Fac(n - 1);

        return (result);
    }
}
