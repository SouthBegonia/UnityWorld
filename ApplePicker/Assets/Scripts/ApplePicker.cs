using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplePicker : MonoBehaviour
{
    public GameObject basketPrefab;
    public int numBaskets = 3;          //篮筐数
    public float basketBottY = -14f;    //第三个篮筐底坐标
    public float basketSpacingY = 2f;   //每个篮筐间隔
    public List<GameObject> basketList; //篮筐的表
    public GameObject endUI;            //游戏结束界面UI
    //public GameObject appleTree_state;  //苹果树启用状态
    //public GameObject apple_state;      //苹果启用状态

    private void Start()
    {
        //初始化产生3各篮筐(位置，表)
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGo = Instantiate(basketPrefab) as GameObject;
            Vector3 pos = Vector3.zero;
            pos.y = basketBottY + (basketSpacingY * i);
            tBasketGo.transform.position = pos;

            basketList.Add(tBasketGo);
        }

        //初始时隐藏GameOver界面
        endUI.SetActive(false);
    }

    //消除所有苹果(包括下落中的)，及销毁篮筐
    public void AppleDestroyed()
    {
        //遍历当前存在的苹果到表 tAppleArray 内
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");

        //销毁所有的苹果
        foreach (GameObject tGo in tAppleArray)
            Destroy(tGo);

        //篮筐自顶往下销毁
        int basketIndex = basketList.Count - 1;
        GameObject tBasketGo = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGo);

        //游戏结束，重新开始
        if (basketList.Count == 0)
        {
            //启用GameOver页面
            endUI.SetActive(true);

            /* 此处本打算游戏结束时禁止树与苹果的脚本，但由于AppleTree.cs内 InvokeRepeating() 的存在暂无法实现
            SetGameState(false);
            */

            //3s后重新开始游戏
            Invoke("ReloadGame", 3f);
        }
    }

    //重新加载游戏
    private void ReloadGame()
    {
        SceneManager.LoadScene("Scene_0");
    }
}


