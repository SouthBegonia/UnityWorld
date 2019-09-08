using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour {

    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        /*脚本添加到要旋转的物体上，物体就会随鼠标的移动而旋转
        GetAxis("Mouse X") :得到一帧内鼠标在水平方向的移动距离 */
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, h, 0);
	}
}
