using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : MonoBehaviour
{

    public GameObject bullet;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello");
        //子弹的实例化
        //GameObject.Instantiate(bullet, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        //左键按下产生子弹
        if(Input.GetMouseButtonDown(0))
        {
            GameObject b = GameObject.Instantiate(bullet, transform.position, transform.rotation);
            Rigidbody rgd = b.GetComponent<Rigidbody>();
            rgd.velocity = transform.forward * speed;
        }
    }
}
