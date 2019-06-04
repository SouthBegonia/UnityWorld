using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    //BoidSpawner 的单例模式，只允许存在BoidSpawner的一个实例，所以存放在静态变量S中
    static public BoidSpawner S;

    //配置参数，调整Boid对象的行为
    public int numBoids = 100;                  //boid 的个数
    public GameObject boidPrefab;               //boid 在unity中的预制体
    public float spawnRadius = 100f;            //实例化 boid 的位置范围
    public float spawnVelcoty = 10f;            //boid 的速度
    public float minVelocity = 0f;
    public float maxVelocity = 30f;
    public float nearDist = 30f;                //判定为附近的 boid 的最小范围值
    public float collisionDist = 5f;            //判定为最近的 boid 的最小范围值(具有碰撞风险)
    public float velocityMatchingAmt = 0.01f;   //与 附近的boid 的平均速度 乘数(影响新速度)
    public float flockCenteringAmt = 0.15f;     //与 附近的boid 的平均三维间距 乘数(影响新速度)
    public float collisionAvoidanceAmt = -0.5f; //与 最近的boid 的平均三维间距 乘数(影响新速度)
    public float mouseAtrractionAmt = 0.01f;    //当 鼠标光标距离 过大时，与其间距的 乘数(影响新速度)
    public float mouseAvoidanceAmt = 0.75f;     //当 鼠标光标距离 过小时，与其间距的 乘数(影响新速度)
    public float mouseAvoiddanceDsit = 15f;
    public float velocityLerpAmt = 0.25f;       //线性插值法计算新速度的 乘数

    public bool ______________;

    public Vector3 mousePos;        //鼠标光标位置

    private void Start()
    {
        //设置单例变量S为BoidSpawner的当前实例
        S = this;

        //初始化NumBoids(当前为100)个Boids
        for (int i = 0; i < numBoids; i++)
            Instantiate(boidPrefab);
    }

    private void LateUpdate()
    {
        //读取鼠标光标位置
        Vector3 mousePos2d = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.y);

        //从世界空间到屏幕空间变换位置
        mousePos = this.GetComponent<Camera>().ScreenToWorldPoint(mousePos2d);
    }
}