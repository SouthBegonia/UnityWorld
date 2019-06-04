using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExample : MonoBehaviour
{
    public float speed = 10f;
    public float fireRate = 0.3f;

    private void Update()
    {
        Move();
    }

    //虚函数 Move()，可以被子类重写
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y += speed * Time.deltaTime;
        pos = tempPos;
    }


    //属性 pos
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "Hero":
                break;
            case "HeroLaser":
                Destroy(this.gameObject);
                break;
        }
    }
}
