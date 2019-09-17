using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0 : Enemy
{
    public float[] fireballSpeed = { 2.5f, -2.5f };
    public float distance = 0.25f;
    public Transform[] fireballs;

    protected override void Start()
    {
        base.Start();

        //修改该boss的受伤免疫时间
        ImmuneTime = 0.2f;
    }

    private void Update()
    {
        for(int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance, Mathf.Sin(Time.time * fireballSpeed[i]) * distance, 0);
        }

        //当boss半血以下时,绕身的火球加速,boss移速增加
        if (((float)hitPoint / (float)maxHitPoint) <= 0.5f)
        {
            fireballSpeed[0] = 4f;
            fireballSpeed[1] = -4f;
            speedMultiple = 1f;
        }
    }
}
