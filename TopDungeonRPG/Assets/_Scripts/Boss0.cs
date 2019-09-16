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

        
    }
}
