using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//枚举用于追踪鼠标交互的各个阶段
public enum MPhase
{
    idle,
    down,
    drag
}

//储存鼠标在各个交互结构中的信息
[System.Serializable]
public class MouseInfo
{
    public Vector3 loc;         //鼠标靠近z=0的3D矢量
    public Vector3 screenLoc;   //鼠标的屏幕位置
    public Ray ray;             //从鼠标变换为3D空间的光束
    public float time;          //记录本地mouseInfo的时间信息
    public RaycastHit hitInfo;  //被光束击中的信息
    public bool hit;            //鼠标是否有单击操作

    //确认mouseRay是否有单击任何内容
    public RaycastHit Raycast()
    {
        hit = Physics.Raycast(ray, out hitInfo);
        return (hitInfo);
    }
    public RaycastHit Raycast(int mask)
    {
        hit = Physics.Raycast(ray, out hitInfo, mask);
        return (hitInfo);
    }
}


public class Mage : PT_MonoBehaviour
{
    static public Mage S;
    static public bool DEBUG = true;

    public float mTapTime = 0.1f;       //定义单击长度
    public float mDragDist = 5;         //定义拖动的最小像素距离

    public float activeScreenWidth = 1; //屏幕使用的%
    public bool _______________;

    public MPhase mPhase = MPhase.idle;
    public List<MouseInfo> mouseInfos = new List<MouseInfo>();

    private void Awake()
    {
        S = this;
        mPhase = MPhase.idle;
    }

    private void Update()
    {
        bool b0Down = Input.GetMouseButtonDown(0);
        bool b0Up = Input.GetMouseButtonUp(0);
        /* 动作类型：
         * 1.单击地面的指定点
         * 2.没用法术时从地面拖动到法师处
         * 3.用法术时沿着地面拖动
         * 4.在敌人面前单击做攻击操作
        */

        bool inActiveArea = (float)Input.mousePosition.x / Screen.width < activeScreenWidth;

        //因为单击有时会发生在单结构中，故用if语句
        //如果鼠标轮为空
        if(mPhase == MPhase.idle)
        {
            if (b0Down && inActiveArea)
            {
                
                mouseInfos.Clear();     //清空mouseInfo
                AddmouseInfo();         //添加第一个MouseInfo
                
                //如果有东西被点中
                if (mouseInfos[0].hit)
                {
                    MouseDown();
                    mPhase = MPhase.down;
                }
            }
        }

        //如果鼠标左键按下
        if (mPhase == MPhase.down)
        {
            AddmouseInfo();             //添加该结构的MouseInfo

            //如果左键释放
            if (b0Up)
            {
                MouseTap();             //单击动作
                mPhase = MPhase.idle;
            }else if (Time.time - mouseInfos[0].time > mTapTime)
            {
                //如果按下长度超过单击的长度，则为拖动
                float dragDist = (lastMouseInfo.screenLoc - mouseInfos[0].screenLoc).magnitude;

                if(dragDist>=mDragDist)
                    mPhase = MPhase.drag;
            }
        }

        //如果鼠标被拖动
        if (mPhase == MPhase.drag)
        {
            AddmouseInfo();

            //鼠标左键释放
            if (b0Up)
            {
                MouseDragUp();
                mPhase = MPhase.idle;
            }
            else
                MouseDrag();    //仍然处于拖动状态
        }
    }
    
    //提取Mouse信息，添加到mouseInfo并返回
    MouseInfo AddmouseInfo()
    {
        MouseInfo mInfo = new MouseInfo();
        mInfo.screenLoc = Input.mousePosition;
        mInfo.loc = Utils.mouseLoc;     //获取Z=0时的鼠标位置
        mInfo.ray = Utils.mouseRay;     //通过鼠标光标从MainCamera获取光束

        mInfo.time = Time.time;
        mInfo.Raycast();                //默认值为未加工的raycast

        //如果是第一个mouseInfo
        if (mouseInfos.Count == 0)
        {
            mouseInfos.Add(mInfo);      //为mouseInfos添加mInfo
        }
        else
        {
            float lastTime = mouseInfos[mouseInfos.Count - 1].time;

            //当最后一个mouseInfo超时
            if(mInfo.time != lastTime)
            {
                mouseInfos.Add(mInfo);  //为mouseInfos添加mInfo
            }
            //在一个结构中AddMouseInfo()可能被调用两次，故需要时间测试
        }
        return (mInfo);
    }

    //访问最新的MouseInfo
    public MouseInfo lastMouseInfo
    {
        get
        {
            if (mouseInfos.Count == 0)
                return (null);
            return (mouseInfos[mouseInfos.Count - 1]);
        }
    }

    //鼠标单击内容(拖动或单击)
    void MouseDown()
    {
        if (DEBUG)
            print("Mage.MouseDown()");
    }

    //单击某对象(按钮)
    void MouseTap()
    {
        if (DEBUG)
            print("Mage.MouseTap()");
    }

    //拖动鼠标穿过
    void MouseDrag()
    {
        if (DEBUG)
            print("Mage.MouseDrag()");
    }

    //鼠标拖动后释放
    void MouseDragUp()
    {
        if (DEBUG)
            print("Mage.MouseDragUp()");
    }
}