using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----bug敌人:跟踪玩家,并造成伤害,可被消灭-----*/
public class EnemyBug : PT_MonoBehaviour,Enemy
{
    //EnemyBug扩展PT_MonoBehaviour类并实现了Enemy接口
    //EnemyBug类的实例作为Enemy接口的实例
    [SerializeField]
    private float _touchDamage = 1;     //EnemyBug每次造成伤害1
    public float touchDamage
    {
        get { return (_touchDamage); }
        set { _touchDamage = value; }
    }
    public string typeString
    {
        get { return (roomXMLString); }
        set { roomXMLString = value; }
    }

    public string roomXMLString;

    public float speed = 0.5f;          //EnemyBug的移动速度
    public float health = 10;           //EnemyBug的生命值
    public float damageScale = 0.8f;
    public float damageScaleDuration = 0.25f;
    public bool ____________________;

    public float _maxHealth;            //EnemyBug的最大生命值
    private float damageScaleStartTime;
    public Vector3 walkTarget;
    public bool walking;
    public Transform characterTrans;

    public Dictionary<ElementType, float> damageDict;//保存每个结构每个道具的攻击

    private void Awake()
    {
        characterTrans = transform.Find("CharacterTrans");
        _maxHealth = health;

        ResetDamageDict();
    }

    //重置damageDict的值
    void ResetDamageDict()
    {
        if (damageDict == null)
        {
            damageDict = new Dictionary<ElementType, float>();
        }
        damageDict.Clear();
        damageDict.Add(ElementType.earth, 0);
        damageDict.Add(ElementType.water, 0);
        damageDict.Add(ElementType.air, 0);
        damageDict.Add(ElementType.fire, 0);
        damageDict.Add(ElementType.aether, 0);
        damageDict.Add(ElementType.none, 0);
    }

    private void Update()
    {
        WalkTo(Mage.S.pos);
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
        characterTrans.rotation = Quaternion.Euler(0, 0, rZ);       //设置characterTrans的旋转量(EnemyBug并未旋转)
    }

    //停止EnemyBug前进
    public void StopWalking()
    {
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //若EnemyBug前进
        if (walking)
        {
            //若EnemyBug非常靠近目的地，则停在当前坐标
            if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime)
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
    }

    //对EnemyBug造成伤害函数
    public void Damage(float amt,ElementType eT,bool damageOverTime= false)
    {
        //如果为DOT,则只销毁该结构的分数量
        if (damageOverTime)
        {
            amt *= Time.deltaTime;
        }

        //分布式处理不同类型的攻击
        switch (eT)
        {
            //火魔法对EnemyBug造成伤害
            case ElementType.fire:
                damageDict[eT] = Mathf.Max(amt, damageDict[eT]);
                break;

            //水魔法无效,会恢复EnemyBug的生命
            //case ElementType.water:
            //damageDict[eT] = Mathf.Max(amt, damageDict[eT]);
            //    break;

            case ElementType.air:
                damageDict[eT] = Mathf.Max(amt, damageDict[eT]);
                break;

            default:
                damageDict[eT] += amt;
                break;
        }
    }

    //对EnemyBug造成伤害函数
    public void Recover()
    {
        //恢复EnemyBug全部生命
        health = _maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        //引用不同类型的攻击

        //使用KeyValuePair迭代字典
        float dmg = 0;
        foreach(KeyValuePair<ElementType,float> entry in damageDict)
        {
            dmg += entry.Value;
        }

        //如果EnemyBug受到攻击
        if (dmg > 0)
        {
            if (characterTrans.localScale == Vector3.one)
            {
                damageScaleStartTime = Time.time;
            }
        }

        //攻击范围动画,交替缩放
        float damU = (Time.time - damageScaleStartTime) / damageScaleDuration;
        damU = Mathf.Min(1, damU);
        float scl = (1 - damU) * damageScale + damU * 1;
        characterTrans.localScale = scl * Vector3.one;

        //减少生命值
        health -= dmg;
        health = Mathf.Min(_maxHealth, health);

        ResetDamageDict();

        //生命值为0则死亡
        if (health <= 0)
        {
            Die();
        }
    }
}
