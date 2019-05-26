using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTrans2 : MonoBehaviour {
    public float SpeedOfRotate = 30.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        /*Time.deltaTime 表示距上一次调用所用的时间*/
        transform.Rotate(Vector3.up * Time.deltaTime * SpeedOfRotate);

        /*围绕世界坐标的Y轴旋转
        transform.RotateAround(Vector3.zero, Vector3.up, SpeedOfRotate * Time.deltaTime);
        */
	}
}
