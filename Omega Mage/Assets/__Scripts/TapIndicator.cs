using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----单击指示标记-----*/
public class TapIndicator : PT_Mover
{
    public float lifeTime = 0.5f;       //持续时间
    public float[] scales;              //插入的比例
    public Color[] colors;              //插入的颜色

    private void Awake()
    {
        scale = Vector3.zero;           //初始时隐藏指示器
    }

    private void Start()
    {
        PT_Loc pLoc;
        List<PT_Loc> locs = new List<PT_Loc>();
        Vector3 tpos = pos;
        tpos.z = -0.1f;

        //在检查器中必须保持相同的比例和颜色
        for(int i = 0; i < scales.Length; i++)
        {
            pLoc = new PT_Loc();
            pLoc.scale = Vector3.one * scales[i];
            pLoc.pos = tpos;
            pLoc.color = colors[i];
            locs.Add(pLoc);

            //回调机制:一种函数授权机制,当移动停止时调用void function()函数
            //完成时调用Callbackmethod()
            callback = CallbackMethod;

            //通过贝赛尔曲线传递一系列PT_Locs和持续时间来初始化移动
            PT_StartMove(locs, lifeTime);
        }

        void CallbackMethod()
        {
            Destroy(gameObject);        //完成移动,销毁gameobject
        }
    }
}
