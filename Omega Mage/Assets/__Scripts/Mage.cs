using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//枚举用于追踪鼠标交互的各个阶段
public enum MPhase
{
    idle,       //空
    down,       //按下
    drag        //拖拽
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
    static public Mage S = new Mage();
    static public bool DEBUG = true;

    public float mTapTime = 0.1f;           //定义单击长度
    public GameObject tapIndicatorPrefab;   //单击指示器的Prefab
    public float mDragDist = 5;             //定义拖动的最小像素距离

    public float activeScreenWidth = 0.75f; //屏幕使用的%

    public float speed = 2;                 //Mage的运动速度

    public GameObject[] elementPrefabs;     //Element_Sphere的预制体
    public float elementRotDist = 0.5f;     //旋转半径
    public float elementRotSpeed = 0.5f;    //旋转周期

    public int maxNumSelectedElements = 1;  //最大选择道具数为1

    public Color[] elementColors;           //线条颜色
    public float lineMinDelta = 0.1f;       //线条2坐标之间的最大最小距离
    public float lineMaxDelta = 0.5f;
    public float lineMaxLength = 6f;        //线条最大长度(限制法术施放长度)

    public GameObject fireGroundSpellPrefab;//火焰魔法
    public GameObject waterGroundSpellPrefab;//水魔法
    public GameObject airGroundSpellPrefab; //雷魔法
    public GameObject earthGroundSpellPrefab;//恢复魔法

    public float health = 3;                //生命值
    public float healthMax = 3;             //最大生命值
    //public List<GameObject> Allhealth;      //3个心形生命点
    public Image healthUI;                  //单个心型血条UI    

    public float damageTime = -100;         //切换场景一定时间Mage不会受到攻击
    public float knockbackDist = 1;         //后退距离
    public float knockbackDur = 0.5f;       //后退秒数
    public float invincibleDur = 0.5f;      //战斗秒数
    public int invTimesToBlink = 4;         //战斗时闪烁

    public bool ____________________________________;

    private bool invincibleBool = false;    //Mage是否在战斗
    private bool knockbackBool = false;     //Mage被击退?
    private Vector3 knockbarDir;            //击退距离
    private Transform viewCharacterTrans;

    protected Transform spellAnchor;        //所有法术的父transform

    public float totalLineLength;
    public List<Vector3> linePts;           //线条显示的坐标点
    protected LineRenderer liner;           //应用LineRenderer组件
    protected float lineZ = -0.1f;          //线条的Z depth

    public MPhase mPhase = MPhase.idle;     //鼠标初始状态为idle
    public List<MouseInfo> mouseInfos = new List<MouseInfo>();
    public string actionStartTag;           //["Mage","Ground","Enemy"]

    public bool walking = false;            //Mage的运动状态
    public Vector3 walkTarget;              //Mage目的地
    public Transform characterTrans;        //Mage的组件

    public List<Element> selectedElements = new List<Element>();

    private void Awake()
    {
        S = this;
        mPhase = MPhase.idle;

        //查询characterTrans以轮转Face()
        characterTrans = transform.Find("CharacterTrans");

        //获取LineRenderer组件并禁用
        liner = GetComponent<LineRenderer>();
        liner.enabled = false;

        //创建空游戏对象命名为 Spell Anchor
        GameObject saGo = new GameObject("Spell Anchor");
        spellAnchor = saGo.GetComponent<Transform>();

        viewCharacterTrans = characterTrans.Find("View_Character");
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
        if (mPhase == MPhase.idle)
        {
            if (b0Down && inActiveArea)
            {
                mouseInfos.Clear();     //清空mouseInfo
                AddmouseInfo();         //添加第一个MouseInfo

                //如果有东西被点中
                if (mouseInfos[0].hit)
                {
                    //Debug.Log("有物体被鼠标点中");
                    MouseDown();
                    mPhase = MPhase.down;
                }
            }
        }

        //如果鼠标左键按下
        if (mPhase == MPhase.down)
        {
            //Debug.Log("执行mPhase=" + mPhase + "分支");
            AddmouseInfo();             //添加该结构的MouseInfo

            //如果左键释放
            if (b0Up)
            {
                MouseTap();             //单击动作
                mPhase = MPhase.idle;
            }
            else if (Time.time - mouseInfos[0].time > mTapTime)
            {
                //如果按下长度超过单击的长度，则为拖动
                float dragDist = (lastMouseInfo.screenLoc - mouseInfos[0].screenLoc).magnitude;

                if (dragDist >= mDragDist)
                    mPhase = MPhase.drag;

                //^^^易错处:若代码放在if(mPhase == MPhase.down)下则会将鼠标点击状态改变为拖动状态,造成无法单击移动只可拖动的问题
                //如果道具没有被选中,则mTapTime一旦结束拖动就开始
                if (selectedElements.Count == 0)
                    mPhase = MPhase.drag;
            }
        }

        //如果鼠标被拖动
        if (mPhase == MPhase.drag)
        {
            //Debug.Log("执行mPhase=" + mPhase + "分支");
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
        OrbitSelectedElements();

        //实时更新UI
        healthUI.fillAmount = (float)health / (float)healthMax;
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
            if (mInfo.time != lastTime)
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
        //if (DEBUG)
        //    print("Mage.MouseDown()");

        //取得鼠标单击的对象
        GameObject clickeddGO = mouseInfos[0].hitInfo.collider.gameObject;

        //判断是Ground/MageEnemy
        GameObject taggedParent = Utils.FindTaggedParent(clickeddGO);
        if (taggedParent == null)
            actionStartTag = "";
        else actionStartTag = taggedParent.tag;
    }

    //单击某对象(按钮)
    void MouseTap()
    {
        //if (DEBUG)
        //    print("Mage.MouseTap()");

        //否则即为单击行走模式
        switch (actionStartTag)
        {
            case "Mage":
                break;
            case "Ground":
            case "Magic":
                WalkTo(lastMouseInfo.loc);  //前进道第一个mouseInfo位置
                ShowTap(lastMouseInfo.loc);
                break;
        }
    }

    //拖动鼠标穿过
    void MouseDrag()
    {
        //if (DEBUG)
        //    print("Mage.MouseDrag()");

        //只有鼠标从地面开始拖动的情况才有效
        if (actionStartTag != "Ground")
            return;

        //如果道具没有被选中,玩家应该随着鼠标移动
        if (selectedElements.Count == 0)
        {
            //继续前往当前mouseInfo位置
            WalkTo(mouseInfos[mouseInfos.Count - 1].loc);
        }
        else
            //地面法术,绘制线条
            AddPointToLiner(mouseInfos[mouseInfos.Count - 1].loc);

        //继续前往当前mouseInfo位置
        //WalkTo(mouseInfos[mouseInfos.Count - 1].loc);
    }

    //鼠标拖动后释放
    void MouseDragUp()
    {
        //if (DEBUG)
        //    print("Mage.MouseDragUp()");

        //只有鼠标从地面开始拖动的情况才有效
        if (actionStartTag != "Ground")
            return;

        //如果没有道具被选中,则马上停止
        if (selectedElements.Count == 0)
        {
            //当拖拽结束时停止前进
            StopWalking();
        }
        else
            //释放法术
            CastGroundSpell();

        //清除绘制器
        ClearLiner();
    }

    void CastGroundSpell()
    {
        //若未选择道具则无法施放法术
        if (selectedElements.Count == 0)
            return;

        //释放不同法术:
        switch (selectedElements[0].type)
        {
            //火法术
            case ElementType.fire:
                GameObject fireGo;
                foreach (Vector3 pt in linePts)
                {
                    //为LinePts中每个vector3创建一个fireGroundSpellPrefab实例
                    fireGo = Instantiate(fireGroundSpellPrefab) as GameObject;
                    fireGo.transform.parent = spellAnchor;
                    fireGo.transform.position = pt;
                }
                break;

            //水法术
            case ElementType.water:
                GameObject waterGo;
                foreach (Vector3 pt in linePts)
                {
                    //为LinePts中每个vector3创建一个waterGroundSpellPrefab实例
                    waterGo = Instantiate(airGroundSpellPrefab) as GameObject;
                    waterGo.transform.parent = spellAnchor;
                    waterGo.transform.position = pt;
                }
                break;

            //雷法术
            case ElementType.air:
                GameObject airGo;
                foreach (Vector3 pt in linePts)
                {
                    //为LinePts中每个vector3创建一个waterGroundSpellPrefab实例
                    airGo = Instantiate(waterGroundSpellPrefab) as GameObject;
                    airGo.transform.parent = spellAnchor;
                    airGo.transform.position = pt;
                }
                break;

            //恢复法术
            case ElementType.earth:
                GameObject earthGo;
                foreach (Vector3 pt in linePts)
                {
                    //为LinePts中每个vector3创建一个waterGroundSpellPrefab实例
                    earthGo = Instantiate(earthGroundSpellPrefab) as GameObject;
                    earthGo.transform.parent = spellAnchor;
                    earthGo.transform.position = pt;
                }
                break;
        }
        //清除selectedElements
        ClearElements();
    }

    //前进到指定位置，z=0
    public void WalkTo(Vector3 xTarget)
    {
        walkTarget = xTarget;       //设置当前目的地
        walkTarget.z = -0.1f;       //固定Z方向
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
        //Debug.Log("执行StopWalking(),walking=" + walking);
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (invincibleBool)
        {
            float blinkU = (Time.time - damageTime) / invincibleDur;
            blinkU *= invTimesToBlink;
            blinkU %= 1.0f;
            bool visible = (blinkU > 0.5f);

            if (Time.time - damageTime > invincibleDur)
            {
                invincibleBool = false;
                visible = true;
            }

            //设置游戏对象失效使其隐身
            viewCharacterTrans.gameObject.SetActive(visible);
        }

        if (knockbackBool)
        {
            if (Time.time - damageTime > knockbackDur)
            {
                knockbackBool = false;
            }
            float knockbackSpeed = knockbackDist / knockbackDur;
            vel = knockbarDir * knockbackSpeed;

            return;
        }

        //若Mage前进
        if (walking)
        {
            //若Mage非常靠近目的地，则停在当前坐标
            if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime)
            {
                //Debug.Log("已靠近目的地,故停止");
                pos = walkTarget;
                StopWalking();
            }
            else
            {   //否则向目的地移动
                GetComponent<Rigidbody>().velocity = (walkTarget - pos).normalized * speed;
                //Debug.Log("未靠近目的地,故继续运动");
            }
        }
        else
        {   //否则静止
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Debug.Log("非walking状态,故静止");
        }
        /* 向目的地运动过程出现的bug：
        问题描述：点击了有效的目的地后，Mage仅移动了较短距离后就仿佛被碰撞阻碍运动，最终停止
        问题分析：当我把Mage稍微调高不与地面TileAnchor接触时，就完全解决了此问题；
                 是否是TileAnchor物体上的Collider，与Mage碰撞产生减速呢？
                 那摩擦力从哪来？Mage上的AngularDrag吗？ --->否，全部调0后并未改变
                 总之，很可能就是Mage模型与TileAnchor物体的碰撞器细微接触问题。
        解决方案：稍微调节Mage高度使其不与TileAnchor接触；
                 或稍微缩小Mage->CharacterTrans->View_Character->legs的collider的范围
        后续问题1:貌似没有根治该问题,有可能是读取鼠标的目的地与Mage运动不在统一z平面,似的Mage的运动受阻
        问题分析: 是在写添加道具代码时位置出错,出错地位于Update()的if内标记处
         
         
         */
    }

    //EnemyBug的碰撞伤害
    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGo = coll.gameObject;

        Tile ti = otherGo.GetComponent<Tile>();
        if (ti != null)
        {
            //height>0说明为墙壁，无法穿过
            if (ti.height > 0)
            {
                //Debug.Log("墙壁,无法穿过");
                StopWalking();
            }
        }

        //判断是否为EnemyBug
        EnemyBug bug = coll.gameObject.GetComponent<EnemyBug>();

        //如果otherGO为EnemyBug,则将bug传递给CollisionDamage()
        //if (bug != null)
        //    CollisionDamage(otherGo);

        //如果otherGO为EnemyBug,则将bug传递给CollisionDamage()
        //该方法将bug解释为Enemy:
        if (bug != null)
            CollisionDamage(bug);
    }

    //EnemySpiker的触发伤害
    private void OnTriggerEnter(Collider other)
    {
        EnemySpiker spiker = other.GetComponent<EnemySpiker>();
        if (spiker != null)
        {
            //CollisionDamage(other.gameObject);

            //CollisionDamage()将攻击者视为Enemy
            CollisionDamage(spiker);
        }
    }

    //Mage受到伤害计算
    void CollisionDamage(Enemy enemy)
    {
        //如果在闪烁就不进行攻击
        if (invincibleBool)
            return;

        //Mage被敌人击中
        StopWalking();
        ClearInput();

        //减少生命值
        //health -= 1;
        health -= enemy.touchDamage;

        //3心型血条减少
        //Allhealth[(int)health].gameObject.SetActive(false);
        //单心型血条减少
        //healthUI.fillAmount = (float)health / (float)healthMax;

        if (health <= 0)
        {
            Die();
            return;
        }

        damageTime = Time.time;
        knockbackBool = true;
        knockbarDir = (pos - enemy.pos).normalized;
        invincibleBool = true;
    }

    //Mage死亡
    void Die()
    {
        //重载场景
        //Application.LoadLevel(0);
        SceneManager.LoadScene(0);
    }

    //显示玩家单击的地方
    public void ShowTap(Vector3 loc)
    {
        GameObject go = Instantiate(tapIndicatorPrefab) as GameObject;
        go.transform.position = loc;
    }

    //选择elType的一个Element_Sphere并添加到selectedElements
    public void SelectedElement(ElementType elType)
    {
        //如果没有道具,就清空所有道具
        if (elType == ElementType.none)
        {
            ClearElements();
            return;
        }

        //如果只有一个可选,则清空该道具
        if (maxNumSelectedElements == 1)
        {
            ClearElements();
        }

        //不可同时选择数量超过maxNumSelectedElements
        if (selectedElements.Count >= maxNumSelectedElements)
            return;

        //可以添加当前道具
        GameObject go = Instantiate(elementPrefabs[(int)elType]) as GameObject;
        Element el = go.GetComponent<Element>();
        el.transform.parent = this.transform;

        //将el添加到selectedElements列表
        selectedElements.Add(el);
    }

    //将selectedElements的所有道具清空并销毁他们的游戏对象
    public void ClearElements()
    {
        foreach (Element el in selectedElements)
        {
            Destroy(el.gameObject);
        }
        selectedElements.Clear();
    }

    //调用每个Update方法使道具围绕旋转
    void OrbitSelectedElements()
    {
        //如果没有则返回
        if (selectedElements.Count == 0)
            return;

        Element el;
        Vector3 vec;
        float theta0, theta;
        float tau = Mathf.PI * 2;

        //将圆圈划分到各个旋转道具
        float rotPreElement = tau / selectedElements.Count;

        //基于时间来设置旋转基础角度
        theta0 = elementRotSpeed * Time.time * tau;

        for (int i = 0; i < selectedElements.Count; i++)
        {
            //确定每个道具的旋转角度
            theta = theta0 + i * rotPreElement;
            el = selectedElements[i];

            //使用简单三角形将角度转换为单位矢量
            vec = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);

            //用elementRotDist乘以单位矢量
            vec *= elementRotDist;

            //拉伸道具到腰部位置
            vec.z = -0.5f;

            //设置Element_Sphere的位置
            el.lPos = vec;
        }
    }

    //为线条添加新坐标:如果太靠近已存在的坐标则忽略
    void AddPointToLiner(Vector3 pt)
    {
        //使pt与地面存在距离
        pt.z = lineZ;

        //如果LinePts为空则添加坐标
        if (linePts.Count == 0)
        {
            linePts.Add(pt);
            totalLineLength = 0;
            return;
        }

        //如果线条超过最大长度则返回
        if (totalLineLength > lineMaxLength)
            return;

        //如果有闲钱坐标pt0,那么查找pt与其距离
        //获取LinePts中的最新坐标
        Vector3 pt0 = linePts[linePts.Count - 1];
        Vector3 dir = pt - pt0;
        float delta = dir.magnitude;
        dir.Normalize();

        totalLineLength += delta;

        //如果小于最小距离则返回
        if (delta < lineMinDelta)
            return;

        //如果大于最大距离则添加坐标
        if (delta > lineMaxDelta)
        {
            //在二者之间添加坐标
            float numToAdd = Mathf.Ceil(delta / lineMaxDelta);
            float midDelta = delta / numToAdd;
            Vector3 ptMid;
            for (int i = 1; i < numToAdd; i++)
            {
                ptMid = pt0 + (dir * midDelta * i);
                linePts.Add(ptMid);
            }
        }

        //添加坐标,更新线条
        linePts.Add(pt);
        UpdateLiner();
    }

    //使用新的坐标更新LineRenderer
    public void UpdateLiner()
    {
        int el = (int)selectedElements[0].type;

        //基于该类型设置线条颜色
        liner.startColor = elementColors[el];
        liner.endColor = elementColors[el];
        //liner.SetColors(elementColors[el], elementColors[el]);

        //更新简要释放法术的外观
        //设置顶点数
        liner.positionCount = linePts.Count;
        //设置各顶点
        for (int i = 0; i < linePts.Count; i++)
            liner.SetPosition(i, linePts[i]);
        //启用LinerRenderer
        liner.enabled = true;
    }

    //清除绘制器
    public void ClearLiner()
    {
        liner.enabled = false;
        linePts.Clear();
    }

    //停止任何的有效拖动或鼠标输入
    public void ClearInput()
    {
        mPhase = MPhase.idle;
    }
}