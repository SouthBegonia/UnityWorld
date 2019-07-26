using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Shell脚本：实现发射的炮弹的爆炸（特效，冲击力，销毁炮弹）
*/
public class shell : MonoBehaviour
{
    public GameObject shellExplotion;   //爆炸特效
    private float explotionTime;        //爆炸特效的持续时间
    public float explotionRadius = 2f;  //爆炸半径
    public float explotionForce = 100f; //爆炸威力

    public int shellDamage = 10;      //炮弹造成的伤害

    private LayerMask lm;

    private void Start()
    {
        //取得爆炸特效持续时间
        ParticleSystem go = shellExplotion.GetComponent<ParticleSystem>();
        explotionTime = go.main.duration;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //当炮弹碰撞到物体则发生爆炸：
        //
        //存在问题1：向前移动时射击，炮弹会与玩家坦克碰撞，造成爆炸效果，但我们需要的是与其他物体碰撞才触发爆炸；
        //解决方案：
        //1.添加碰撞验证是否为除玩家外的其他物体(本例方案)
        //2.调节炮弹和坦克的碰撞体尺寸，满足运动过程不追赶的条件
        //
        //存在问题2：是否可以将实例化的炮弹赋在各自物体下成为子物体(玩家或者敌人)，此处判断当炮弹碰撞的非自身物体时才爆炸生效
        //解决方案：
        //


        //炮弹撞击，发生爆炸特效，随后子弹和特效销毁
        GameObject seGo = Instantiate(shellExplotion, transform.position, transform.rotation) as GameObject;
        Destroy(gameObject);
        Destroy(seGo, explotionTime);


        //炮弹接触物体，产生局部冲击力
        //第三个参数 lm :使得炮弹仅对敌人阵营(敌人所在图层)造成伤害
        Collider[] cols = Physics.OverlapSphere(transform.position, explotionRadius, lm);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                //向爆炸点的范围空间施加爆炸力
                Rigidbody r = cols[i].GetComponent<Rigidbody>();
                if (r != null)
                    r.AddExplosionForce(explotionForce, transform.position, explotionRadius);

                //造成伤害
                Unit u = cols[i].GetComponent<Unit>();
                if (u != null)
                    u.ApplyDamage(shellDamage);
            }
        }

        //由爆炸范围产生问题：
        //自己或者AI敌人紧贴墙壁射击由于爆炸范围的影响而伤血
    }

    public void Init(LayerMask enemyLayer)
    {
        lm = enemyLayer;
    }
}