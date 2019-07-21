using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour
{
    //协程功能主要利用了C#的迭代器
    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(5);
        print("WaitAndPrint " + Time.time);
    }

    //在WWW加载完后继续执行的情况
    IEnumerator TestWWW()
    {
        string url = "www.google.com";
        WWW www = new WWW(url);
        yield return www;
    }

    IEnumerator Start()
    {
        print("Starting " + Time.time);

        //链接协程，先执行WaitAndPrint
        yield return StartCoroutine("WaitAndPrint");
        print("Done " + Time.time);
    }
}
