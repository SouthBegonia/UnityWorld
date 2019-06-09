using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWeapon : MonoBehaviour
{
    public GameObject shell;            //炮弹
    public float shootPower = 35f;      //火力
    public Transform shootPoint;        //炮弹射出点
    private AudioSource shootFiring;    //开火声

    private void Start()
    {
        //取得开火声音源
        shootFiring = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //空格键射击
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void Shoot()
    {
        //Instantiate返回类型为 object，最好每次进行转换
        GameObject ShellGo = Instantiate(shell, shootPoint.position, shootPoint.rotation) as GameObject;
        Rigidbody ShellRigidbody = ShellGo.GetComponent<Rigidbody>();

        //实现炮弹射出方法：
        //1.赋予炮弹初速度
        ShellRigidbody.velocity = shootPoint.forward * shootPower;
        //2.施加力
        //ShellRigidbody.AddForce(shootPoint.forward * shootPower * 50f);
        //问题：如何实现发射炮弹的后坐力？


        //发出开火声
        shootFiring.Play();
    }
}
