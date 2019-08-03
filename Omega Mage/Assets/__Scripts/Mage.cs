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

//ElementType枚举
public enum ElementType
{
    earth,      //0
    water,      //1
    air,        //2
    fire,       //3
    aether,     //4
    none        //5
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
    public GameObject tapIndicatorPrefab;//单击指示器的Prefab
    public float mDragDist = 5;         //定义拖动的最小像素距离

    public float activeScreenWidth = 0.75f; //屏幕使用的%

    public float speed = 2;             //Mage的运动速度
    public bool _______________;

    public MPhase mPhase = MPhase.idle;
    public List<MouseInfo> mouseInfos = new List<MouseInfo>();
  
    public bool walking = false;        //Mage的运动状态
    public Vector3 walkTarget;          //Mage目的地
    public Transform characterTrans;    //Mage的组件

    private void Awake()
    {
        S = this;
        mPhase = MPhase.idle;

        //查询characterTrans以轮转Face()
        characterTrans = transform.Find("CharacterTrans");
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

        //Mage移动到最新的mouseInfo位置
        WalkTo(lastMouseInfo.loc);

        //显示玩家单击的地方
        ShowTap(lastMouseInfo.loc);
    }

    //拖动鼠标穿过
    void MouseDrag()
    {
        if (DEBUG)
            print("Mage.MouseDrag()");

        //继续前往当前mouseInfo位置
        WalkTo(mouseInfos[mouseInfos.Count - 1].loc);
    }

    //鼠标拖动后释放
    void MouseDragUp()
    {
        if (DEBUG)
            print("Mage.MouseDragUp()");

        //当拖拽结束时停止前进
        StopWalking();
    }

    //前进到指定位置，z=0
    public void WalkTo(Vector3 xTarget)
    {
        walkTarget = xTarget;       //设置当前目的地
        walkTarget.z = 0;           //锁Z轴
        walking = true;             //移动状态
        Face(walkTarget);           //面向walkTarget的方向
    }

    //面向焦点
    public void Face(Vector3 poi)
    {
        Vector3 delta = poi - pos;                                  //查找焦点的向量
        float rZ = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);   //获取z的旋转量
        characterTrans.rotation = Quaternion.Euler(0, 0, rZ);       //设置characterTrans的旋转量(Mage并未旋转)
    }

    //停止Mage前进
    public void StopWalking()
    {
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //若Mage前进
        if (walking)
        {
            //若Mage非常靠近目的地，则停在当前坐标
            if((walkTarget-pos).magnitude < speed * Time.fixedDeltaTime)
            {
                pos = walkTarget;
                StopWalking();
            }
            else
            {   //否则向目的地移动
                GetComponent<Rigidbody>().velocity = (walkTarget - pos).normalized * speed;
            }
        }
        else
        {   //否则静止
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        /* 向目的地运动过程出现的bug：
        问题描述：点击了有效的目的地后，Mage仅移动了较短距离后就仿佛被碰撞阻碍运动，最终停止
        问题分析：当我把Mage稍微调高不与地面TileAnchor接触时，就完全解决了此问题；
                 是否是TileAnchor物体上的Collider，与Mage碰撞产生减速呢？
                 那摩擦力从哪来？Mage上的AngularDrag吗？ --->否，全部调0后并未改变
                 总之，很可能就是Mage模型与TileAnchor物体的碰撞器细微接触问题。
        解决方案：稍微调节Mage高度使其不与TileAnchor接触；
                 或稍微缩小Mage->CharacterTrans->View_Character->legs的collider的范围
         
         
         */
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGo = coll.gameObject;

        Tile ti = otherGo.GetComponent<Tile>();
        if (ti != null)
        {
            //height>0说明为墙壁，无法穿过
            if (ti.height > 0)
                StopWalking();
        }
    }

    //显示玩家单击的地方
    public void ShowTap(Vector3 loc)
    {
        GameObject go = Instantiate(tapIndicatorPrefab) as GameObject;
        go.transform.position = loc;
    }
}