using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----雷魔法:对bug造成小幅伤害,但对Spiker无效-----*/
public class AirGroundSpell : PT_MonoBehaviour
{
    public float duration = 4;              //游戏对象的生命周期
    public float durationVariance = 0.5f;   //持续时间差值,使持续时间在3.5~4.5
    public float fadeTime = 1f;             //衰减时间长度
    public float timeStart;                 //游戏对象开始时间

    public float damagePreSecond = 5;       //每秒伤害量

    private void Start()
    {
        timeStart = Time.time;
        duration = Random.Range(duration - durationVariance, duration + durationVariance);
    }

    private void Update()
    {
        //u储存已消耗的的时间百分比(0~1)
        float u = (Time.time - timeStart) / duration;

        //u的值决定何时开始衰减
        float fadePercent = 1 - (fadeTime / duration);

        //如果大于开始衰减,就下落到地面
        if (u > fadePercent)
        {
            float u2 = (u - fadePercent) / (1 - fadePercent);

            Vector3 loc = pos;
            loc.z = u2 * 2;
            pos = loc;
        }

        //如果大于持续时间,则销毁
        if (u > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameObject go = Utils.FindTaggedParent(other.gameObject);

        //if (go == null)
        //    go = other.gameObject;

        //Utils.tr("Flame hit", go.name);
    }

    private void OnTriggerStay(Collider other)
    {
        //获取EnemyBug脚本其他组件的引用
        EnemyBug recipient = other.GetComponent<EnemyBug>();

        //如果有EnemyBug组件,使用雷法术消灭它
        if (recipient != null)
        {
            recipient.Damage(damagePreSecond, ElementType.air, true);
        }
    }
}
