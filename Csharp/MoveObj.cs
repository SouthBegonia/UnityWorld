using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour {

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //使用上下方向键或者W，S键来控制前进和后退
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        //使用左右键或者A，D键来控制左右旋转
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation); //绕Z轴移动
        transform.Rotate(0, rotation, 0);   //绕Y轴旋转
	}
}
