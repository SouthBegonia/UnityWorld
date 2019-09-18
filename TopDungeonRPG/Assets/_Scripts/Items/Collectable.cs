using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可收集类的脚本:
public class Collectable : Colliderable
{
    protected bool collected;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            OnCollect();
    }

    protected virtual void OnCollect()
    {
        collected = true;
    }
}
