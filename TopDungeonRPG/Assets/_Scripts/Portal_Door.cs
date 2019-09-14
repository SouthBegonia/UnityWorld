using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//传送门脚本:进入挂载脚本的物体,传送到boxOUT物体附近
public class Portal_Door : Colliderable
{
    public BoxCollider2D boxOUT;            //传送到的地点

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnCollide(Collider2D coll)
    {      
        if (coll.name == "Player")
        {
            //朝向boxOUT的y轴正方向移动一定距离防止再次出发传送
            Vector3 vector = boxOUT.transform.localPosition;
            if (boxOUT.gameObject.transform.rotation.z == 0)
                vector.y += 0.2f;
            else
                vector.y -= 0.2f;

            GameManager.instance.player.transform.position = vector;
        }
    }
}
