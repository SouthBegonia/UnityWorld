using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    private int currentCharacterSelection = 0;          //当前选择的PlayerSprite序号
    public Image characterSprite;                       //Player的Sprite
    public Image weaponSprite;                          //Weapon的Sprite
    public RectTransform xpBar;                         //经验条组件

    //人物选择按钮:
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
        }
    }

    //替换所选择的Sprite函数:
    private void OnSelectionChanged()
    {
        characterSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    //武器升级按钮:
    public void OnUpgradeClick()
    {
        //若判断结果为true,代表武器可以升级
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    //Menu更新函数:
    public void UpdateMenu()
    {
        //更新武器sprite及升级所需金币
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        //若当前武器的等级已经达到武器价格表长度,即说明已经升级到最高等级,否则更新显示升级当前武器所需价格
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        //更新人物等级
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoint.ToString() + " /" + GameManager.instance.player.maxHitPoint;
        pesosText.text = GameManager.instance.pesos.ToString();

        //更新XpBar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (currentLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total exprience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXP = GameManager.instance.GetXPToLevel(currentLevel-1);
            int currLevelXP = GameManager.instance.GetXPToLevel(currentLevel);

            int diff = currLevelXP - prevLevelXP;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXP;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }
}
