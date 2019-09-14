using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泉水恢复生命值脚本
public class HealingFountain : Colliderable
{
    public int healingAmount = 1;
    private float healCoolDown = 1.0f;
    private float lastHeal;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "player")
            return;

        if (Time.time - lastHeal > healCoolDown)
        {
            lastHeal = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }
    }
}
