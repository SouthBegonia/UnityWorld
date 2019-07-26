using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
GameManager脚本：判定玩家死否死亡，若死亡则重置游戏
*/
public class GameManager : MonoBehaviour
{
    public GameObject Player;       //玩家

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        //初始化玩家对象
        Player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        //当玩家死亡
        if (Player == null)
        {
            //慢动作
            Time.timeScale = 0.3f;

            //重启游戏
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene(0);

            //StartCoroutine(StartGame());
        }
    }

    //IEnumerator StartGame()
    //{
    //    //慢动作
    //    Time.timeScale = 0.3f;
    //    yield return new WaitForSeconds(1f);

    //    //1s后 重载场景
    //    SceneManager.LoadScene(0);
    //}
}