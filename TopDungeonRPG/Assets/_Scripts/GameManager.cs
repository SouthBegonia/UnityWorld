using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    //资源:玩家/武器Sprite,武器价格,xpTable
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //各类引用:玩家,武器,floatingTextManager
    public Player player;
    public Weapon weapon;
    public FloatingTextManager FloatingTextManager;

    //金钱,经验
    public int pesos;
    public int experience;

    //通用显示Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //
    public bool TryUpgradeWeapon()
    {
        //若武器等级已经达到最高,则无法再升级
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        //若金钱足够,则进行升级
        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();

            return true;
        }
        return false;
    }

    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        //s += "0" + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode sceneMode)
    {
        if (!PlayerPrefs.HasKey("Savestate"))
            return;

        string[] data = PlayerPrefs.GetString("SavaState").Split('|');
        /* "10|20|30|5"
         * => "10" "20" "30" "5"
         */

        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        weapon.SetWeaponLevel(int.Parse(data[3]));

        //SceneManager.sceneLoaded -= LoadState;
        //Debug.Log("LoadState");
    }
}
