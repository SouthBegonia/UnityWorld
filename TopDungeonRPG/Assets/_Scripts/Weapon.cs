using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Colliderable
{
    //武器伤害参数
    public int damagePoint = 1;
    public float pushForce = 2.0f;

    //武器等级参数
    public int weaponLevel = 0;
    public SpriteRenderer SpriteRenderer;

    //武器冷却参数
    private float coolDown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > coolDown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;

            Damag dmg = new Damag
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
        //base.OnCollide(coll);
        
    }

    private void Swing()
    {
        Debug.Log("Swing");
    }
}
