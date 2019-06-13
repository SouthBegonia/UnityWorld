using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITank : Unit
{
    private float enemySearchRange;      //搜索敌人(玩家或者其他阵营的敌人)的范围
    private GameObject enemy;            //敌人(玩家或者其他阵营的敌人)
    private LayerMask enemyLayer;        //所在图层

    public float attackRange;       //攻击范围
    public float moveSpeed = 8f;    //移动速度
    private TankWeapon tw;          //武器组件
    //private float timer;
    //public float shootCoolDown = 2f;//射击冷却时间
    private NavMeshAgent tankEnemyNMA;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Player");    //取得玩家标识
        tw = GetComponent<TankWeapon>();                        //取得武器脚本
        tankEnemyNMA = GetComponent<NavMeshAgent>();

        tw.Init(team);      //初始化阵营设置
        enemyLayer = LayerManager.GetEnemyLayer(team);
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
            return;

        //射击冷却计时，测量敌人与玩家间距
        //timer += Time.fixedDeltaTime;
        float dist = Vector3.Distance(enemy.transform.position, transform.position);

        //Navigation导航移动(
        tankEnemyNMA.SetDestination(enemy.transform.position);

        //当敌人距离玩家足够近时
        if (dist <= tankEnemyNMA.stoppingDistance)
        {
            //产生问题：导航后敌人已经处于stoppingDistance范围内，假若玩家一直处于该范围内，则敌人不再导航移动，原地射击
            //问题分析：导航进入范围内就不再导航(不再移动或旋转)
            //解决方案：进入导航的目的地区域后，就开启方向跟踪，即下行代码
            transform.LookAt(enemy.transform.position);


            //射线检测碰撞：只有当敌人与玩家间无障碍物时才射击(避免怼墙)
            //从敌人坦克位置发射一条射线
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //检验射出炮弹：
                //如果射线碰撞到玩家坦克才进行射击
                if (hit.collider.gameObject == enemy)
                {
                    //输出碰撞对象，在场景视图中绘制射线
                    //Debug.Log("碰撞对象: " + hit.collider.name);
                    //Debug.DrawLine(ray.origin, hit.point, Color.red);

                    //调用武器脚本内射击方法
                    tw.Shoot();
                }
            }
        }

        //产生问题：若将导航移动 tankEnemyNMA.SetDestination(player.transform.position) 写于if内
        //         会在导航触碰到边界时被弹开，而后一直在规划导航，然而敌人坦克却不断被弹来弹去
        //问题分析：
        //解决方案：写在外部Update内
        //
        //其他问题：挖油机Navigation baking不了
    }

    //搜索范围内的敌人
    public void SearchEnemy()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, enemySearchRange, enemyLayer);
        enemy = cols[Random.Range(0, cols.Length)].gameObject;
    }
}
