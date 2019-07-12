using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static public Slingshot S;
    public GameObject prefabProjectile; //弹丸预制体
    public float velocityMult = 10f;    //弹丸初速度
    public bool ______________________;
    public GameObject launchPoint;      //激活状态高光
    public Vector3 launchPos;           //弹丸发射位置
    public GameObject projectile;       //实例化的弹丸
    public bool aimingMode;             //玩家是否在弹弓上单击鼠标左键

    public float time;                  //计时器
    public float timeLimit = 7;         //时限(超过时限强制返回)
    public bool canTime;                //是否开始计时(仅当发射弹丸时才开启计时)
    public bool canGetGoal;             //是否可以碰撞到目标Goal(仅当发射中的弹丸可以触碰)

    private void Awake()
    {
        S = this;

        //初始化激活状态高光
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);

        //初始化弹丸发射位置为高光坐标
        launchPos = launchPointTrans.position;

        //初始时不可计时
        canTime = false;

        //初始时Goal不可碰撞
        canGetGoal = false;
    }

    //当鼠标进入Collider或GUIElement内才调用
    private void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    //当鼠标从Collider或GUIElement内移出才调用
    private void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    //当鼠标在Collider或GUIElement内按下左键才调用
    private void OnMouseDown()
    {
        //玩家在鼠标光标悬停在弹弓上方按下了鼠标左键
        aimingMode = true;

        //实例化一个弹丸，设置坐标及刚体信息
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        //计时
        if(canTime)
            time += Time.deltaTime;

        //若弹弓未处于瞄准模式则返回
        if (!aimingMode)
            return;

        //获取光标在二维窗口中的当前坐标
        Vector3 mousePos2D = Input.mousePosition;

        //将光标位置转换为三维世界坐标
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //计算launchPos 到 mousePos3D 两点之间的坐标差
        Vector3 mouseDelta = mousePos3D - launchPos;

        //将mouseDelta 坐标差限制在弹弓的SphereCollider范围内
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            //在保持MouseDelta方向不变的前提下将其长度变为1
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //将弹丸projectile 移动到新的位置
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            //如果松开鼠标
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;

            /*---弹射出弹丸的方法：施加初速度与施加力，效果相同---*/
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
            //projectile.GetComponent<Rigidbody>().AddForce(-mouseDelta * 400f);

            //设置跟随相机的兴趣点(跟随点)
            FollowCam.s.poi = projectile;

            //留空projectile字段，以便下次发射时储存新的弹丸，并非销毁
            projectile = null;

            //弹丸已经发射，可以开始计时，且发射的弹丸可合法碰撞Goal
            canTime = true;         
            canGetGoal = true;

            //重置下一发弹丸的尾拖
            //此处可实现短时间内发射多个尾拖弹丸
            ProjectileLine.S.poi = null;

            MissionDemolition.ShotFired();
        }
    }
}