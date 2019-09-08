using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlong : MonoBehaviour
{
    //涉及竞态条件(Race Conditions)，若在两个激活脚本内同时使用Update()存在调用顺序不确定问题
    //CountItHigher.cs 内的Update先执行，此处 LateUpdate后于它执行
    private void LateUpdate()
    {
        //调用绑定在 CountItHigher.cs 中 CountItHigher 类 
        CountItHigher cih = this.gameObject.GetComponent<CountItHigher>();

        //调用组件检测
        if (cih != null)
        {
            //访问属性 currentNum 返回 _num值给 tx
            float tx = cih.currentNum / 10f;
            Vector3 tempLoc = pos;
            tempLoc.x = tx;
            pos = tempLoc;
        }
        else { Debug.Log("Error!"); }
    }

    //设置属性 pos
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }
}
