using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//宝箱脚本:玩家触碰获得金币
public class Chest : Collectable
{
    public Sprite emptyChest;       //宝箱的Sprite
    public int pesosAmount = 5;     //宝箱内的金币数

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;

            //显示宝箱获得金钱UI
            GameManager.instance.ShowText("+" + pesosAmount + " pesos", 25, Color.yellow, transform.position, Vector3.up * 20, 1.5f);

            //在GameManager内同步金钱数目
            GameManager.instance.pesos += pesosAmount;
        }
        
    }
}
