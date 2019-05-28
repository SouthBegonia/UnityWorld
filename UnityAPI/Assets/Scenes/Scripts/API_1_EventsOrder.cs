using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_1_EventsOrder : MonoBehaviour
{   
    /*------------*/
    private void Awake()
    {
        //即便脚本被禁用，也会在scene加载后执行
        print("Awake()");
    }

    private void OnEnable()
    {
        print("OnEnable()");
    }

    private void Start()
    {
        print("Start()");
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.mass = 10f;
        rb.AddForce(Vector3.up * 100f);
    }

    /*------------*/
    private void Reset()
    {
        print("Reset()");
    }

    /*------------*/
    private void FixedUpdate()
    {
        print("FixUpdate");
    }

    private void Update()
    {
        print("Update");
    }

    private void LateUpdate()
    {
        print("LateUpdate");
    }
    
    /*事例
    void Update()
    {
        Debug.Log("在Update中执行");
        Debug.Log("time:" + Time.time);
        Debug.Log("deltatime" + Time.deltaTime);
        Debug.Log("fixedtime:" + Time.fixedTime);
        Debug.Log("fixedDeltatimetime:" + Time.fixedDeltaTime);
    }
    void FixedUpdate()
    {
        Debug.Log("在fixedUpdate中执行");
        Debug.Log("time:" + Time.time);
        Debug.Log("deltatime" + Time.deltaTime);
        Debug.Log("fixedtime:" + Time.fixedTime);
        Debug.Log("fixedDeltatimetime:" + Time.fixedDeltaTime);
    }
    */
}
