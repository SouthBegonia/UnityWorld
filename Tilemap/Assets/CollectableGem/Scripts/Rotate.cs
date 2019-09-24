using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float speed = 5;
	public Vector3 rotateDirection;

	void Update(){
		transform.Rotate (rotateDirection * speed * Time.deltaTime);
	}
}
