using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalmet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Goal.goalmet = true;
            //Color c = GetComponent<Renderer>().material.color;
            Color c = new Color(92, 250, 0, 64);
            GetComponent<Renderer>().material.color = c;
            Debug.Log("AAA");
        }
    }
}
