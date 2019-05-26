using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTrans1 : MonoBehaviour {
    public float SpeedOfTranslate = 1.0f;

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {

        /*Time.deltaTime 表示距上一次调用所用的时间*/
        transform.Translate(Vector3.forward * Time.deltaTime * SpeedOfTranslate);
	}
}
