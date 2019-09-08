using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpRef : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int score = 110;

        ClampScore(ref score);

        Debug.Log(score);
	}
	
    void ClampScore(ref int num)
    {
        num = Mathf.Clamp(num, 0, 100);     //限定值在0~100范围内
    }
	// Update is called once per frame
	void Update () {
		
	}
}
