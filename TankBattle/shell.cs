using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shell : MonoBehaviour
{
    public GameObject shellExplotion;   //爆炸特效
    private float explotionTime;        //爆炸特效的持续时间
    public float explotionRadius = 3f;  //爆炸半径
    public float explotionForce = 500f; //爆炸威力

    private void Start()
    {
        //取得爆炸特效持续时间
        ParticleSystem go = shellExplotion.GetComponent<ParticleSystem>();
        explotionTime = go.main.duration;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //当炮弹碰撞到物体则发生爆炸
        //存在问题：向前移动时射击，炮弹会与玩家坦克碰撞，造成爆炸效果，但我们需要的是与其他物体碰撞才触发爆炸；
        //解决方案：
        //1.添加碰撞验证是否为除玩家外的其他物体(本例方案)
        //2.调节炮弹和坦克的碰撞体尺寸，满足运动过程不追赶的条件
        if (collision.gameObject.tag != "Player")
        {
            GameObject seGo = Instantiate(shellExplotion, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
            Destroy(seGo,explotionTime);
        }


        //爆炸特效
        Collider[] cols = Physics.OverlapSphere(transform.position, explotionRadius);
        if (cols.Length > 0)
        {
            for(int i=0;i<cols.Length;i++)
            {
                Rigidbody r = cols[i].GetComponent<Rigidbody>();
                if (r != null)
                    r.AddExplosionForce(explotionForce, transform.position, explotionRadius);
            }
        }
    }
}
