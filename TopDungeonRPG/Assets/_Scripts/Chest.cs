using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int pesosAmount = 5;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;

            //显示宝箱获得金钱UI
            GameManager.instance.ShowText("+" + pesosAmount + " pesos", 25, Color.yellow, transform.position, Vector3.up * 20, 1.5f);
        }
        
    }
}
