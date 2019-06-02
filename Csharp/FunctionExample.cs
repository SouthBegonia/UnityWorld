using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionExample : MonoBehaviour
{
    /*备注：在 FunctionExample 类内，其间任何函数的定义无相后之分；
     因为C#会在代码运行前就检查该类内所有函数定义*/

    //统计执行了几帧
    //作用域为整个 FubctionExample 类模块，其中所有函数都可调用
    public int numTimesCalled = 0;

    public List<GameObject> reallyLongList;
        
    void Update()
    {
        //numTimesCalled++;
        //CountUpdates();
    }

    //统计Update执行次数
    void CountUpdates()
    {
        string outputMessage = "Update 次数： " + numTimesCalled;
        print(outputMessage);
    }

    private void Awake()
    {
        //移动对象 Phil 到坐标原点
        MoveToOrigin("Phil");

        //Unity中修改某单一轴线的值：
        GameObject go = GameObject.Find("Phil");
        //错误示范： go.transform.position.x = 0f;
        //正确用法：
        Vector3 tempPos;
        tempPos = go.transform.position;
        tempPos.x = 0f;
        go.transform.position = tempPos;
    }

    //遍历表内是否存在名为 theName 的对象，存在则修改坐标到原点
    void MoveToOrigin(string theName)
    {
        foreach(GameObject go in reallyLongList)
        {
            if(go.name == theName)
            {
                go.transform.position = Vector3.zero;
                return;
            }
        }
    }
}
