using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //资源:玩家/武器Sprite,武器价格,xpTable
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //各类引用:玩家,武器,floatingTextManager
    public Player player;
    public Weapon weapon;
    public CharacterMenu menu;
    public FloatingTextManager FloatingTextManager;
    public RectTransform hitpointBar;
    //public RectTransform xpBar;

    //金钱,经验
    public int pesos;
    public int experience;


    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(FloatingTextManager.gameObject);
            return;
        }

        instance = this;

        //执行加载存档
        SceneManager.sceneLoaded += LoadState;

        //保留的物体
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("FloatingTextManager").transform.parent);
        DontDestroyOnLoad(GameObject.Find("Menu"));
        DontDestroyOnLoad(GameObject.Find("HUD"));
    }

    //通用显示Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //判断武器是否能够升级
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

    //XP升级系统
    public int GetCurrentLevel()
    {
        int l = 0, add = 0;
        while (experience >= add)
        {
            add += xpTable[l];
            l++;

            if (l == xpTable.Count)
                return l;
        }
        return l;
    }
    public int GetXPToLevel(int level)
    {
        int l = 0, xp = 0;
        while (l < level)
        {
            xp += xpTable[l];
            l++;
        }
        return xp;
    }
    public void GrantXP(int xp)
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;

        menu.UpdateMenu();

        if (currentLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        ShowText("LEVEL UP!", 30, Color.yellow, player.transform.position, Vector3.up * 30, 2.0f);
        //("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        //Debug.Log("LevelUP");
        player.OnLevelUp();
    }

    //生命值Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitpointBar.localScale = new Vector3(ratio, 1, 1);
    }


    //存储游戏数值信息函数:
    public void SaveState()
    {
        Debug.Log("SaveState");

        //游戏数值载体s
        string s = "";

        //以s字符串为载体储存游戏数值信息
        //'|'为间隔符,区分开各类游戏数值
        s += "0" + "|";                     //data[0]
        s += pesos.ToString() + "|";        //data[1]
        s += experience.ToString() + "|";   //data[2]
        s += weapon.weaponLevel.ToString(); //data[3]

        //存储游戏信息字符串
        PlayerPrefs.SetString("SaveState", s);      
    }

    //加载存储的游戏数值的函数:金币,经验,等级,武器,场景出生地等
    public void LoadState(Scene s, LoadSceneMode sceneMode)
    {
        Debug.Log("LoadState");

        //备注:是Save不是Sava,千万别写错,否则找不到
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        //取得各游戏信息到data[]内
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        //    s: "10|20|30|5"   => "10" "20" "30" "5"
        //Debug.Log("data:" + data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3]);

        //加载金币
        pesos = int.Parse(data[1]);

        //加载经验及等级
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        //加载武器
        weapon.SetWeaponLevel(int.Parse(data[3]));
              
        //加载场景出生地
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
