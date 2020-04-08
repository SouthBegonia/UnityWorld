using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


/// <summary>
/// 为Player脚本的Inspector页面进行定制
/// </summary>
[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
    Player player;
    bool showWeapons;
    bool showSkinSel;
    bool showShoe;
    bool showOrnaments;


    private void OnEnable()
    {
        // 获取当前自定义Inspector的对象
        player = (Player)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        // 设置页面布局方向：垂直/水平
        EditorGUILayout.BeginVertical();


        // 绘制Player的基本信息
        GUIStyle PlayerInfoStyle = new GUIStyle()
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
        EditorGUILayout.LabelField(new GUIContent("---基础信息---", "Player的基本信息"), PlayerInfoStyle);
        player.id = EditorGUILayout.IntField("Player ID", player.id);
        player.playerName = EditorGUILayout.TextField("Player Name", player.playerName);


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        // 绘制Player的背景故事
        GUIStyle PlayerBackStoryStyle = new GUIStyle()
        {
            fontSize = 15,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            wordWrap = true,
        };
        EditorGUILayout.LabelField(new GUIContent("---背景故事---", "描述Player的故事"), PlayerBackStoryStyle);
        player.backStory = EditorGUILayout.TextArea(player.backStory, GUILayout.MinHeight(50));

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // 滑块绘制Player的health
        GUIStyle dataStyle = new GUIStyle()
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
        EditorGUILayout.LabelField("---数值信息---", dataStyle);
        player.health = EditorGUILayout.Slider("Health", player.health, 0, 100);

        // 根据生命值设置生命条的背景颜色
        if (player.health / player.maxHealth < 0.3)
            GUI.color = Color.red;
        else
            GUI.color = Color.green;

        // 指定生命值的宽高
        Rect progressRect = GUILayoutUtility.GetRect(50, 20);

        // 绘制生命条
        EditorGUI.ProgressBar(progressRect, player.health / 100.0f, "Health");
        GUI.color = Color.white;

        // 滑块绘制伤害值
        player.damage = EditorGUILayout.Slider("Damage", player.damage, 0, 20);
        if (player.damage < 10)
            EditorGUILayout.HelpBox("伤害太低！", MessageType.Error);
        else if (player.damage > 15)
            EditorGUILayout.HelpBox("伤害太高！", MessageType.Warning);


        // 绘制武器信息：设置内容折叠
        showWeapons = EditorGUILayout.Foldout(showWeapons, "武器");
        if (showWeapons)
        {
            player.weaponDmg1 = EditorGUILayout.FloatField("Weapon 1 Damage", player.weaponDmg1);
            player.weaponDmg2 = EditorGUILayout.FloatField("Weapon 2 Damage", player.weaponDmg2);
            player.weaponDmg3 = EditorGUILayout.FloatField("Weapon 3 Damage", player.weaponDmg3);
        }


        // 绘制选择皮肤页面
        showSkinSel = EditorGUILayout.Foldout(showSkinSel, "皮肤");
        if (showSkinSel)
        {
            player.nowSkin = GUILayout.SelectionGrid(player.nowSkin, player.skins, 1);
            //player.playerSkins =  EditorGUI.ObjectField(new Rect(0, 0, 30, 30),player.playerSkins,typeof(GameObject),);
        }


        // 绘制鞋子信息
        showShoe = EditorGUILayout.Foldout(showShoe, "鞋子");
        if (showShoe)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(50));
            player.shoeName = EditorGUILayout.TextField(player.shoeName);
            EditorGUILayout.LabelField("Size", GUILayout.MaxWidth(50));
            player.shoeSize = EditorGUILayout.IntField(player.shoeSize);
            EditorGUILayout.LabelField("Type", GUILayout.MaxWidth(50));
            player.shoeType = EditorGUILayout.TextField(player.shoeType);
            EditorGUILayout.EndHorizontal();
        }

        //// 饰品
        //showOrnaments = EditorGUILayout.Foldout(showOrnaments, "饰品");
        //if (showOrnaments)
        //{
        //    for (int i = 0; i < player.Ornaments.Length; i++)
        //    {
        //        //Debug.Log(i);
        //        player.Ornaments[i] = (GameObject)EditorGUILayout.ObjectField("kkk", player.Ornaments[i], typeof(GameObject), true);
        //    }
        //}
        //player.obj  = (GameObject)EditorGUILayout.ObjectField("Buggy Game Object", player.obj, typeof(GameObject), true);

        EditorGUILayout.Space(20);


        // 绘制按钮重置信息
        if (GUILayout.Button("重置Player信息"))
        {
            player.ResetPlayer();
        }

        EditorGUILayout.EndVertical();
    }
}
