using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;

    public string backStory;

    public string playerName;
    public float health;

    [HideInInspector]
    public float maxHealth = 100;

    public float damage = 12;
    public float weaponDmg1, weaponDmg2, weaponDmg3;

    public string shoeName;
    public int shoeSize;
    public string shoeType;

    [HideInInspector]
    public int nowSkin = 0;
    public string[] skins = new string[] { "Skin 1", "Skin 2", "Skin 3", "Skin 4" };

    //[HideInInspector]
    //public GameObject[] Ornaments = new GameObject[4];

    //public GameObject obj;

    private void OnEnable()
    {

    }

    private void Start()
    {
        health = maxHealth;
        //Ornaments = new GameObject[4]
        //{
        //new GameObject("饰品1"),
        //new GameObject("饰品2"),
        //new GameObject("饰品3"),
        //new GameObject("饰品4"),
        //};
    }

    private void Update()
    {

    }

    public void SetPlayerSkin()
    {
        //Renderer meshTemp = new Renderer();
        //switch(nowSkin)
        //{
        //    case 0:
        //        meshTemp.material.color = Color.black;
        //        break;
        //    case 1:
        //        meshTemp.material.color = Color.red;
        //        break;
        //    case 2:
        //        meshTemp.material.color = Color.yellow;
        //        break;
        //    case 3:
        //        meshTemp.material.color = Color.green;
        //        break;
        //}
        //playerMeshRenderer = meshTemp;
    }

    public void ResetPlayer()
    {
        id = 9527;
        playerName = "Joker";
        health = 80;
        damage = 12;
        shoeName = "金猴";
        shoeSize = 30;
        shoeType = "皮鞋";
        weaponDmg1 = 10;
        weaponDmg2 = 15;
        weaponDmg3 = 20;
        backStory = "没啥可讲的故事...";
        SetPlayerSkin();
    }
}