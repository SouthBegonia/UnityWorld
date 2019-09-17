using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可被攻击的对象的类
public class Fighter : MonoBehaviour
{
    //基本参数:生命值,最大生命值,速度恢复系数
    public int hitPoint = 10;               //当前生命值
    public int maxHitPoint = 10;            //最大生命值
    public float pushRecoverySpeed = 0.2f;  //被击退后恢复运动状态

    //免疫时间参数
    protected float ImmuneTime = 0.75f;
    protected float lastImmune;

    //被击攻击参数:击退距离
    protected Vector3 pushDirection;

    //接受伤害函数
    protected virtual void ReceiveDamage(Damag dmg)
    {
        //如果不在免疫时间内,则会被造成伤害
        if (Time.time - lastImmune > ImmuneTime)
        {
            //重置免疫时间
            lastImmune = Time.time;

            //造成伤害
            hitPoint -= dmg.damageAmount;

            //被击退
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
        }

        //显示造成伤害UI
        //舍弃原因:敌人或者Player受伤都显示同类UI,较为混乱;且因为BoxCollider2D过小,告诉碰撞后易出现穿透bug
        //GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

        //如果对象血量低于0,则死亡
        if (hitPoint <= 0)
        {
            hitPoint = 0;
            Death();
        }
    }

    //死亡函数
    protected virtual void Death()
    {

    }
}
