using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    //Enemy的transform.position
    Vector3 pos { get; set; }

    //通过接触Enemy完成攻击
    float touchDamage { get; set; }

    //从Room.xml获得类型字符串
    string typeString { get; set; }

    GameObject gameObject { get; }
    Transform transform { get; }
}
