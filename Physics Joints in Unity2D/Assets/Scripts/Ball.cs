using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    private Vector3 pos;                // Ball initial position

	// Use this for initialization
	void Start () {
        // Save ball position
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // If the ball gets off the screen
        if (transform.position.y < -8)
        {
            // stop forces affecting the rigidbody
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            // reset position
            transform.position = pos;
            // activate forces affecting the rigidbody
            this.GetComponent<Rigidbody2D>().isKinematic = false;
        }
	}
}
