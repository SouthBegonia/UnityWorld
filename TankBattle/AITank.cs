using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITank : Unit
{
    private GameObject player;      //玩家标识
    public float attackRange;       //敌人攻击范围
    public float moveSpeed = 8f;    //敌人移动速度
    private TankWeapon tw;
    private float timer;
    public float shootCoolDown = 2f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //取得玩家标识
        tw = GetComponent<TankWeapon>();                        //取得武器脚本
    }

    private void FixedUpdate()
    {
        //计时
        timer += Time.fixedDeltaTime;

        if (player == null)
            return;

        //若敌人距离玩家太远，则跟踪靠近
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > attackRange)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        else
        {
            //只有玩家在敌人射程范围，才会射击
            if (timer > shootCoolDown)  
            {
                tw.Shoot();
                timer = 0f;
            }
        }

        //敌人的方向跟随玩家
        transform.LookAt(player.transform.position);

        //StartCoroutine(Wait(2f));
    }

    //IEnumerator Wait(float t)
    //{
    //    tw.Shoot();
    //    Debug.Log("协程");
    //    yield return new WaitForSeconds(t);
    //}
}
