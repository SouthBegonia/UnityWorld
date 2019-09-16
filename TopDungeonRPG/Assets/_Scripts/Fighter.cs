using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可被攻击的对象的类
public class Fighter : MonoBehaviour
{
    //基本参数:生命值,最大生命值,速度恢复系数
    public int hitPoint = 10;
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f;

    //免疫时间参数
    protected float ImmuneTime = 0.75f;
    protected float lastImmune;

    //被击退坐标
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
