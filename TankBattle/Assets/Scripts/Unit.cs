using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Unit脚本：实现所有坦克的基本属性（阵营，血量，死亡爆炸特效，伤害计算）
*/
public enum Team
{
    Blue, Green, Red
}

//生命值系统 被其他类继承
public class Unit : MonoBehaviour
{
    public int health = 100;
    public int curHealth;                  //当前生命值
    public GameObject deadEffect;           //死亡特效(爆炸火花，坦克残骸)
    public float explotionForce = 7000f;    //死后爆炸威力(不影响其他物体的生命值)
    public float explotionRadius = 2f;      //死后爆炸范围(不影响其他物体的生命值)

    public Team team;                       //阵营

    //产生问题：本想在此处设置坦克被摧毁后的爆炸声，但在下面Destruct()内play时报错(未设置物体)
    //解决方案：将TankExplotion 声音挂载于死亡特效deadEffect 下即可实现功能
    //public AudioSource explotionClip;      //死后爆炸声


    public void Start()
    {
        //初始化开始生命值：满血100
        curHealth = health;
    }

    //返回当前生命值
    public int GetCurHealth()
    {
        return curHealth;
    }

    //造成伤害
    public void ApplyDamage(int damage)
    {
        //若生命值非零则减血，否则死亡
        if (curHealth > damage)
            curHealth -= damage;
        else
            Destruct();
    }

    //死亡
    public void Destruct()
    {
        //实例化死亡特效：坦克残骸与爆炸火花：
        //
        //产生问题1：实例化了爆炸特效，但是敌人坦克本身不被销毁，生命值也非规则变化
        //分析解决：若此处不新建deGo接收实例化的爆炸特效(TankBody)，则后续Destory报错，显示不可消除asset资源，即不可销毁预制体
        //      因此最好每次实例化后都另建接收物体(上面的tankExplotion同理)
        //产生问题2：下列实例化 deadEffect 的if语句内，不可创建物体接收，需要在if外接收，显示嵌入语句不可声明或者标记
        //解决方法：if 外创建deGo对象，在实例化接收

        GameObject deGo = null;
        if (deadEffect != null)
            deGo = Instantiate(deadEffect, transform.position, transform.rotation) as GameObject;


        //添加爆炸范围：将坦克残骸的部件炸开，但区别于shell.cs内，此处不影响生命值
        Collider[] cols = Physics.OverlapSphere(transform.position, explotionRadius);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                Rigidbody r = cols[i].GetComponent<Rigidbody>();
                if (r != null)
                    r.AddExplosionForce(explotionForce, transform.position, explotionRadius);
            }
        }

        //销毁被炸坦克
        Destroy(gameObject);

        //此处不销毁爆炸特效，因为爆炸特效是坦克残骸   
        //Destroy(deGo);    
    }
}