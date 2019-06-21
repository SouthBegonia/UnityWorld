using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Unit player;         //玩家血条UI
    public Image healthBar;     //玩家血条bar
    public Text healthLabel;    //玩家血量text

    private void Update()
    {
        //更新血条bar
        healthBar.fillAmount = (float)player.GetCurHealth() / (float)player.health;

        //更新血量text
        healthLabel.text = player.GetCurHealth().ToString();
    }
}
