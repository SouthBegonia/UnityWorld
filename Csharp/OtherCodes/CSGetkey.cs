using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGetkey : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        /* 键盘操作的一种方式
        *if (Input.GetKeyDown("up"))
        *    print("键盘上方向键被按下");
        *if (Input.GetKeyUp("up"))
        *    print("键盘上方向键抬起");
        */

        /* 另一种方式*/
        if(Input.GetKey(KeyCode.UpArrow))
            print("键盘上方向键被按下");
        if(Input.GetKeyUp(KeyCode.UpArrow))
            print("键盘上方向键抬起");

        if (Input.GetKey(KeyCode.DownArrow))
            print("键盘下方向键被按下");
        if(Input.GetKeyUp(KeyCode.DownArrow))
            print("键盘下方向键抬起");

        if (Input.GetKey(KeyCode.LeftArrow))
            print("键盘左方向键被按下");
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            print("键盘左方向键抬起");

        if (Input.GetKey(KeyCode.RightArrow))
            print("键盘右方向键被按下");
        if (Input.GetKeyUp(KeyCode.RightArrow))
            print("键盘右方向键抬起");

        if (Input.GetKeyDown(KeyCode.Space))
            print("键盘空格键被按下");
        if(Input.GetKeyUp(KeyCode.Space))
            print("键盘空格键抬起");
    }
}
