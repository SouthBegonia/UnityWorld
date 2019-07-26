using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TankWeapon脚本：实现坦克发射炮弹的过程(实例化炮弹，赋予初速度，开火声，炮弹冷却)
*/
public class TankWeapon : MonoBehaviour
{
    public GameObject shell;            //炮弹
    public float shootPower = 35f;      //火力
    public Transform shootPoint;        //炮弹射出点
    private AudioSource shootFiring;    //开火声
    private bool isWeaponready;         //是否可以射击
    public float shootCoolDown = 1.5f;    //射击冷却时间

    private LayerMask enemyLayer;       //敌人层级

    private float timer = 0;
    private float shootCool = 2f;

    private void Start()
    {
        //取得开火声音源
        shootFiring = GetComponent<AudioSource>();

        //初始化射击冷却
        isWeaponready = true;
    }

    public void Shoot()
    {
        //若射击未冷却则返回
        if (!isWeaponready)
            return;

        //Instantiate返回类型为 object，最好每次进行转换
        GameObject ShellGo = Instantiate(shell, shootPoint.position, shootPoint.rotation) as GameObject;
        Rigidbody ShellRigidbody = ShellGo.GetComponent<Rigidbody>();

        //
        ShellGo.GetComponent<shell>().Init(enemyLayer);

        //实现炮弹射出方法：
        //1.赋予炮弹初速度
        ShellRigidbody.velocity = shootPoint.forward * shootPower;
        //2.施加力
        //ShellRigidbody.AddForce(shootPoint.forward * shootPower * 50f);
        //问题：如何实现发射炮弹的后坐力？

        //发出开火声
        shootFiring.Play();

        //重置射击冷却
        isWeaponready = false;
        StartCoroutine(WeaponCoolDown());
    }

    //协程延时
    IEnumerator WeaponCoolDown()
    {
        yield return new WaitForSeconds(shootCoolDown);
        isWeaponready = true;       //恢复射击冷却
        //Debug.Log("协程");
    }

    //初始化阵营配置
    public void Init(Team team)
    {
        enemyLayer = LayerManager.GetEnemyLayer(team);
    }

    //private void FixedUpdate()
    //{
    //    timer += Time.deltaTime;

    //    if (timer > shootCool)
    //    {
    //        isWeaponready = true;
    //        timer = 0;
    //    }    
    //}
}