using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGetComponent : MonoBehaviour {

    /*声明游戏对象*/
    GameObject player;


    /*方法1:
     * 先声明组件变量 script, t
     * 在Start()或Update()内把得到的组件保存在组件变量
     * 注意：Getcomponent函数较耗时，应避免在Update()内使用
     */
    public Example script;
    public Transform t;


	void Start () {
        /*通过名字(本例游戏对象名 Player，名称可改)来查找游戏对象*/
        if (player = GameObject.Find("Player"))
            Debug.Log("GameObject.Find successfully!");



        /*访问游戏对象的组件:如Transform，或者脚本 Example .方法1:*/
        if (script = GetComponent<Example>())
            Debug.Log("GetComponent <Example> Successfully!");
        if (t = GetComponent<Transform>())
            Debug.Log("GetComponent <Transform> Successfully!");

        /*方法2:
         *Example script  = GetComponent<Example>();    //得到GameObject上的Example脚本组建
         * script = DoSomething();      //访问组件变量
         */
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}
