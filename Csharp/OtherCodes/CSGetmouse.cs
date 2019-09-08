using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGetmouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
            print("鼠标左键被按下");
        if (Input.GetMouseButtonDown(1))
            print("鼠标右键被按下");
        if (Input.GetMouseButtonUp(2))
        {
            print("中键抬起.");
            print("当前鼠标位置：" + Input.mousePosition);
        }
	}
}
