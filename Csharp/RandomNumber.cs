using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour {

    public float num = 0.0f;
    public float Min = 1.0f;
    public float Max = 10.0f;

	// Use this for initialization
	void Start () {
        print("Before Random: " + num);

        num = Random.Range(Min, Max);

        print("After Random: " + num);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
