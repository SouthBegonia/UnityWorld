using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSwitch : MonoBehaviour {
    private string player = "Jack";

	void Start () {
		switch(player)
        {
            case "Tom":
                Debug.Log("This is Tom");
                break;
            case "Jack":
                Debug.Log("Hi, Jack");
                break;
            case "Rose":
                Debug.Log("Nice to meet you");
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
