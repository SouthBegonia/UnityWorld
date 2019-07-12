using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public FollowCam s;      //FollowCam的单例对象
    public float easing = 0.05f;
    public Vector2 minXY;           //默认值[0,0]
    public bool _________________;
    public GameObject poi;          //兴趣点
    public float camZ;              //相机的Z坐标


    private void Awake()
    {
        s = this;
        camZ = this.transform.position.z;
    }


    //物理模拟 50fps
    private void FixedUpdate()
    {
        Vector3 destination;

        //若兴趣点不存在则返回
        if (poi == null)
            destination = Vector3.zero;
        else
        {
            //获取兴趣点的位置
            destination = poi.transform.position;

            //如果兴趣点是一个Projectile实例，检查它是否停止
            if(poi.tag == "Projectile")
            {
                //如果它处于为移动sleeping状态，则返回默认视图
                if (poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    //重置计时
                    Slingshot.S.time = 0;
                    Slingshot.S.canTime = false;

                    //重置Goal合法性
                    Slingshot.S.canGetGoal = false;

                    poi = null;
                    return;
                }
                else if (Slingshot.S.time > Slingshot.S.timeLimit)
                {   
                    //若timeLimit时限内未完成静止，则强制返回
                    Slingshot.S.time = 0;
                    Slingshot.S.canTime = false;

                    Slingshot.S.canGetGoal = false;

                    poi = null;
                    return;
                }
            }
        }

        /*
        限定x和y的最小值：实现相机不会移动到x，y轴的负方向上，即不会显示地面以下
        */
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.x, destination.y);

        /*
         在相机当前位置和目标位置之间增添插值：
         Vector3.Lerp()返回两点之间的一个线性插值位置，取两点位置的加权平均值;
         当easing=0时，返回 transform.position；
         当easing=0.05时，表示让相机从 当前位置(起始位置)向 兴趣点poi位置移动，每帧移动5%的距离；
         由此当弹丸射出时，相机每帧跟随
         */
        destination = Vector3.Lerp(transform.position, destination, easing);

        //保持destination.z的值为camZ，因为弹丸锁了Z轴，否则画面会在Z方向移动，造成视野前后移动
        destination.z = camZ;

        //设置相机位置到destination
        transform.position = destination;

        //设置相机的orthographicSize，使地面始终处于画面中
        GetComponent<Camera>().orthographicSize = destination.y + 10;
    }
}
