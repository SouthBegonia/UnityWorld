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
        //此处易出现bug:
        //贴墙的泉水与tilemap中的Collision图层的砖块进行碰撞,进而检测失误
        //解决方案:再添加Collision碰撞判定
        if (coll.name != "Player" && !GameManager.instance.player.isAlive || coll.name=="Collision")
            return;

        if (Time.time - lastHeal > healCoolDown && healingTotal > 0)
        {
            Debug.Log(coll.name);
            lastHeal = Time.time;
            healingTotal--;

            GameManager.instance.player.Heal(healingAmount);
        }
    }
}
