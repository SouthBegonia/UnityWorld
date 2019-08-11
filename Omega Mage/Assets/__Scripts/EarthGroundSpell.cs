using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----恢复魔法:恢复Mage生命-----*/
public class EarthGroundSpell : PT_MonoBehaviour
{
    public float duration = 4;              //游戏对象的生命周期
    public float durationVariance = 0.5f;   //持续时间差值,使持续时间在3.5~4.5
    public float fadeTime = 1f;             //衰减时间长度
    public float timeStart;                 //游戏对象开始时间

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

    private void OnTriggerStay(Collider other)
    {
        //获取Mage脚本其他组件的引用
        Mage recipient = other.GetComponent<Mage>();

        //如果有Mage组件,且其生命值未满,则可恢复其生命值
        if (recipient != null)
        {
            if (recipient.health < recipient.healthMax)
                recipient.health += (Time.fixedDeltaTime / 10);
        }
    }

    
}
