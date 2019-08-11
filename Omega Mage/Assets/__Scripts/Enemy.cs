using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    //Enemy的transform.position
    Vector3 pos { get; set; }

    //通过接触Enemy完成攻击
    float touchDamage { get; set; }
}


//public class Enemy : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
