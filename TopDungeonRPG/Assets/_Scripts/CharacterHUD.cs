using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//左上角的UI:包括当前等级,生命值,经验值
public class CharacterHUD : MonoBehaviour
{
    public RectTransform healthBar;     //生命条
    public RectTransform xpBar;         //经验条
    public Text level;                  //等级text

    private void Start()
    {
        //初始时刷新1次,否则出现经验值等于存档不符
        UpdateHUD();
    }


    //Menu菜单更新函数:
    public void UpdateHUD()
    {
        //更新人物等级
        level.text = GameManager.instance.GetCurrentLevel().ToString();

        //更新healthBar
        float ratio = (float)GameManager.instance.player.hitPoint / (float)GameManager.instance.player.maxHitPoint;
        healthBar.localScale = new Vector3(ratio, 1, 1);

        //更新XpBar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (currentLevel == GameManager.instance.xpTable.Count)
        {
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXP = GameManager.instance.GetXPToLevel(currentLevel - 1);
            int currLevelXP = GameManager.instance.GetXPToLevel(currentLevel);

            int diff = currLevelXP - prevLevelXP;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXP;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
        }
    }
}
