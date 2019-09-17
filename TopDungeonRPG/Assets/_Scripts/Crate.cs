using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可销毁类的脚本:可挂在于木箱等障碍物
public class Crate : Fighter
{
    private void Start()
    {
        ImmuneTime = 0.5f;
    }

    protected override void ReceiveDamage(Damag dmg)
    {
        if (Time.time - lastImmune > ImmuneTime)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
        }

        //如果对象血量低于0,则死亡
        if (hitPoint <= 0)
        {
            hitPoint = 0;
            Death();
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }
}
