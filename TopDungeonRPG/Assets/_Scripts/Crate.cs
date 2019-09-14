using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //显示造成伤害UI
        //GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

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
