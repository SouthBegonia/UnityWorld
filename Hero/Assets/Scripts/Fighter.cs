using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Damage
{
    public float HitPoint;
    public Vector3 Pos;     //发出攻击物体的坐标
    public float pushForce;
}


public class Fighter : MonoBehaviour
{
    public float Health;        //生命值
    public  float HealthMax = 100f;   //最大生命值
    public float InvincibleTime = 10f;       //免疫时间
    //protected BoxCollider2D hitCollider;  //自身触发器
    //private Rigidbody2D rigidbody;

    protected float lastHurtTime;

    protected virtual void InitHealth()
    {
        lastHurtTime = -InvincibleTime;
        Health = HealthMax;
    }

    protected virtual void GetHurt(Damage damage)
    {
        if (Time.time - lastHurtTime > InvincibleTime && Health>0)
        {
            lastHurtTime = Time.time;
            Health -= damage.HitPoint;
            //Vector2 delta = new Vector3(-damage.Pos.x * damage.pushForce, transform.parent.position.y);
            //rigidbody.velocity += delta;
        }
        if (Health <= 0)
            Death();        
    }

    protected virtual void Death()
    {
        Debug.Log("Die");
    }
}
