using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
UI脚本：实现主视角下玩家的血条UI及玩家坦克外围的血条UI，胜利及失败UI 
*/
public class UI : MonoBehaviour
{
    //Canvas->Screen Space模式下的UI
    public Unit player;                 //玩家血条UI
    public Image healthBar;             //玩家血条bar
    public Text healthLabel;            //玩家血量text
    public GameObject gameOverText;     //游戏结束
    public GameObject gameWinText;      //游戏胜利
    
    //HealthCanvas->World Space模式下的UI，
    public Image healthSlider;          //坦克外侧的血条

    public GameObject blueEnemys;
    public GameObject redEnemys;

    private void Update()
    {
        //当玩家存活
        if (player)
        {
            //更新血条bar
            healthBar.fillAmount = (float)player.GetCurHealth() / (float)player.health;
            healthSlider.fillAmount = (float)player.GetCurHealth() / (float)player.health;

            //更新血量text
            healthLabel.text = player.GetCurHealth().ToString();

            //血量在特定范围内变色
            if (player.GetCurHealth() <= 70 && player.GetCurHealth() > 40)
            {
                healthLabel.color = Color.yellow;
                healthBar.color = Color.yellow;

                healthSlider.color = Color.yellow;
            }
            else if (player.GetCurHealth() <= 40)
            {
                healthLabel.color = Color.red;
                healthBar.color = Color.red;

                healthSlider.color = Color.red;
            }
        }
        else
        {
            //当玩家死后
            healthLabel.color = Color.red;
            healthLabel.text = "0";
            healthBar.fillAmount = 0f;

            healthSlider.fillAmount = 0f;

            //启用GameOver页面
            gameOverText.SetActive(true);
        }

        //当场上再无红蓝敌人，说明胜利
        if (blueEnemys.transform.childCount == 0 && redEnemys.transform.childCount == 0)
        {
            //启用Win页面
            gameWinText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene(0);
        }
    }
}