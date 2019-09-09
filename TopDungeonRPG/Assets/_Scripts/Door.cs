using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Colliderable
{
    public Sprite doorOpenSprite;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpenSprite;
    }

}
