using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System;

public class TodayIsAGoodDay : MonoBehaviour
{
    // 在TodayIsAGoodDayEditor中 根据ShowName的值显示/隐藏Name
    public bool ShowName;
    [HideInInspector]
    public string Name = "Joker";

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Name = " + Name);
        }

    }
}
