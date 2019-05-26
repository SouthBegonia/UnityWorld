using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSLoop2 : MonoBehaviour {
    private string[] nameArray = { "Jack", "Tom", "Rose" };

	void Start () {
		foreach(string str in nameArray)
        {
            Debug.Log(str);     //遍历数组并打印
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
