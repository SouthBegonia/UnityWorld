using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*
AItank脚本：实现npc坦克 设置阵营、搜索敌人、自动导航、攻击、自身血条UI
*/
public class AITank : Unit
{
    public float enemySearchRange = 99999;      //搜索敌人(玩家或者其他阵营的敌人)的范围
    private GameObject enemy;            //敌人(玩家或者其他阵营的敌人)
    private LayerMask enemyLayer;        //所在图层

    public float attackRange = 15;  //攻击范围
    public float moveSpeed = 8f;    //移动速度
    private TankWeapon tw;          //武器组件

    private NavMeshAgent tankEnemyNMA;  //AI坦克导航

    private Collider[] cols;        //所有 敌人Collider 的数组
    public float[] cols_dist;       //与所有 敌人的间距 的数组
    private int EnemyNearest;       //最近敌人标记

    public Image healthSlider;      //坦克血条UI

    public void Start()
    {
        base.Start();   //Unit内的start()

        //取得武器脚本及导航
        tw = GetComponent<TankWeapon>();            
        tankEnemyNMA = GetComponent<NavMeshAgent>();
        //if (healthSlider = GetComponentInChildren<Image>())
        //    Debug.Log("suce");

        //初始化阵营设置
        enemyLayer = LayerManager.GetEnemyLayer(team);
        tw.Init(team);  
        
        //初始化  
        cols_dist = new float[20];
    }


    //跟踪方案1：Translate和LookAt方法
    //优点：即时快速跟踪，即时转向
    //缺点：不会自动绕路
    //private void FixedUpdate()
    //{
    //    //计时
    //    timer += Time.fixedDeltaTime;

    //    if (player == null)
    //        return;

    //    //若敌人距离玩家太远，则跟踪靠近
    //    float dist = Vector3.Distance(player.transform.position, transform.position);
    //    if (dist > attackRange)
    //        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    //    else
    //    {
    //        //只有玩家在敌人射程范围，才会射击
    //        if (timer > shootCoolDown)  
    //        {
    //            tw.Shoot();
    //            timer = 0f;
    //        }
    //    }

    //跟踪方案2：Nav Mash Agent 组件
    //优点：实现绕路跟踪，不会即使转向，降低难度，最主要避免了卡在玩家与敌人坦克之间的墙面上(也就是不会自动绕路)
    //缺点：Navigation的烘培设置，产生的其他问题(下面分析)

    private void Update()
    {
        if (enemy == null)
        {
            SearchEnemy();
            return;
        }

        /*----AITank的血条UI----*/
        if (this)
        {
            healthSlider.fillAmount = (float)this.GetCurHealth() / (float)this.health;

            if (this.GetCurHealth() <= 70 && this.GetCurHealth() > 40)
                healthSlider.color = Color.yellow;
            else if (this.GetCurHealth() <= 40)
                healthSlider.color = Color.red;
        }

        /*----搜索敌人并射击功能----*/
        //射击冷却计时，测量敌人与自身间距
        //timer += Time.fixedDeltaTime;
        float dist = Vector3.Distance(enemy.transform.position, transform.position);

        //Navigation导航移动(由于attackRange 和Stopping Distance都设为同值，故Update内导航与下面else内导航同效
        tankEnemyNMA.SetDestination(enemy.transform.position);

        //当敌人距离玩家足够近时
        if (dist <= attackRange)
        {
            //产生问题：导航后敌人已经处于stoppingDistance范围内，假若玩家一直处于该范围内，则敌人不再导航移动，原地射击
            //问题分析：导航进入范围内就不再导航(不再移动或旋转)
            //解决方案：进入导航的目的地区域后，就开启方向跟踪，即下行代码
            transform.LookAt(enemy.transform.position);



            /*--------下面产生天大的BUG--------*/
            //原思路：(在未分类设置敌人阵营的层次时)射线检测是否为敌人，是则射击shoot()
            //产生问题：(在设置不同敌人阵营的层次后)射线检测到的一直是tank上的部件，而那些部件物体不是enemy，if不满足无法进行射击
            //问题分析：内层if判定不正确
            //解决方案：由于设置的阵营层次，就不存在怼墙伤害和队友伤害。因此无需再射线检测，直接shoot()

            ////射线检测碰撞：只有当敌人与玩家间无障碍物时才射击(避免怼墙)
            //Ray ray = new Ray(transform.position, transform.forward);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //{
            //    Debug.Log(gameObject.name + "hit"+hit.collider);
            //    //检验射出炮弹：
            //    //如果射线碰撞到玩家坦克才进行射击
            //    if (hit.collider.gameObject.layer == enemy.layer)
            //    {
            //        Debug.Log(gameObject.name + "shoot Enemy");
            //        //输出碰撞对象，在场景视图中绘制射线
            //        //Debug.Log("碰撞对象: " + hit.collider.name + "发射方：" + gameObject.name);
            //        //Debug.DrawLine(ray.origin, hit.point, Color.red);

            //        //调用武器脚本内射击方法
            //        tw.Shoot();
            //    }
            //}

            //开火攻击
            tw.Shoot();
        }
        else
        {
            //当锁定的敌人超出范围后，重新锁定最近的敌人
            SearchEnemy();

            //tankEnemyNMA.SetDestination(enemy.transform.position);
        }

        //其他问题：挖油机Navigation baking不了
    }

    ////搜索范围内的敌人：选取最近一个敌人,直至击败才切换目标
    //public void SearchEnemy()
    //{
    //    cols = Physics.OverlapSphere(transform.position, enemySearchRange, enemyLayer);
    //    if (cols.Length > 0)
    //    {
    //        float dist = 1000f;
    //        for (int i = 0; i < cols.Length; i++)
    //        {
    //            float temp = Vector3.Distance(cols[i].gameObject.transform.position, transform.position);
    //            if (temp < dist)
    //                enemy = cols[i].gameObject;
    //        }

    //        //随机选取目标
    //        //enemy = cols[Random.Range(0, cols.Length)].gameObject; 
    //    }
    //    //Debug.Log(gameObject.name);
    //    for (int i = 0; i < cols.Length; i++)
    //        Debug.Log(gameObject.name + " 's enemy is" + cols[i].name);
    //}


    //搜索范围内的敌人-改：优先攻击距离自己最近的敌人(即在attackRange范围内的敌人)
    public void SearchEnemy()
    {
        cols = Physics.OverlapSphere(transform.position, enemySearchRange, enemyLayer);

        if (cols.Length > 0)
        {
            //将所有敌人的距离存入数组
            for (int i = 0; i < cols.Length; i++)
                cols_dist[i] = Vector3.Distance(cols[i].gameObject.transform.position, transform.position);

            //Debug相关：
            //Debug.Log("cols.length = " + cols.Length + "\n cols_dist.legth = " + cols_dist.Length);
            //for (int i = 0; i < cols.Length; i++)
            //    Debug.Log(gameObject.name + " Distance from " + cols[i].name + " " + cols_dist[i]);
            //Debug.Log(cols[0].gameObject.name + cols[1].gameObject.name);
            //Debug.Log(gameObject.name + "'s enemy array length is " + cols.Length);
            //for (int i = 0; i < cols.Length; i++)
            //    Debug.Log(i + " " + cols[i].gameObject.name + " " + cols_dist[i]);

            //更新最近的敌人
            EnemyNearest = ChooseNearestEnemy(cols_dist, cols.Length);
            enemy = cols[EnemyNearest].gameObject;
            //Debug.Log(gameObject.name + " 锁定的敌人是 " + enemy.name);
        }

        //Debug.Log(gameObject.name);
        //for (int i = 0; i < cols.Length; i++)
        //    Debug.Log(gameObject.name + " 's enemy is " + cols[i].name);
    }

    //选择最近敌人，返回敌人在在数组内的标记
    public int ChooseNearestEnemy(float[] temp, int length)
    {
        int k = 0;

        ////检测敌人距离及编号
        //for (int m = 0; m < length; m++)
        //    Debug.Log(gameObject.name + " 的第 " + m + " 个敌人是距离它 " + temp[m] + " 远的 " + cols[m].gameObject.name);

        for (int i = 0; i < length; i++)
            for (int j = i + 1; j < length; j++)
                if (temp[k] > temp[j])
                    k = j;

        return k;
    }
}