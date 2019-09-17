using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自动触发开门的脚本:
public class Door : Colliderable
{
    public Sprite doorOpenSprite;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpenSprite;
    }

}
