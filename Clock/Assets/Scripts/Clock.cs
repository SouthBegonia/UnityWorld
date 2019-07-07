using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    //接收三个指针
    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject secondHand;

    //小时、分钟、秒钟
    private int h;
    private int m;
    private int s;

    void Update()
    {
        //读取本地时间
        GetNowTime();

        //更新针轴旋转：
        //Quaternion.AngleAxis(angle : float, axis : Vector3)：绕 axis轴旋转 angle角度，创建一个旋转
        //其中绕 axis轴方向：左手拇指指向axis方向，四指所环绕的方向(类似左手螺旋定则)
        hourHand.transform.rotation = Quaternion.AngleAxis((30 * h + 0.5f * m + (30.0f / 3600.0f) * s), Vector3.back);
        minuteHand.transform.rotation = Quaternion.AngleAxis((6 * m + 0.1f * s), Vector3.back);
        if (secondHand != null)
            secondHand.transform.rotation = Quaternion.AngleAxis((6 * s), Vector3.back);
    }

    //读取本地时间信息
    private void GetNowTime()
    {
        h = DateTime.Now.Hour;
        m = DateTime.Now.Minute;
        s = DateTime.Now.Second;
    }
}