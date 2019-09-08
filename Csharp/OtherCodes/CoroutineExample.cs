using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExample : MonoBehaviour {

	/* 协同程序返回类型必须为 IEnumerator，且 yield 用yield return 代替 */

    
	IEnumerator Start () {
        print("Starting " + Time.time);
        
        //启动协同程序用 StartCoroutine函数
        yield return StartCoroutine(WaitAndPrint());
        print("Done " + Time.time);
	}
	
    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(3f);
        print("WaitAndPrint " + Time.time);
    }
}
