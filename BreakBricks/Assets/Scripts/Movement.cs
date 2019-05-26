using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");  //x轴
        float v = Input.GetAxis("Vertical");    //y轴
        //Debug.Log(h);
        transform.Translate(new Vector3(h, v, 0) * Time.deltaTime * speed);
        //左右镜头移动速度1 m/s * speed
        

    }
}
