using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalmet = false;

    private void OnTriggerEnter(Collider other)
    {
        //当发射中的Projectile触碰到Goal，才可判定抵达Goal
        if (other.gameObject.tag == "Projectile" && Slingshot.S.canGetGoal)
        {
            Goal.goalmet = true;

            //变更Goal颜色
            Color c = new Color(92, 250, 0, 64);
            GetComponent<Renderer>().material.color = c;
        }
    }
}
