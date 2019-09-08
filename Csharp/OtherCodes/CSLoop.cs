using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSLoop : MonoBehaviour {
    private int i = 0;
    private int x = 0;
    private int y = 0;

	void Start () {
		while(i<10)
        {
            Debug.Log(i);
            i++;
        }

        for(; x<10;++x)
        {
            Debug.Log(x);
        }

        do
        {
            Debug.Log(y);
            ++y;
        } while (y < 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
