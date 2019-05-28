using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_2_Time : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        Debug.Log("Time.time: " + Time.time);
        Debug.Log("Time.fixedTime: " + Time.fixedTime);
        Debug.Log("Time.realtimeSinceStartup: " + Time.realtimeSinceStartup);
        Debug.Log("Time.deltaTime: " + Time.deltaTime);
        Debug.Log("Time.fixedDeltaTime: " + Time.fixedDeltaTime);
        Debug.Log("Time.frameCount: " + Time.frameCount);
        Debug.Log("Time.timeScale: " + Time.timeScale);
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log("Time.time: " + Time.time);
        Debug.Log("Time.fixedTime: " + Time.fixedTime);
        Debug.Log("Time.realtimeSinceStartup: " + Time.realtimeSinceStartup);
        Debug.Log("Time.deltaTime: " + Time.deltaTime);
        Debug.Log("Time.fixedDeltaTime: " + Time.fixedDeltaTime);
        Debug.Log("Time.frameCount: " + Time.frameCount);
        Debug.Log("Time.timeScale: " + Time.timeScale);
        Debug.Log("----------------------------");
        */

        /*  实现Cube的移动，50Hz，速度为1m/s，当缺陷在于 '/50f'，由于Update每帧用时不定
        方法1：
        Transform transform = GetComponent<Transform>();
        transform.Translate(Vector3.up / 50f);
        方法2：
        public Transform Transform;
        Transform.Translate(Vector3.up / 50f);
        方法3：
        transform.Translate(Vector3.up / 50f);

        修改：Time.deltaTime表示真实的每帧用时，在0.2s左右浮动，做到无论怎么修改帧率，移动速度永远为1m/s
        transform.Translate(Vector3.up * Time.deltaTime);
        */

        /* Time.timeScale 慢动作
        transform.Translate(Vector3.up * Time.deltaTime );
        Time.timeScale = 0.5f;
        */
        transform.Translate(Vector3.up * Time.deltaTime);
    }
}
