using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泉水恢复生命值脚本
public class HealingFountain : Colliderable
{
    public int healingAmount = 1;       //每次治疗量
    public int healingTotal = 10;       //总共可以恢复多少血量
    private float healCoolDown = 0.5f;  //每次治疗的间隔
    private float lastHeal;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player" && !GameManager.instance.player.isAlive)
            return;

        if (Time.time - lastHeal > healCoolDown && healingTotal > 0)
        {
            lastHeal = Time.time;
            healingTotal--;

            GameManager.instance.player.Heal(healingAmount);
            //Debug.Log("healingAmount");
        }
    }
}
