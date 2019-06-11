using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITank : Unit
{
    private GameObject player;      //玩家标识
    public float attackRange;       //敌人攻击范围
    public float moveSpeed = 8f;    //敌人移动速度
    private TankWeapon tw;          //武器组件
    private float timer;
    public float shootCoolDown = 2f;//敌人射击冷却时间
    public NavMeshAgent tankEnemyNMA;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //取得玩家标识
        tw = GetComponent<TankWeapon>();                        //取得武器脚本
        tankEnemyNMA = GetComponent<NavMeshAgent>();
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

    //    //敌人的方向跟随玩家
    //    transform.LookAt(player.transform.position);

    //    //协程计时方法(未实现)
    //    //StartCoroutine(Wait(2f));
    //}

    ////协程延时
    //IEnumerator Wait(float t)
    //{
    //    tw.Shoot();
    //    Debug.Log("协程");
    //    yield return new WaitForSeconds(t);
    //}


    //跟踪方案2：Nav Mash Agent 组件
    //优点：实现绕路跟踪，不会即使转向，降低难度，最主要避免了卡在玩家与敌人坦克之间的墙面上(也就是不会自动绕路)
    //缺点：Navigation的烘培设置，产生的其他问题(下面分析)
    private void Update()
    {
        if (player == null)
            return;

        //射击冷却计时，测量敌人与玩家间距
        timer += Time.fixedDeltaTime;
        float dist = Vector3.Distance(player.transform.position, transform.position);

        //导航移动
        tankEnemyNMA.SetDestination(player.transform.position);
        
        //当距离够近：转向朝向玩家，若射击冷却完毕，敌人进行射击
        if (dist <= tankEnemyNMA.stoppingDistance)
        {
            //产生问题：导航后敌人已经处于stoppingDistance范围内，假若玩家一直处于该范围内，则敌人不再导航移动，原地射击
            //问题分析：导航进入范围内就不再导航(不再移动或旋转)
            //解决方案：进入导航的目的地区域后，就开启方向跟踪，即下行代码
            transform.LookAt(player.transform.position);

            //若可以射击，就进行射击
            if(timer>shootCoolDown)
            {
                tw.Shoot();
                timer = 0f;
            }
        }
        else
        {
            //产生问题：若将导航移动 tankEnemyNMA.SetDestination(player.transform.position) 写于此处
            //         会在导航触碰到边界时被弹开，而后一直在规划导航，然而敌人坦克却不断被弹来弹去
            //问题分析：
            //解决方案：写在外部Update内
        }
       //其他问题：挖油机Navigation baking不了
       //
    }
}
