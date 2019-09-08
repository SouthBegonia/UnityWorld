using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float num1 = 2f, num2 = 3f;
        float multiply, sum;
        Calculate(num1, num2, out multiply, out sum);
        Debug.Log(multiply);    //输出 6
        Debug.Log(sum);     //输出 5
	}
	
    void Calculate(float num1, float num2, out float multiply, out float sum)
    {
        multiply = num1 * num2;
        sum = num1 + num2;
    }
}
