using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chest : Enemy
{
    public bool _________________;
    public Sprite[] sprites;

    //宝箱怪在追逐Player时才原形毕露
    protected override void Update()
    {
        base.Update();
        if (chasing)
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
}
